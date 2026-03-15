# Frontend Rules

## Project Structure

```
src/SelfGrind.App/src/
├── api/
│   ├── apiClient/              ← Kiota-generated client (never edit manually)
│   ├── BearerAuthenticationProvider.ts
│   └── index.ts
├── components/
│   ├── base/                   ← shared UI primitives (Button, Badge, Icon, etc.)
│   ├── form/                   ← form components (TextField, FormField)
│   ├── layout/                 ← SidebarLayout, AppLayout
│   ├── icons/                  ← SVG icon components
│   └── <feature>/              ← feature-specific components per domain area
├── composables/                ← useApiClient, useAuth, useForm, useNavigation
├── router/index.ts             ← route definitions
├── schemas/                    ← Zod validation schemas
├── stores/useAuthStore.ts      ← Pinia auth store
├── views/                      ← page-level components
├── types.ts                    ← app-wide TypeScript types
├── style.css                   ← Tailwind v4 + theme tokens
└── main.ts                     ← app entry point
```

---

## Component Conventions

### Script setup with TypeScript
Always use `<script setup lang="ts">`.

### Variant props — never raw CSS or hex
Define a `variant` (and optionally `size`) prop as a union type. Map variants to **complete static class strings** using `Record`:

```typescript
export type CardVariant = 'default' | 'info' | 'violet' | 'success';

interface CardProps {
    variant?: CardVariant;
    label: string;
    value: string;
}

const props = withDefaults(defineProps<CardProps>(), {
    variant: 'default',
});

const bgClasses: Record<CardVariant, string> = {
    default: 'bg-primary-900',
    info: 'bg-info-500/30',
    violet: 'bg-violet-500/30',
    success: 'bg-success-500/30',
};

const borderClasses: Record<CardVariant, string> = {
    default: 'border-primary-600',
    info: 'border-info-500',
    violet: 'border-violet-500',
    success: 'border-success-500',
};

const containerClass = computed(() =>
    twMerge('flex flex-col p-4 rounded-xl border', bgClasses[props.variant], borderClasses[props.variant])
);
```

### Export types from child components
Parent views/components import types from children:

```typescript
// In ChildComponent.vue
export interface ChildItem {
    label: string;
    variant: CardVariant;
}

// In ParentView.vue
import type { ChildItem } from '@/components/feature/ChildComponent.vue';
const items: ChildItem[] = [{ label: 'Foo', variant: 'info' }];
```

### Computed for derived values
Use `computed()` for any derived display string, never inline template expressions:

```typescript
const progressPercent = computed(() => Math.round((props.current / props.max) * 100));
const progressLabel = computed(() => `${progressPercent.value}%`);
```

### twMerge for external class merging
Use `twMerge()` when a component accepts an external `class` prop that might conflict with internal classes:

```typescript
import { twMerge } from 'tailwind-merge';
const classes = computed(() => twMerge('base-classes', props.class));
```

### Static Tailwind classes only
Tailwind v4 scans statically. Dynamic interpolation does **not** work:

```typescript
// WRONG — class won't be generated
const cls = `bg-${props.color}-500`;

// CORRECT — full class string in Record map
const bgMap: Record<Variant, string> = {
    info: 'bg-info-500',
    error: 'bg-error-500',
};
```

---

## Color / Theme Rules

All colors are defined as tokens in `src/style.css` under `@theme`.

### Available semantic colors
- `error-500` — red/danger (#ef4444)
- `info-500` — blue (#3b82f6)
- `violet-500` — violet (#8b5cf6)
- `warning-500` — yellow (#eab308)
- `success-500` — green (#22c55e)
- `orange-500` — orange (#f97316)
- `accent-500` / `accent-700` — indigo
- `secondary-500` / `secondary-700` — purple
- `primary-900` / `primary-800` / `primary-600` / `primary-500` / `primary-400` / `primary-200`
- Dark bg variants: `orange-900`, `blue-900`, `green-900`, `purple-900`, `crimson-900`

### Opacity modifiers (preferred over RGBA)
```html
<!-- CORRECT -->
<div class="bg-info-500/30 border-b-violet-500/50">

<!-- WRONG — use theme tokens, not hex -->
<div style="background: rgba(59, 130, 246, 0.3)">
```

### Gradient utilities
```html
<div class="bg-gradient-purple">         <!-- indigo → purple -->
<div class="bg-gradient-midnight">       <!-- dark blue gradient -->
<span class="text-gradient-accent">      <!-- gradient text -->
```

### Never hardcode colors
Do not pass hex values as props or use them inline. Add new colors to `style.css @theme` first.

---

## View Conventions

- One view file per route, placed in `src/views/`
- Views compose feature components; they do not contain raw HTML UI elements
- Import types from child components using `import type`
- Use `as const` for variant literals in data arrays:

```typescript
const stats = [
    { label: 'Strength', value: 78, variant: 'error' as const },
    { label: 'Knowledge', value: 85, variant: 'info' as const },
];
```

---

## Routing Conventions

```typescript
// 1. Add view to AppViews map (lazy loaded)
export const AppViews = {
    myFeature: () => import('@/views/MyFeatureView.vue'),
    // ...
};

export type AppRouteNames = keyof typeof AppViews;

// 2. Register route using createRoute()
createRoute('/my-feature', 'myFeature', LayoutType.WITH_SIDEBAR, true)
// path, name (must match AppViews key), layoutType, requiresAuth
```

`LayoutType.WITH_SIDEBAR` — shows sidebar navigation.
`LayoutType.WITHOUT_SIDEBAR` — full-width layout (auth pages).

---

## API Client Usage

The Kiota client is a singleton factory. Never instantiate it directly:

```typescript
import { useApiClient } from '@/composables/useApiClient';

const apiClient = useApiClient();
const result = await apiClient.api.tasks.post(command);
```

After any backend endpoint change, regenerate the client:
```bash
npm run build:api   # from src/SelfGrind.App
```

---

## State Management (Pinia)

Only `useAuthStore` exists currently. Pattern for new stores:

```typescript
export const useFeatureStore = defineStore('feature', () => {
    const items = ref<Item[]>([]);

    const count = computed(() => items.value.length);

    function addItem(item: Item) {
        items.value.push(item);
    }

    return { items, count, addItem };
});
```

- Use Composition API (function syntax), not Options API
- Persist sensitive data to `localStorage` manually (see `useAuthStore`)
- Use `computed()` for derived state

---

## TypeScript Rules

- **Never use `any`** — always define proper interfaces
- Use `readonly` for props that should not be mutated
- Export component-specific types so parents can import them
- Use discriminated unions or union types for variant props
- Prefer `interface` for object shapes, `type` for unions

---

## Component File Location

| Type | Location |
|------|----------|
| Shared primitives | `src/components/base/` |
| Form inputs | `src/components/form/` |
| Layout wrappers | `src/components/layout/` |
| SVG icons | `src/components/icons/` |
| Feature components | `src/components/<feature>/` (e.g., `dashboard/`, `character/`) |
| Page views | `src/views/` |
| Auth views | `src/views/auth/` |

---

## Composables

| File | Purpose |
|------|---------|
| `useApiClient.ts` | Singleton Kiota client factory |
| `useAuth.ts` | Login/register/logout TanStack Query mutations |
| `useForm.ts` | Form submission handler with error integration |
| `useFormErrors.ts` | Field error tracking |
| `useNavigation.ts` | Navigation helper methods |
