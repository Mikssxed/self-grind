import { object, string, number, enum as zEnum, optional, array, date } from 'zod';

export const addTaskSchema = object({
    title: string().min(1, 'Task title is required'),
    description: string().min(1, 'Description is required').max(500, 'Description must be 500 characters or less'),
    repetitionType: zEnum(['Daily', 'Weekly', 'Once']),
    exp: number().min(5).max(100),
    attribute: zEnum(['Strength', 'Knowledge', 'Health', 'Charisma', 'Focus', 'Creativity'], {
        errorMap: () => ({ message: 'Select a character attribute' }),
    }),
    daysOfWeek: optional(array(number().min(0).max(6))),
    taskDate: optional(date()),
    startDate: optional(date()),
    endDate: optional(date()),
    repeatInterval: optional(number().int().min(1).max(7)),
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
    if (data.repetitionType === 'Daily' || data.repetitionType === 'Weekly') {
        if (!data.startDate) {
            ctx.addIssue({ code: 'custom', message: 'Start date is required', path: ['startDate'] });
        }
        if (!data.endDate) {
            ctx.addIssue({ code: 'custom', message: 'End date is required', path: ['endDate'] });
        }
        if (data.startDate && data.endDate && data.endDate < data.startDate) {
            ctx.addIssue({ code: 'custom', message: 'End date must be on or after start date', path: ['endDate'] });
        }
    }
});
