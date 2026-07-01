# React LLM Reference

Generated from the React documentation reference pages at `https://react.dev/reference/react`.

Source version observed: React `19.2`.

This file is designed for use in LLM coding tools such as ChatGPT, Cursor, Windsurf, Claude, GitHub Copilot context files, or project-level documentation. It summarizes React's current public reference surface and adds practical guidance for generating modern React code.

---

## How to use this file with an LLM

When asking an LLM to write React code, include this file and state:

- Use modern React function components.
- Prefer Hooks over class components.
- Follow the Rules of Hooks.
- Keep components pure.
- Use effects only for synchronization with external systems.
- Use framework conventions when the project is using Next.js, Remix, React Router, Vite, or another React framework.
- Avoid deprecated React DOM APIs removed in React 19.
- Treat Canary and Experimental APIs as unavailable unless the project explicitly opts into those release channels.

---

## Core React mental model

React is a library for building user interfaces from components.

Modern React code should generally be written as function components:

```tsx
type GreetingProps = {
  name: string;
};

export function Greeting({ name }: GreetingProps) {
  return <h1>Hello, {name}</h1>;
}
```

React components should be pure during render:

- Same inputs should produce the same JSX.
- Do not mutate props, state, context, or module-level values during render.
- Do not perform network calls, subscriptions, timers, logging side effects, DOM writes, or storage writes during render.
- Event handlers are the preferred place for user-triggered side effects.
- Effects are for synchronizing with external systems after render.

---

## React package: `react`

Import Hooks and React APIs from `react`.

```tsx
import { useState, useEffect, useMemo } from 'react';
```

### Hooks overview

Hooks let function components use React features.

Rules:

- Call Hooks only at the top level of React function components or custom Hooks.
- Do not call Hooks inside loops, conditions, nested functions, callbacks, `try` / `catch`, or after early returns.
- Custom Hooks should be named with a `use` prefix.
- Components should be named with uppercase identifiers.
- Hooks must be called in a stable order on every render.

Correct:

```tsx
function UserCard({ userId }: { userId: string }) {
  const [expanded, setExpanded] = useState(false);

  if (!userId) {
    return null;
  }

  return (
    <button onClick={() => setExpanded((value) => !value)}>
      {expanded ? 'Hide' : 'Show'} details
    </button>
  );
}
```

Incorrect:

```tsx
function UserCard({ enabled }: { enabled: boolean }) {
  if (enabled) {
    const [count, setCount] = useState(0); // Do not call Hooks conditionally
  }

  return null;
}
```

---

## State Hooks

### `useState`

Use `useState` for local component state that can be replaced directly.

```tsx
const [name, setName] = useState('');
const [count, setCount] = useState(0);
```

Prefer updater functions when the new state depends on previous state:

```tsx
setCount((current) => current + 1);
```

Do not mutate state objects or arrays in place:

```tsx
setItems((items) => [...items, newItem]);
setUser((user) => ({ ...user, name: nextName }));
```

Good use cases:

- Form input values.
- UI toggles.
- Selected tab or active item.
- Local client-only interaction state.

Avoid:

- Duplicating data that can be derived from props or existing state.
- Storing values that do not affect rendering. Use `useRef` instead.
- Storing remote server cache manually when a framework or data library owns it.

### `useReducer`

Use `useReducer` when state transitions are more complex or action-based.

```tsx
type State = {
  count: number;
};

type Action =
  | { type: 'increment' }
  | { type: 'decrement' }
  | { type: 'reset' };

function reducer(state: State, action: Action): State {
  switch (action.type) {
    case 'increment':
      return { count: state.count + 1 };
    case 'decrement':
      return { count: state.count - 1 };
    case 'reset':
      return { count: 0 };
    default:
      return state;
  }
}

function Counter() {
  const [state, dispatch] = useReducer(reducer, { count: 0 });

  return (
    <button onClick={() => dispatch({ type: 'increment' })}>
      {state.count}
    </button>
  );
}
```

Good use cases:

- Multi-step state transitions.
- State machines.
- Complex form state.
- Related state values that change together.

---

## Context Hooks

### `useContext`

Use `useContext` to read and subscribe to a context value.

```tsx
const ThemeContext = createContext<'light' | 'dark'>('light');

function Button() {
  const theme = useContext(ThemeContext);
  return <button className={theme}>Save</button>;
}
```

Guidance:

- Use context for values needed by many descendants.
- Good candidates: theme, locale, auth session summary, feature flags, dependency objects.
- Avoid putting high-frequency changing state in broad context providers unless you understand the rerender behavior.
- Split contexts when unrelated values update independently.
- Avoid using context as a default global state store for everything.

---

## Ref Hooks

### `useRef`

Use `useRef` for mutable values that do not trigger rendering.

```tsx
const inputRef = useRef<HTMLInputElement | null>(null);

function focusInput() {
  inputRef.current?.focus();
}
```

Good use cases:

- DOM nodes.
- Timer IDs.
- Previous values.
- Imperative library instances.
- Values needed by event handlers but not by JSX output.

Do not read or write `ref.current` during render unless it is for predictable one-time initialization.

### `useImperativeHandle`

Use `useImperativeHandle` to customize the imperative API exposed through a ref.

```tsx
type InputHandle = {
  focus: () => void;
};

function TextInput({ ref }: { ref: React.Ref<InputHandle> }) {
  const inputRef = useRef<HTMLInputElement | null>(null);

  useImperativeHandle(ref, () => ({
    focus() {
      inputRef.current?.focus();
    },
  }));

  return <input ref={inputRef} />;
}
```

Guidance:

- Use rarely.
- Prefer declarative props over imperative handles.
- Useful for design system primitives and wrappers around imperative DOM behavior.

---

## Effect Hooks

### `useEffect`

Use `useEffect` to synchronize with external systems.

```tsx
useEffect(() => {
  const connection = createConnection(roomId);
  connection.connect();

  return () => {
    connection.disconnect();
  };
}, [roomId]);
```

Good use cases:

- Subscriptions.
- WebSocket connections.
- Browser APIs.
- Third-party widgets.
- Timers.
- Manual DOM event listeners.
- External stores when `useSyncExternalStore` is not appropriate.

Avoid effects for:

- Deriving values from props or state.
- Responding to user events that can be handled directly in event handlers.
- Resetting state that could be represented by a different key or derived structure.
- Fetching data in frameworks that provide route loaders, server components, or query libraries.

Prefer this:

```tsx
const fullName = `${firstName} ${lastName}`;
```

Over this:

```tsx
const [fullName, setFullName] = useState('');

useEffect(() => {
  setFullName(`${firstName} ${lastName}`);
}, [firstName, lastName]);
```

### `useLayoutEffect`

Use `useLayoutEffect` only when you must measure layout or synchronously make layout-related changes before the browser paints.

```tsx
useLayoutEffect(() => {
  const rect = ref.current?.getBoundingClientRect();
  setHeight(rect?.height ?? 0);
}, []);
```

Guidance:

- Prefer `useEffect` unless the visual result would flicker.
- Avoid on the server or in server-rendered code paths where possible.

### `useInsertionEffect`

Use `useInsertionEffect` only for CSS-in-JS libraries that must insert styles before DOM mutations are committed.

Application code almost never needs this Hook.

### `useEffectEvent`

Use `useEffectEvent` to separate non-reactive event-like logic from an Effect.

Useful when an Effect should depend on one value but still read the latest version of another value without making the Effect reactive to it.

---

## Performance Hooks

### `useMemo`

Use `useMemo` to cache an expensive calculation between renders.

```tsx
const visibleTodos = useMemo(() => {
  return todos.filter((todo) => todo.tab === activeTab);
}, [todos, activeTab]);
```

Guidance:

- Do not use `useMemo` everywhere by default.
- Use it for expensive calculations, stable object identities passed to memoized children, or values used as dependencies by other Hooks.
- Code must still be correct without `useMemo`; it is a performance optimization.

### `useCallback`

Use `useCallback` to cache a function definition.

```tsx
const handleSave = useCallback(() => {
  saveDraft(draftId);
}, [draftId]);
```

Good use cases:

- Passing callbacks to memoized child components.
- Stable callback dependencies for Effects or custom Hooks.
- Avoiding unnecessary work in specific measured performance paths.

Avoid using `useCallback` as a default habit for every event handler.

### `useTransition`

Use `useTransition` to mark state updates as non-blocking.

```tsx
const [isPending, startTransition] = useTransition();

function onSearch(nextQuery: string) {
  setQuery(nextQuery);

  startTransition(() => {
    setDeferredQuery(nextQuery);
  });
}
```

Good use cases:

- Updating expensive UI without blocking urgent input.
- Switching tabs or filters where old UI can stay visible while new UI renders.
- Non-urgent rendering work.

### `useDeferredValue`

Use `useDeferredValue` to defer a non-urgent value.

```tsx
const deferredQuery = useDeferredValue(query);
```

Good use cases:

- Keeping typing responsive while a large list updates.
- Rendering stale results while new results are prepared.

---

## Other React Hooks

### `useDebugValue`

Use `useDebugValue` inside custom Hooks to customize labels shown in React DevTools.

```tsx
function useOnlineStatus() {
  const isOnline = useSyncExternalStore(subscribe, getSnapshot);
  useDebugValue(isOnline ? 'Online' : 'Offline');
  return isOnline;
}
```

### `useId`

Use `useId` to generate stable unique IDs for accessibility attributes.

```tsx
const id = useId();

return (
  <>
    <label htmlFor={id}>Email</label>
    <input id={id} type="email" />
  </>
);
```

Do not use `useId` for list keys. Use stable IDs from your data.

### `useSyncExternalStore`

Use `useSyncExternalStore` to subscribe to external stores safely.

```tsx
const snapshot = useSyncExternalStore(
  store.subscribe,
  store.getSnapshot,
  store.getServerSnapshot
);
```

Good use cases:

- Browser APIs with subscription models.
- Third-party state stores.
- Custom external stores.

### `useActionState`

Use `useActionState` to manage state from actions, especially form actions.

```tsx
const [state, formAction, isPending] = useActionState(submitForm, initialState);
```

Good use cases:

- Form submission state.
- Server/client action result state.
- Pending state tied to an action.

### `useOptimistic`

Use `useOptimistic` for optimistic UI updates while an async action is in progress.

```tsx
const [optimisticMessages, addOptimisticMessage] = useOptimistic(
  messages,
  (currentMessages, newMessage: Message) => [...currentMessages, newMessage]
);
```

Good use cases:

- Chat messages.
- Likes/reactions.
- Inline edits.
- UI where the expected result can be shown before confirmation.

---

## React components

Import built-in React components from `react` where needed.

### `<Fragment>` / `<>...</>`

Use fragments to group multiple JSX nodes without adding extra DOM.

```tsx
return (
  <>
    <h1>Title</h1>
    <p>Body</p>
  </>
);
```

Use explicit `<Fragment key={...}>` when rendering keyed fragment lists.

### `<StrictMode>`

Use `<StrictMode>` to enable development-only checks.

```tsx
createRoot(rootElement).render(
  <StrictMode>
    <App />
  </StrictMode>
);
```

Guidance:

- Keep enabled in development.
- It may intentionally double-invoke some logic to surface unsafe side effects.
- Do not write code that depends on effects running exactly once in development.

### `<Suspense>`

Use `<Suspense>` to show fallback UI while children are loading or suspended.

```tsx
<Suspense fallback={<Spinner />}>
  <Profile />
</Suspense>
```

Good use cases:

- Code splitting with `lazy`.
- Framework-supported data loading.
- Server Components and streaming UI.

### `<Profiler>`

Use `<Profiler>` to measure rendering performance programmatically.

```tsx
<Profiler id="Dashboard" onRender={handleRender}>
  <Dashboard />
</Profiler>
```

This is usually for diagnostics and tooling, not normal app structure.

### `<Activity>`

Use `<Activity>` to hide and restore UI and internal state of children.

Because this is newer, check project React version and release channel before relying on it.

### `<ViewTransition>`

`<ViewTransition>` is Canary-only. Do not use unless the project explicitly uses the React Canary release channel.

---

## React APIs

### `createContext`

Use `createContext` to define a context.

```tsx
const AuthContext = createContext<AuthContextValue | null>(null);
```

Prefer wrapping access in a custom Hook when a provider is required:

```tsx
function useAuth() {
  const value = useContext(AuthContext);

  if (!value) {
    throw new Error('useAuth must be used inside AuthProvider');
  }

  return value;
}
```

### `lazy`

Use `lazy` for code-splitting components.

```tsx
const SettingsPage = lazy(() => import('./SettingsPage'));

<Suspense fallback={<Spinner />}>
  <SettingsPage />
</Suspense>
```

### `memo`

Use `memo` to skip rerenders when props are unchanged.

```tsx
const Row = memo(function Row({ item }: { item: Item }) {
  return <li>{item.name}</li>;
});
```

Guidance:

- Use after identifying unnecessary rerenders.
- Works best with stable props.
- Pair with `useMemo` / `useCallback` only when identity stability matters.

### `startTransition`

Use `startTransition` outside components or when the Hook form is not available to mark updates as non-urgent.

```tsx
startTransition(() => {
  setPage(nextPage);
});
```

### `act`

Use `act` in tests to ensure React updates are flushed before assertions.

Testing libraries often wrap common utilities in `act` for you.

### `use`

Use `use` to read a resource such as a Promise or context.

```tsx
function Message({ messagePromise }: { messagePromise: Promise<string> }) {
  const message = use(messagePromise);
  return <p>{message}</p>;
}
```

Guidance:

- Unlike Hooks, `use` has special behavior and can be called in conditions and loops.
- It may suspend while waiting for a Promise.
- Use within framework-supported patterns for data loading when possible.

### `cache`

Use `cache` to memoize async work across render attempts in supported server-oriented React environments.

Primarily useful for frameworks and server rendering patterns.

### `cacheSignal`

Use `cacheSignal` with cached async work to detect when React abandons or invalidates work.

Primarily useful in advanced server rendering and framework-level code.

### `captureOwnerStack`

Use for debugging owner stacks. Not typical application code.

### Canary / Experimental APIs

Do not use these unless explicitly requested and the project opts into the corresponding React release channel:

- `addTransitionType` - Canary.
- `experimental_taintObjectReference` - Experimental.
- `experimental_taintUniqueValue` - Experimental.

---

## React DOM package: `react-dom`

The `react-dom` package contains APIs for browser DOM environments. These are not for React Native.

```tsx
import { createPortal, flushSync } from 'react-dom';
```

### `createPortal`

Use `createPortal` to render children into a different DOM node.

```tsx
return createPortal(
  <ModalContent />,
  document.getElementById('modal-root')!
);
```

Good use cases:

- Modals.
- Tooltips.
- Toasts.
- Overlays.

### `flushSync`

Use `flushSync` to force React to flush an update synchronously.

```tsx
flushSync(() => {
  setOpen(true);
});
```

Guidance:

- Use sparingly.
- Can hurt performance.
- Usually only needed for integration with browser APIs or imperative code that needs the DOM updated immediately.

### Resource preloading APIs

React DOM provides resource hint APIs:

- `prefetchDNS`
- `preconnect`
- `preload`
- `preloadModule`
- `preinit`
- `preinitModule`

Use these to tell the browser about resources before they are discovered naturally.

```tsx
preconnect('https://example.com');
preload('/font.woff2', { as: 'font' });
preinit('/script.js', { as: 'script' });
```

Guidance:

- React frameworks often handle this.
- Use only when you know the resource will likely be needed.
- Prefer framework-level metadata/resource APIs when available.

---

## React DOM Hooks

### `useFormStatus`

Import from `react-dom`.

```tsx
import { useFormStatus } from 'react-dom';

function SubmitButton() {
  const { pending } = useFormStatus();

  return (
    <button type="submit" disabled={pending}>
      {pending ? 'Saving...' : 'Save'}
    </button>
  );
}
```

Use inside a component rendered within a form to read the form submission status.

---

## React DOM components

React supports browser built-in HTML and SVG elements.

### Common components

Most HTML/SVG components support common props and events:

```tsx
<div
  id="panel"
  className="panel"
  onClick={handleClick}
  aria-expanded={expanded}
>
  Content
</div>
```

React-specific props include:

- `ref`
- `key`
- `children`
- `dangerouslySetInnerHTML`

Use `dangerouslySetInnerHTML` only for trusted or sanitized HTML.

### Form components

React has special behavior for form elements:

- `<form>`
- `<input>`
- `<select>`
- `<textarea>`
- `<option>`
- `<progress>`

Controlled input:

```tsx
const [email, setEmail] = useState('');

<input
  value={email}
  onChange={(event) => setEmail(event.target.value)}
/>
```

Uncontrolled input:

```tsx
<input defaultValue="initial value" />
```

Guidance:

- Use controlled inputs when React state should be the source of truth.
- Use uncontrolled inputs for simpler forms or framework form actions.
- Do not switch an input between controlled and uncontrolled over its lifetime.
- Use `defaultValue` / `defaultChecked` for uncontrolled initial values.
- Use `value` / `checked` with `onChange` for controlled values.

### Resource and metadata components

React has special handling for:

- `<link>`
- `<meta>`
- `<script>`
- `<style>`
- `<title>`

These can annotate the document or load external resources. Prefer framework metadata APIs when using frameworks that manage document head behavior.

---

## React DOM client APIs: `react-dom/client`

### `createRoot`

Use `createRoot` to render a React app into a browser DOM node.

```tsx
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { App } from './App';

const rootElement = document.getElementById('root');

if (!rootElement) {
  throw new Error('Root element not found');
}

createRoot(rootElement).render(
  <StrictMode>
    <App />
  </StrictMode>
);
```

### `hydrateRoot`

Use `hydrateRoot` when the DOM node already contains server-rendered React HTML.

```tsx
hydrateRoot(document.getElementById('root')!, <App />);
```

Use for SSR hydration, usually through a framework.

---

## React DOM server APIs: `react-dom/server`

Server rendering APIs are typically used by frameworks. Most components should not import them directly.

### Web Streams

Use in environments that support Web Streams:

- `renderToReadableStream`
- `resume`

### Node.js Streams

Use in Node.js stream environments:

- `renderToPipeableStream`
- `resumeToPipeableStream`

### Non-streaming legacy server APIs

Available but limited compared with streaming APIs:

- `renderToString`
- `renderToStaticMarkup`

Guidance:

- Prefer streaming SSR when building your own server integration.
- Prefer a framework when possible.
- Avoid introducing server rendering manually into a client-only app without a clear architecture.

---

## React DOM static APIs: `react-dom/static`

Static APIs generate static HTML for React components.

### Web Streams

- `prerender`
- `resumeAndPrerender` - Experimental.

### Node.js Streams

- `prerenderToNodeStream`
- `resumeAndPrerenderToNodeStream` - Experimental.

Guidance:

- Usually framework-owned.
- Use for static generation and prerendering infrastructure.
- Do not use Experimental variants unless the project opts into them.

---

## React Server Components

React Server Components render ahead of time in an environment separate from the client app or SSR server.

Key points:

- Server Components can run at build time or per request.
- They can read server-side resources without shipping that code to the browser.
- They cannot use client-only Hooks like `useState` or `useEffect`.
- Client interactivity requires Client Components.
- A Server Component can pass serializable props to Client Components.
- Frameworks own most Server Component integration details.

Use Server Components for:

- Reading from a database or filesystem.
- Loading static content.
- Reducing client bundle size.
- Keeping server-only dependencies off the client.

Use Client Components for:

- State.
- Effects.
- Browser APIs.
- Event handlers.
- Interactive UI.

---

## React Server Functions

Server Functions allow Client Components to call async functions executed on the server when supported by the framework.

Guidance:

- Use framework conventions.
- Treat arguments and return values as serialized boundary data.
- Validate authorization and input on the server.
- Do not trust client-provided data.

---

## Directives

### `'use client'`

Use at the top of a module to mark it as a Client Component entry point in React Server Component frameworks.

```tsx
'use client';

import { useState } from 'react';

export function Counter() {
  const [count, setCount] = useState(0);
  return <button onClick={() => setCount((value) => value + 1)}>{count}</button>;
}
```

Use when the file needs:

- State.
- Effects.
- Browser APIs.
- Event handlers.
- Client-only libraries.

### `'use server'`

Use to mark server functions in supported frameworks.

```tsx
'use server';

export async function saveUser(formData: FormData) {
  // Validate and write on the server
}
```

Guidance:

- Keep server-only code server-only.
- Do not import server modules into client components unless the framework supports the boundary.
- Validate permissions and inputs.

### React Compiler directives

For React Compiler-enabled projects:

- `"use memo"` requests compilation/memoization for a function.
- `"use no memo"` opts a function out.

Only use these when React Compiler is configured and the team has a compiler strategy.

---

## React Compiler

React Compiler is a build-time optimization tool that automatically memoizes components and values.

Configuration areas include:

- `compilationMode`
- `gating`
- `logger`
- `panicThreshold`
- `target`

Guidance:

- Do not assume React Compiler is enabled.
- If enabled, keep components pure and follow the Rules of React.
- Avoid manual memoization everywhere just because compiler exists.
- Use project config as source of truth.

---

## eslint-plugin-react-hooks

Use this plugin to enforce React rules and catch common mistakes.

Important rules include:

- `rules-of-hooks` - validates Hook call locations.
- `exhaustive-deps` - validates dependency arrays for Hooks.
- `component-hook-factories`
- `config`
- `error-boundaries`
- `gating`
- `globals`
- `immutability`
- `incompatible-library`
- `preserve-manual-memoization`
- `purity`
- `refs`
- `set-state-in-effect`
- `set-state-in-render`
- `static-components`
- `unsupported-syntax`
- `use-memo`

Guidance for LLMs:

- Do not silence Hook lint warnings by removing dependencies.
- If an Effect dependency causes repeated execution, restructure the code.
- Move event-specific logic into event handlers.
- Move non-reactive Effect logic into `useEffectEvent` when available and appropriate.
- Use functional state updates to remove unnecessary dependencies when correct.
- Extract custom Hooks for repeated logic.

---

## Rules of React

### Components and Hooks must be pure

Do:

```tsx
function Price({ cents }: { cents: number }) {
  return <span>${(cents / 100).toFixed(2)}</span>;
}
```

Do not:

```tsx
function Price({ cents }: { cents: number }) {
  localStorage.setItem('last-price', String(cents)); // side effect during render
  return <span>{cents}</span>;
}
```

### React calls components and Hooks

Do not call components as ordinary functions:

```tsx
// Incorrect
const result = UserCard({ user });
```

Use JSX:

```tsx
<UserCard user={user} />
```

### Rules of Hooks

Do not conditionally call Hooks. Put conditions inside Hooks or split components.

```tsx
useEffect(() => {
  if (!enabled) {
    return;
  }

  const subscription = subscribe();
  return () => subscription.unsubscribe();
}, [enabled]);
```

---

## Legacy React APIs

These APIs still exist but are not recommended for new code:

- `Children`
- `cloneElement`
- `Component`
- `createElement`
- `createRef`
- `forwardRef`
- `isValidElement`
- `PureComponent`

Guidance:

- Prefer function components over class components.
- Prefer JSX over direct `createElement` calls.
- Prefer composition/render props/custom Hooks over `cloneElement` when possible.
- Use modern ref-as-prop behavior when supported by the project and React version.
- Keep legacy APIs only when maintaining older code or library compatibility requires them.

---

## Removed React DOM APIs in React 19

Avoid these removed APIs:

- `findDOMNode`
- `hydrate`
- `render`
- `unmountComponentAtNode`
- `renderToNodeStream`
- `renderToStaticNodeStream`

Use instead:

- `createRoot` instead of `render`.
- `hydrateRoot` instead of `hydrate`.
- `root.unmount()` instead of `unmountComponentAtNode`.
- Modern `react-dom/server` streaming APIs instead of removed stream APIs.

---

## TypeScript patterns

Prefer explicit prop types.

```tsx
type ButtonProps = {
  children: React.ReactNode;
  variant?: 'primary' | 'secondary';
  onClick?: () => void;
};

export function Button({ children, variant = 'primary', onClick }: ButtonProps) {
  return (
    <button data-variant={variant} onClick={onClick}>
      {children}
    </button>
  );
}
```

Prefer `React.ReactNode` for renderable children.

```tsx
type CardProps = {
  title: string;
  children: React.ReactNode;
};
```

Prefer DOM event types when needed:

```tsx
function handleChange(event: React.ChangeEvent<HTMLInputElement>) {
  setValue(event.target.value);
}
```

Prefer precise refs:

```tsx
const inputRef = useRef<HTMLInputElement | null>(null);
```

Avoid `React.FC` unless the project convention uses it. Plain functions are usually clearer and avoid implicit children behavior.

---

## Common implementation guidance for LLMs

### Component structure

Prefer:

```tsx
type UserListProps = {
  users: User[];
  onSelectUser: (userId: string) => void;
};

export function UserList({ users, onSelectUser }: UserListProps) {
  if (users.length === 0) {
    return <EmptyState message="No users found." />;
  }

  return (
    <ul>
      {users.map((user) => (
        <li key={user.id}>
          <button onClick={() => onSelectUser(user.id)}>{user.name}</button>
        </li>
      ))}
    </ul>
  );
}
```

Guidance:

- Use early returns for empty/error/loading states.
- Use stable keys from data, not array indexes, when list order can change.
- Keep components small enough to understand.
- Extract custom Hooks for reusable stateful behavior.
- Extract pure helper functions for calculations.

### Data fetching

Do not default to `useEffect` for data fetching if the project uses a framework with loaders, Server Components, server actions, or a query library.

In plain client React, an Effect can fetch data, but include cancellation/ignore handling:

```tsx
useEffect(() => {
  let ignore = false;

  async function loadUser() {
    setStatus('loading');

    try {
      const user = await getUser(userId);

      if (!ignore) {
        setUser(user);
        setStatus('success');
      }
    } catch (error) {
      if (!ignore) {
        setStatus('error');
      }
    }
  }

  loadUser();

  return () => {
    ignore = true;
  };
}, [userId]);
```

Prefer a dedicated data library for caching, invalidation, retries, and request deduplication.

### Derived state

Prefer deriving during render:

```tsx
const completedCount = todos.filter((todo) => todo.completed).length;
```

Avoid mirroring derived state with Effects:

```tsx
useEffect(() => {
  setCompletedCount(todos.filter((todo) => todo.completed).length);
}, [todos]);
```

### Event handlers

Use event handlers for user-triggered work:

```tsx
async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
  event.preventDefault();
  await saveForm();
}
```

### Conditional rendering

Use clear conditional rendering:

```tsx
if (isLoading) return <Spinner />;
if (error) return <ErrorMessage error={error} />;

return <Content />;
```

### Lists

Use stable keys:

```tsx
{items.map((item) => (
  <ItemRow key={item.id} item={item} />
))}
```

Avoid indexes as keys when items can be reordered, inserted, or removed.

### Accessibility

Default guidance:

- Use semantic HTML first.
- Use labels for form controls.
- Use `button` for actions and `a` for navigation.
- Keep keyboard interaction intact.
- Use ARIA only when semantic HTML is insufficient.

Example:

```tsx
<label htmlFor={emailId}>Email</label>
<input id={emailId} type="email" autoComplete="email" />
```

---

## Project setup notes

For a Vite React TypeScript app:

```tsx
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { App } from './App';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <App />
  </StrictMode>
);
```

Typical app-level providers:

```tsx
function Root() {
  return (
    <StrictMode>
      <AppProviders>
        <App />
      </AppProviders>
    </StrictMode>
  );
}
```

Use framework-specific entry points when not using Vite.

---

## Quick API index

### `react` Hooks

- `useActionState`
- `useCallback`
- `useContext`
- `useDebugValue`
- `useDeferredValue`
- `useEffect`
- `useEffectEvent`
- `useId`
- `useImperativeHandle`
- `useInsertionEffect`
- `useLayoutEffect`
- `useMemo`
- `useOptimistic`
- `useReducer`
- `useRef`
- `useState`
- `useSyncExternalStore`
- `useTransition`

### `react` Components

- `<Fragment>` / `<>...</>`
- `<Profiler>`
- `<StrictMode>`
- `<Suspense>`
- `<Activity>`
- `<ViewTransition>` - Canary only.

### `react` APIs

- `act`
- `addTransitionType` - Canary only.
- `cache`
- `cacheSignal`
- `captureOwnerStack`
- `createContext`
- `lazy`
- `memo`
- `startTransition`
- `use`
- `experimental_taintObjectReference` - Experimental only.
- `experimental_taintUniqueValue` - Experimental only.

### `react-dom` Hooks

- `useFormStatus`

### `react-dom` APIs

- `createPortal`
- `flushSync`
- `preconnect`
- `prefetchDNS`
- `preinit`
- `preinitModule`
- `preload`
- `preloadModule`

### `react-dom/client`

- `createRoot`
- `hydrateRoot`

### `react-dom/server`

- `renderToPipeableStream`
- `renderToReadableStream`
- `renderToStaticMarkup`
- `renderToString`
- `resume`
- `resumeToPipeableStream`

### `react-dom/static`

- `prerender`
- `prerenderToNodeStream`
- `resumeAndPrerender` - Experimental only.
- `resumeAndPrerenderToNodeStream` - Experimental only.

### Rules and tooling

- Components and Hooks must be pure.
- React calls Components and Hooks.
- Rules of Hooks.
- eslint-plugin-react-hooks lints.
- React Compiler configuration and directives.

---

## Official documentation links

Primary reference:

- `https://react.dev/reference/react`

React package:

- `https://react.dev/reference/react/hooks`
- `https://react.dev/reference/react/components`
- `https://react.dev/reference/react/apis`
- `https://react.dev/reference/rsc/directives`

React DOM:

- `https://react.dev/reference/react-dom`
- `https://react.dev/reference/react-dom/hooks`
- `https://react.dev/reference/react-dom/components`
- `https://react.dev/reference/react-dom/client`
- `https://react.dev/reference/react-dom/server`
- `https://react.dev/reference/react-dom/static`

Rules and tooling:

- `https://react.dev/reference/rules`
- `https://react.dev/reference/eslint-plugin-react-hooks`
- `https://react.dev/reference/react-compiler`

React Server Components:

- `https://react.dev/reference/rsc/server-components`
- `https://react.dev/reference/rsc/server-functions`
- `https://react.dev/reference/rsc/directives`

---

## Prompt snippet for coding agents

Use this when asking an LLM to modify React code:

```text
Use the React LLM Reference file. Write modern React 19-compatible code with function components and Hooks. Follow the Rules of Hooks, keep render pure, avoid unnecessary Effects, and prefer derived values during render. Use TypeScript types for props. Do not use React 19 removed ReactDOM APIs. Do not use Canary or Experimental APIs unless explicitly requested. When the project framework provides data loading, routing, metadata, or server-action conventions, follow the framework instead of inventing plain client React patterns.
```
