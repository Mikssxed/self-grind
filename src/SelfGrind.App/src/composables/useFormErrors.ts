import type { ApiOperationResult } from '@/api/apiClient/models';
import { ref, type Ref } from 'vue';

export interface FormErrors {
    general: Ref<string | null>;
    fieldErrors: Ref<Record<string, string>>;
    setErrors: (errors: ApiOperationResult['errors'] | null | undefined) => void;
    createFieldErrorSetter: <T extends string>(
        setFieldError: (field: T, message: string | string[] | undefined) => void
    ) => (field: string, message: string) => void;
    clearErrors: () => void;
}

/**
 * Composable to handle form errors from API responses
 */
export function useFormErrors(): FormErrors {
    const general = ref<string | null>(null);
    const fieldErrors = ref<Record<string, string>>({});

    /**
     * Parse API operation result errors and extract general and field-specific errors
     */
    const setErrors = (errors: ApiOperationResult['errors'] | null | undefined) => {
        // Reset errors
        general.value = null;
        fieldErrors.value = {};

        if (!errors) return;

        // Get the additional data from the errors object
        const additionalData = errors.additionalData;
        if (!additionalData) return;

        // Process each error key
        for (const [fieldName, value] of Object.entries(additionalData)) {
            // value should be an array of strings
            const errorMessages = Array.isArray(value) ? value : [String(value)];
            const errorMessage = errorMessages[0] || '';

            if (fieldName === 'general') {
                general.value = errorMessage;
            } else {
                fieldErrors.value[fieldName] = errorMessage;
            }
        }
    };

    /**
     * Create a type-safe wrapper for setFieldError that works with dynamic field names from backend
     */
    const createFieldErrorSetter = <T extends string>(
        setFieldError: (field: T, message: string | string[] | undefined) => void
    ) => {
        return (field: string, message: string) => {
            // Cast is safe here because we're wrapping the strictly-typed function
            // and the caller (backend) may send any field name
            setFieldError(field as T, message);
        };
    };

    const clearErrors = () => {
        general.value = null;
        fieldErrors.value = {};
    };

    return {
        general,
        fieldErrors,
        setErrors,
        createFieldErrorSetter,
        clearErrors,
    };
}
