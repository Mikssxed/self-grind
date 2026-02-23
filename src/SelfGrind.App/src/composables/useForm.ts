import type { ApiOperationResult } from '@/api/apiClient/models';
import type { UseMutationReturnType } from '@tanstack/vue-query';
import type { GenericObject, SubmissionContext, SubmissionHandler } from 'vee-validate';
import { ref } from 'vue';
import { useFormErrors } from './useFormErrors';

interface ApiErrorResponse extends Error, ApiOperationResult {}

function isApiErrorResponse(error: Error): error is ApiErrorResponse {
    return 'errors' in error && 'isSuccess' in error;
}

export function useForm<TModel extends GenericObject, TResult>(
    mutation: UseMutationReturnType<TResult, Error, TModel, unknown>,
    onSuccess?: (result: TResult) => void
) {
    const data = ref<TModel>();
    const formErrors = useFormErrors();

    const handler: SubmissionHandler<TModel> = async (
        values: TModel,
        actions: SubmissionContext<TModel>
    ) => {
        formErrors.clearErrors();
        // Clear general error in vee-validate (using type assertion for non-model field)
        (actions.setFieldError as (field: string, message: string | undefined) => void)(
            'general',
            undefined
        );
        data.value = values;

        mutation.mutate(values, {
            onError: (error: Error) => {
                if (isApiErrorResponse(error)) {
                    if (error.errors) {
                        formErrors.setErrors(error.errors);

                        // Set general error in vee-validate context
                        if (formErrors.general.value) {
                            (actions.setFieldError as (field: string, message: string) => void)(
                                'general',
                                formErrors.general.value
                            );
                        }

                        // Apply field errors to vee-validate
                        for (const [field, message] of Object.entries(
                            formErrors.fieldErrors.value
                        )) {
                            const fieldSetter = formErrors.createFieldErrorSetter(
                                actions.setFieldError
                            );
                            fieldSetter(field, message);
                        }
                    }
                } else {
                    const errorMessage = 'An error occurred. Please try again.';
                    formErrors.general.value = errorMessage;
                    (actions.setFieldError as (field: string, message: string) => void)(
                        'general',
                        errorMessage
                    );
                }
            },
            onSuccess: (result) => {
                if (onSuccess) {
                    onSuccess(result);
                }
            },
        });
    };

    return {
        data,
        result: mutation.data,
        isProcessing: mutation.isPending,
        handler,
    };
}
