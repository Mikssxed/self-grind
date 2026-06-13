import {
    DayOfWeekObject,
    type BaseAttribute,
    type DayOfWeek,
    type TaskItemDto,
    type TaskRepetitionType,
} from '@/api/apiClient/models';
import { DateOnly } from '@microsoft/kiota-abstractions';

// Day-of-week index (0 = Sunday … 6 = Saturday) <-> generated enum, kept in one place so the
// Add and Edit task forms map identically. Object.values preserves the enum's declared order.
const dayIndexToEnum = Object.values(DayOfWeekObject) as DayOfWeek[];

/** The shape the task form (vee-validate) works with — dates as JS `Date`, days as 0-6 indices. */
export interface TaskFormValues {
    title: string;
    description: string;
    repetitionType: TaskRepetitionType;
    exp: number;
    attribute: BaseAttribute | undefined;
    daysOfWeek: number[];
    taskDate: Date | undefined;
    startDate: Date | undefined;
    endDate: Date | undefined;
    repeatInterval: number;
}

/** The command body shared by Create and Update (Update adds `id` on top). */
export interface TaskCommandFields {
    title: string;
    description: string;
    repetitionType: TaskRepetitionType;
    exp: number;
    attribute: BaseAttribute | null;
    daysOfWeek: DayOfWeek[] | null;
    startDate: DateOnly | null;
    endDate: DateOnly | null;
    repeatInterval: number;
}

/** Blank form state for creating a new task. */
export function emptyTaskFormValues(): TaskFormValues {
    return {
        title: '',
        description: '',
        repetitionType: 'Daily',
        exp: 10,
        attribute: undefined,
        daysOfWeek: [],
        taskDate: undefined,
        startDate: undefined,
        endDate: undefined,
        repeatInterval: 1,
    };
}

function dateOnlyToDate(value: DateOnly | null | undefined): Date | undefined {
    if (!value) return undefined;
    return new Date(value.year, value.month - 1, value.day);
}

const isOnce = (type: TaskRepetitionType) => type === 'Once';

/** Prefills the form from an existing task (used by the edit flow). */
export function taskToFormValues(task: TaskItemDto): TaskFormValues {
    const repetitionType = (task.repetitionType ?? 'Daily') as TaskRepetitionType;
    const once = isOnce(repetitionType);
    const start = dateOnlyToDate(task.startDate);

    return {
        title: task.title ?? '',
        description: task.description ?? '',
        repetitionType,
        exp: task.exp ?? 10,
        attribute: task.attribute ?? undefined,
        daysOfWeek: (task.daysOfWeek ?? [])
            .map((d) => dayIndexToEnum.indexOf(d))
            .filter((i) => i >= 0),
        taskDate: once ? start : undefined,
        startDate: once ? undefined : start,
        endDate: once ? undefined : dateOnlyToDate(task.endDate),
        repeatInterval: task.repeatInterval ?? 1,
    };
}

/** Builds the create/update command body from validated form values. */
export function buildTaskCommandFields(values: TaskFormValues): TaskCommandFields {
    const once = isOnce(values.repetitionType);

    const startSource = once ? values.taskDate : values.startDate;
    const endSource = once ? values.taskDate : values.endDate;

    return {
        title: values.title,
        description: values.description,
        repetitionType: values.repetitionType,
        exp: values.exp,
        attribute: values.attribute ?? null,
        daysOfWeek:
            values.daysOfWeek
                ?.map((n) => dayIndexToEnum[n])
                .filter((d): d is DayOfWeek => d !== undefined) ?? null,
        startDate: startSource ? DateOnly.fromDate(startSource) : null,
        endDate: endSource ? DateOnly.fromDate(endSource) : null,
        repeatInterval: values.repeatInterval ?? 1,
    };
}
