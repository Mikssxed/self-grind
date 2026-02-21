import { useAuthStore } from '@/stores/useAuthStore';
import type { AuthenticationProvider, RequestInformation } from '@microsoft/kiota-abstractions';

export class BearerAuthenticationProvider implements AuthenticationProvider {
    public async authenticateRequest(
        request: RequestInformation
        // additionalAuthenticationContext?: Record<string, unknown>
    ): Promise<void> {
        const authStore = useAuthStore();
        const authHeader = authStore.getAuthHeader();

        if (authHeader && !request.headers.has('Authorization')) {
            request.headers.add('Authorization', authHeader);
        }
    }
}
