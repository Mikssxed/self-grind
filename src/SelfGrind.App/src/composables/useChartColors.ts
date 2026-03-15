function getCssColor(variable: string): string {
    return getComputedStyle(document.documentElement)
        .getPropertyValue(variable)
        .trim();
}

export function useChartColors() {
    return {
        success: getCssColor('--color-success-500'),
        info: getCssColor('--color-info-500'),
        error: getCssColor('--color-error-500'),
        warning: getCssColor('--color-warning-500'),
        accent: getCssColor('--color-accent-500'),
        violet: getCssColor('--color-violet-500'),
    };
}
