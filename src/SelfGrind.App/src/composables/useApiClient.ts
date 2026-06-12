import { createApiClient } from '@/api/apiClient/apiClient';
import { BearerAuthenticationProvider } from '@/api/BearerAuthenticationProvider';
import { refreshTokensOnce } from '@/api/tokenRefresh';
import router from '@/router';
import { useAuthStore } from '@/stores/useAuthStore';
import { FetchRequestAdapter, KiotaClientFactory } from '@microsoft/kiota-http-fetchlibrary';

let apiClientInstance: ReturnType<typeof createApiClient> | null = null;

function isIdentityRequest(url: string): boolean {
    return new URL(url, window.location.origin).pathname.startsWith('/api/identity/');
}

async function fetchWithTokenRefresh(request: string, init: RequestInit): Promise<Response> {
    const response = await fetch(request, init);
    if (response.status !== 401 || isIdentityRequest(request)) return response;

    const authHeader = await refreshTokensOnce();
    if (!authHeader) {
        const authStore = useAuthStore();
        authStore.clearTokens();
        await router.push({ name: 'login', query: { returnUrl: router.currentRoute.value.fullPath } });
        return response;
    }

    const headers = new Headers(init.headers);
    headers.set('Authorization', authHeader);
    return fetch(request, { ...init, headers });
}

export function useApiClient() {
    if (!apiClientInstance) {
        const authProvider = new BearerAuthenticationProvider();
        const httpClient = KiotaClientFactory.create(fetchWithTokenRefresh);
        const requestAdapter = new FetchRequestAdapter(authProvider, undefined, undefined, httpClient);

        apiClientInstance = createApiClient(requestAdapter);
    }

    return apiClientInstance;
}
