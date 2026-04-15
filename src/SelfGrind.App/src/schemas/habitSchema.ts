import { object, string, number } from 'zod';

export const habitSchema = object({
    name: string().min(1, 'Habit name is required'),
    targetValue: number().min(1).max(20),
    unit: string(),
});
