import type { LoginResponseApiOperationResult } from '@/api/apiClient/models';
import { useAuthStore } from '@/stores/useAuthStore';

let refreshPromise: Promise<string | null> | null = null;

async function requestNewTokens(): Promise<string | null> {
    const authStore = useAuthStore();
    const refreshToken = authStore.tokenData?.refreshToken;
    if (!refreshToken) return null;

    const response = await fetch('/api/identity/refresh', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ refreshToken }),
    });
    if (!response.ok) return null;

    const result: LoginResponseApiOperationResult = await response.json();
    if (!result.isSuccess || !result.data) return null;

    authStore.setTokens({
        accessToken: result.data.accessToken,
        refreshToken: result.data.refreshToken,
        tokenType: result.data.tokenType,
        expiresIn: result.data.expiresIn,
    });
    return authStore.getAuthHeader();
}

// Concurrent callers share a single refresh request so the rotated token is not invalidated twice
export function refreshTokensOnce(): Promise<string | null> {
    if (!refreshPromise) {
        refreshPromise = requestNewTokens()
            .catch(() => null)
            .finally(() => {
                refreshPromise = null;
            });
    }
    return refreshPromise;
}
