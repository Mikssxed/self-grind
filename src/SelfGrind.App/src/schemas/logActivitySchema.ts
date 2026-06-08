import { object, string, number, enum as zEnum, optional } from 'zod';

export const logActivitySchema = object({
    activityName: string().min(1, 'Activity name is required').max(100),
    notes: optional(string().max(500)),
    exp: number().min(5).max(500),
    attribute: zEnum(['Strength', 'Knowledge', 'Health', 'Discipline', 'Focus', 'Energy'], {
        required_error: 'Attribute is required',
    }),
});
