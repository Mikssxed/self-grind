import type { LoginUserCommand, RegisterUserCommand } from '@/api/apiClient/models';
import { useAuthStore } from '@/stores/useAuthStore';
import { useMutation } from '@tanstack/vue-query';
import { useRoute, useRouter } from 'vue-router';
import { useApiClient } from './useApiClient';

export function useRegisterMutation() {
    const apiClient = useApiClient();
    const router = useRouter();

    return useMutation({
        mutationFn: async (data: RegisterUserCommand) => {
            await apiClient.api.identity.register.post(data);
        },
        onSuccess: () => {
            // Navigate to a confirmation page or login
            router.push({ name: 'login', query: { registered: 'true' } });
        },
    });
}

export function useLoginMutation() {
    const apiClient = useApiClient();
    const authStore = useAuthStore();
    const router = useRouter();
    const route = useRoute();

    return useMutation({
        mutationFn: async (data: LoginUserCommand) => {
            const response = await apiClient.api.identity.login.post(data);
            if (!response) {
                throw new Error('No response from login');
            }
            return response;
        },
        onSuccess: (data) => {
            authStore.setTokens({
                accessToken: data.data?.accessToken ?? '',
                refreshToken: data.data?.refreshToken ?? '',
                tokenType: data.data?.tokenType ?? 'Bearer',
                expiresIn: data.data?.expiresIn ?? 3600,
            });

            const returnUrl = route.query.returnUrl as string | undefined;
            router.push(returnUrl || { name: 'home' });
        },
    });
}

export function useLogout() {
    const authStore = useAuthStore();
    const router = useRouter();

    return () => {
        authStore.clearTokens();
        router.push({ name: 'login' });
    };
}
