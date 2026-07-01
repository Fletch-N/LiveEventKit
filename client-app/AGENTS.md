# Agent Instructions

## Project Documentation

This project includes LLM-oriented reference documentation in:

`docs/llm/`

Before generating or modifying code, agents should check this folder for relevant documentation related to the library, framework, or tool they are working with.

Current reference files include:

* `docs/llm/mantine-llms.txt` — Mantine UI documentation for component usage, styling, hooks, theming, and form patterns.
* `docs/llm/react-llms-reference.md` — React documentation for components, Hooks, effects, React DOM APIs, Server Components, and modern React patterns.
* `docs/llm/react-router-llms-reference.md` — React Router documentation for routing modes, route configuration, loaders, actions, fetchers, navigation, pending UI, redirects, and route modules.

As more documentation files are added to `docs/llm/`, agents should treat them as project-level source material and use them when relevant.

## Documentation Usage Rules

When working on code:

1. Check `docs/llm/` for relevant reference files before making changes.
2. Prefer project-provided documentation over general memory of a library.
3. Follow the documented patterns for APIs, components, hooks, and framework conventions.
4. Do not invent APIs that are not supported by the referenced documentation.
5. If documentation conflicts with existing project code, preserve project conventions unless the task explicitly asks to update them.
6. If documentation is missing for a tool or library, use the existing codebase patterns first.

## React

When working on React code, use:

`docs/llm/react-llms-reference.md`

Follow modern React guidance:

* Use function components and Hooks.
* Follow the Rules of Hooks.
* Keep render logic pure.
* Prefer derived values during render instead of unnecessary `useEffect`.
* Use `useEffect` only for synchronization with external systems.
* Use TypeScript prop types.
* Use stable keys for lists.
* Prefer semantic HTML and accessible form patterns.
* Do not use removed React DOM APIs such as `ReactDOM.render`, `hydrate`, `findDOMNode`, or `unmountComponentAtNode`.
* Do not use Canary or Experimental React APIs unless explicitly requested.

## Mantine

When working on Mantine UI code, use:

`docs/llm/mantine-llms.txt`

Follow Mantine documentation for:

* Component props and composition.
* Styling patterns.
* Theme customization.
* Hooks.
* Forms.
* Layout components.
* Accessibility guidance.

Prefer Mantine components over custom UI primitives when the project already uses Mantine for that type of UI.

## Adding More Documentation

When new LLM reference files are added to `docs/llm/`, agents should automatically consider them part of the project guidance.

Recommended naming pattern:

* `docs/llm/<library-or-tool>-llms-reference.md`
* `docs/llm/<library-or-tool>-llms.txt`

Examples:

* `docs/llm/react-llms-reference.md`
* `docs/llm/mantine-llms.txt`
* `docs/llm/zustand-llms-reference.md`
* `docs/llm/axios-llms-reference.md`
* `docs/llm/vite-llms-reference.md`

## Agent Priority Order

When making implementation decisions, use this priority order:

1. Explicit user instructions.
2. Existing project code and conventions.
3. Relevant files in `docs/llm/`.
4. Official framework or library behavior.
5. General best practices.

When uncertain, make the smallest safe change that matches the existing project style.