# Live Event Kit Client App

## Overview

LiveEventKit is a Vite + React + TypeScript frontend for building and managing live event platform workflows, with Mantine providing the core UI system.

## LLM / Agent Reference

This project includes LLM-oriented documentation for coding agents in `docs/llm/`.

Agents should read `AGENTS.md` first, then check `docs/llm/` for any reference files relevant to the code they are generating or modifying. Agents should prefer these project-provided references over general memory of a library.

Current reference files include:

- `docs/llm/mantine-llms.txt`: Mantine UI reference documentation.
- `docs/llm/react-llms-reference.md`: React reference documentation.

When working with React, Mantine, or any other documented tool in this project, agents should follow the corresponding reference file before introducing new APIs, patterns, components, or conventions.

## Tech Stack

- Vite
- React
- TypeScript
- Mantine
- ESLint
- PostCSS

## Getting Started

Install dependencies:

```bash
npm install
```

Start the local development server:

```bash
npm run dev
```

## Development

Application code lives in `src/`. The app is organized around a thin application layer, route-level pages, domain features, shared components, and shared utilities.

Update `src/app/Theme.ts` for global Mantine theme configuration, including client colors, typography, spacing, radius, shadows, and component defaults.

## Project Structure

```txt
src/
  app/: app wiring only. Providers, router, Mantine theme, root shell.
  pages/: route-level screens that compose features together.
  features/: business/domain areas such as events, attendees, tickets, check-in, venues, and auth.
  components/ui/: reusable generic UI pieces with no event-platform knowledge.
  components/layout/: shell, nav, header, sidebar, and page frame components.
  lib/: shared non-React utilities like API client, env parsing, and date helpers.
  hooks/: truly generic hooks. Feature-specific hooks stay inside their feature.
```

## Scripts

- `npm run dev`: start the Vite development server.
- `npm run build`: type-check and build the production bundle.
- `npm run lint`: run ESLint.
- `npm run preview`: preview the production build locally.
