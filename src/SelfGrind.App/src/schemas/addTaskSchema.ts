import { object, string, number, enum as zEnum, optional, array, date } from 'zod';

export const addTaskSchema = object({
    title: string().min(1, 'Task title is required'),
    description: optional(string()),
    repetitionType: zEnum(['Daily', 'Weekly', 'Once']),
    exp: number().min(5).max(100),
    attribute: optional(zEnum(['Strength', 'Knowledge', 'Health', 'Charisma', 'Focus', 'Creativity'])),
    daysOfWeek: optional(array(number().min(0).max(6))),
    taskDate: optional(date()),
}).superRefine((data, ctx) => {
    if (data.repetitionType === 'Weekly') {
        if (!data.daysOfWeek || data.daysOfWeek.length === 0) {
            ctx.addIssue({
                code: 'custom',
                message: 'Select at least one day',
                path: ['daysOfWeek'],
            });
        }
    }
    if (data.repetitionType === 'Once') {
        if (!data.taskDate) {
            ctx.addIssue({
                code: 'custom',
                message: 'Date is required for one-time tasks',
                path: ['taskDate'],
            });
        }
    }
});
