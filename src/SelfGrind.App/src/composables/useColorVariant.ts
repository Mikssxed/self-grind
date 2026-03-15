export type ColorVariant = 'error' | 'info' | 'violet' | 'warning' | 'success';

export const textClasses: Record<ColorVariant, string> = {
    error: 'text-error-500',
    info: 'text-info-500',
    violet: 'text-violet-500',
    warning: 'text-warning-500',
    success: 'text-success-500',
};

export const borderLeftClasses: Record<ColorVariant, string> = {
    error: 'border-l-error-500',
    info: 'border-l-info-500',
    violet: 'border-l-violet-500',
    warning: 'border-l-warning-500',
    success: 'border-l-success-500',
};
