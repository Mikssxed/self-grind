import { object, string } from 'zod';

export const registerSchema = object({
    username: string().min(3, 'Username must be at least 3 characters long'),
    email: string().email('Invalid email address'),
    password: string()
        .min(6, 'Password must be at least 6 characters long')
        .regex(/[0-9]+/, 'Password must contain at least one number')
        .regex(/[a-zA-Z]+/, 'Password must contain at least one letter')
        .regex(/[!@#$%^&*(),.?":{}|<>]+/, 'Password must contain at least one special character'),
    confirmPassword: string().min(6, 'Confirm Password must be at least 6 characters long'),
}).refine((data) => data.password === data.confirmPassword, {
    message: 'Passwords do not match',
    path: ['confirmPassword'],
});
