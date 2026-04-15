import { object, string, number, enum as zEnum, optional } from 'zod';

export const logActivitySchema = object({
    activityName: string().min(1, 'Activity name is required'),
    notes: optional(string()),
    duration: optional(string()),
    exp: number().min(5).max(50),
    attribute: optional(zEnum(['Strength', 'Knowledge', 'Health', 'Charisma', 'Focus', 'Creativity'])),
});
