# SelfGrind Frontend

Vue 3 + TypeScript + Vite frontend for the SelfGrind task management application.

## Quick Start

```bash
# Install dependencies
npm install

# Start development server (make sure backend is running first!)
npm run dev

# Build for production
npm run build
```

## Prerequisites

- Node.js 18+ and npm
- Backend API running on `http://localhost:5081`

For complete setup instructions, see the [main README](../../README.md) in the project root.

## Development

The frontend runs on `http://localhost:5173` and automatically proxies API requests to the backend.

### Regenerate API Client

After making changes to the backend API:

```bash
npm run build:api
```

This will:

1. Extract OpenAPI spec from the backend
2. Generate a TypeScript client using Microsoft Kiota

## Tech Stack

- **Vue 3** - Progressive JavaScript framework
- **TypeScript** - Type-safe JavaScript
- **Vite** - Fast build tool and dev server
- **TailwindCSS** - Utility-first CSS framework
- **Vue Router** - Official router for Vue
- **Pinia** - State management
- **VeeValidate + Zod** - Form validation
- **TanStack Query** - Data fetching and caching
- **Kiota** - API client generation
