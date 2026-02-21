import { createApiClient } from '@/api/apiClient/apiClient';
import { BearerAuthenticationProvider } from '@/api/BearerAuthenticationProvider';
import { FetchRequestAdapter } from '@microsoft/kiota-http-fetchlibrary';

let apiClientInstance: ReturnType<typeof createApiClient> | null = null;

export function useApiClient() {
    if (!apiClientInstance) {
        const authProvider = new BearerAuthenticationProvider();
        const requestAdapter = new FetchRequestAdapter(authProvider);

        apiClientInstance = createApiClient(requestAdapter);
    }

    return apiClientInstance;
}
