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

    // An expired access token still counts as a session — the API client refreshes it on 401
    const isAuthenticated = computed(() => tokenData.value !== null);

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
        if (!tokenData.value || Date.now() >= tokenData.value.expiresAt) return null;
        return `${tokenType.value} ${accessToken.value}`;
    }

    // True when a non-null access token still has at least `bufferMs` of life left
    function isAccessTokenFresh(bufferMs = 0): boolean {
        return !!tokenData.value && Date.now() + bufferMs < tokenData.value.expiresAt;
    }

    return {
        tokenData,
        isAuthenticated,
        accessToken,
        tokenType,
        setTokens,
        clearTokens,
        getAuthHeader,
        isAccessTokenFresh,
    };
});
