import { object, string } from 'zod';

export const registerSchema = object({
    username: string().min(3, 'Username must be at least 3 characters long'),
    email: string().email('Invalid email address'),
    password: string().min(6, 'Password must be at least 6 characters long'),
    confirmPassword: string().min(6, 'Confirm Password must be at least 6 characters long'),
});
