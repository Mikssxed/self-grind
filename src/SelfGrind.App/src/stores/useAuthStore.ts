import { defineStore } from 'pinia';
import { computed, ref } from 'vue';

interface TokenData {
    accessToken: string;
    refreshToken: string;
    tokenType: string;
    expiresIn: number;
    expiresAt: number;
}

export const useAuthStore = defineStore('auth', () => {
    const tokenData = ref<TokenData | null>(null);

    // Load token from localStorage on initialization
    const storedToken = localStorage.getItem('auth_token');
    if (storedToken) {
        try {
            tokenData.value = JSON.parse(storedToken);
        } catch (e) {
            localStorage.removeItem('auth_token');
        }
    }

    const isAuthenticated = computed(() => {
        if (!tokenData.value) return false;
        // Check if token is expired
        return Date.now() < tokenData.value.expiresAt;
    });

    const accessToken = computed(() => tokenData.value?.accessToken ?? null);
    const tokenType = computed(() => tokenData.value?.tokenType ?? 'Bearer');

    function setTokens(data: {
        accessToken: string;
        refreshToken: string;
        tokenType: string;
        expiresIn: number;
    }) {
        const expiresAt = Date.now() + data.expiresIn * 1000;
        const newTokenData: TokenData = {
            ...data,
            expiresAt,
        };
        tokenData.value = newTokenData;
        localStorage.setItem('auth_token', JSON.stringify(newTokenData));
    }

    function clearTokens() {
        tokenData.value = null;
        localStorage.removeItem('auth_token');
    }

    function getAuthHeader(): string | null {
        if (!isAuthenticated.value || !accessToken.value) return null;
        return `${tokenType.value} ${accessToken.value}`;
    }

    return {
        tokenData,
        isAuthenticated,
        accessToken,
        tokenType,
        setTokens,
        clearTokens,
        getAuthHeader,
    };
});
