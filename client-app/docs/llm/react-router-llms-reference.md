# React Router LLM Reference

Generated from the React Router documentation at `https://reactrouter.com/home` and related reference pages.

This file is designed for LLM coding agents working in React + TypeScript projects. It summarizes current React Router concepts, APIs, and patterns in a form that is easier for coding tools to use than raw documentation.

---

## How to use this file with an LLM

When asking an LLM to write or modify routing code, include this file and state:

- Use the routing mode already present in this project.
- Do not mix Declarative Mode, Data Mode, and Framework Mode patterns unless the task explicitly requires migration.
- Prefer existing project conventions over introducing a new routing architecture.
- Use `react-router` imports unless the existing project clearly uses a different import convention.
- Use TypeScript types for route params, loader data, action data, and navigation-related props where practical.
- Prefer route loaders/actions/fetchers in Data or Framework Mode instead of ad hoc `useEffect` data fetching.
- Avoid unstable, UNSAFE, or internal APIs unless the project already uses them and the task requires it.

---

## React Router mental model

React Router is a routing library and framework toolkit for React. It maps URLs to UI, supports navigation, nested layouts, route params, pending states, data loading, mutations, redirects, error boundaries, and framework-level rendering strategies.

React Router can be used in three primary modes:

1. Declarative Mode
2. Data Mode
3. Framework Mode

These modes are additive. Declarative Mode is the simplest. Data Mode adds loaders, actions, fetchers, pending UI, and route object configuration outside React render. Framework Mode adds a Vite plugin, route modules, generated route types, automatic code splitting, SSR/static rendering strategies, and stronger conventions.

Important rule for agents:

> First identify the mode the project is already using, then stay within that mode.

---

## Mode selection

### Declarative Mode

Use Declarative Mode for simple client-side routing with JSX route definitions.

Typical APIs:

- `<BrowserRouter>`
- `<Routes>`
- `<Route>`
- `<Link>`
- `<NavLink>`
- `<Navigate>`
- `<Outlet>`
- `useNavigate`
- `useLocation`
- `useParams`
- `useSearchParams`

Example:

```tsx
import { BrowserRouter, Link, Route, Routes } from 'react-router';

export function App() {
  return (
    <BrowserRouter>
      <nav>
        <Link to="/">Home</Link>
        <Link to="/about">About</Link>
      </nav>

      <Routes>
        <Route index element={<Home />} />
        <Route path="about" element={<About />} />
      </Routes>
    </BrowserRouter>
  );
}
```

Use Declarative Mode when:

- The app is a simple SPA.
- Routing is component-only.
- Data fetching is handled elsewhere.
- The project already uses `<BrowserRouter>` and `<Routes>`.

Avoid introducing Data Router APIs into a Declarative app unless the user asks to migrate.

---

### Data Mode

Use Data Mode when routes are configured as route objects outside React render and passed to `<RouterProvider>`.

Typical APIs:

- `createBrowserRouter`
- `RouterProvider`
- route objects
- `loader`
- `action`
- `Component`
- `ErrorBoundary`
- `HydrateFallback`
- `<Form>`
- `useLoaderData`
- `useActionData`
- `useNavigation`
- `useFetcher`
- `redirect`

Example:

```tsx
import { createBrowserRouter, RouterProvider } from 'react-router';

const router = createBrowserRouter([
  {
    path: '/',
    Component: RootLayout,
    children: [
      { index: true, Component: HomePage },
      { path: 'about', Component: AboutPage },
      {
        path: 'projects/:projectId',
        loader: projectLoader,
        Component: ProjectPage,
      },
    ],
  },
]);

export function AppRouter() {
  return <RouterProvider router={router} />;
}
```

Use Data Mode when:

- The project uses `createBrowserRouter`.
- Data should load before route render.
- Form submissions should trigger route actions.
- Pending navigation states matter.
- Route-level error boundaries are useful.
- You want React Router data APIs without adopting full Framework Mode.

Data Routers should be created once outside the React tree, not inside a component or React state.

---

### Framework Mode

Use Framework Mode when the project uses React Router as a full framework, usually with `@react-router/dev`, `app/routes.ts`, route modules, and generated `+types`.

Typical files and APIs:

- `app/routes.ts`
- `@react-router/dev/routes`
- `route`
- `index`
- `layout`
- `prefix`
- route modules
- `loader`
- `clientLoader`
- `action`
- `clientAction`
- `headers`
- `links`
- `meta`
- generated `Route` types from `./+types/...`

Example route config:

```tsx
import {
  type RouteConfig,
  index,
  layout,
  prefix,
  route,
} from '@react-router/dev/routes';

export default [
  index('./home.tsx'),
  route('about', './about.tsx'),
  layout('./auth/layout.tsx', [
    route('login', './auth/login.tsx'),
    route('register', './auth/register.tsx'),
  ]),
  ...prefix('projects', [
    index('./projects/index.tsx'),
    route(':projectId', './projects/project.tsx'),
  ]),
] satisfies RouteConfig;
```

Example route module:

```tsx
import type { Route } from './+types/project';

export async function loader({ params }: Route.LoaderArgs) {
  const project = await getProject(params.projectId);
  return { project };
}

export default function Project({ loaderData, params }: Route.ComponentProps) {
  return (
    <section>
      <h1>{loaderData.project.name}</h1>
      <p>Project ID: {params.projectId}</p>
    </section>
  );
}
```

Use Framework Mode when:

- The project has `app/routes.ts`.
- Route modules import `Route` from `./+types/...`.
- The app uses React Router's framework templates or Vite plugin.
- You need type-safe route module props.
- You need SSR, SSG, SPA rendering strategies, or automatic code splitting.

Do not convert a normal Vite SPA to Framework Mode unless the user explicitly asks.

---

## Installation and package notes

Current docs use:

```shell
npm i react-router
```

React Router Framework Mode projects are commonly created with:

```shell
npx create-react-router@latest my-react-router-app
```

For a Vite React TypeScript app that is not using Framework Mode, Data Mode is commonly installed with `react-router` and rendered with `createBrowserRouter` plus `RouterProvider`.

Check the existing project before changing package names or entry points.

---

## Routing concepts

### Route paths

Route paths define URL patterns.

```tsx
{ path: '/', Component: Root }
{ path: 'about', Component: About }
{ path: 'projects/:projectId', Component: Project }
```

Guidance:

- Child paths are relative to parent routes.
- Do not start child paths with `/` unless you intentionally want an absolute path.
- Dynamic segments begin with `:`.
- Splat routes use `*`.

### Nested routes

Nested routes are defined with `children`.

```tsx
const router = createBrowserRouter([
  {
    path: '/dashboard',
    Component: DashboardLayout,
    children: [
      { index: true, Component: DashboardHome },
      { path: 'settings', Component: DashboardSettings },
    ],
  },
]);
```

Parent routes render child routes with `<Outlet />`.

```tsx
import { Outlet } from 'react-router';

export function DashboardLayout() {
  return (
    <main>
      <DashboardNav />
      <Outlet />
    </main>
  );
}
```

### Layout routes

A layout route provides UI around child routes.

Data Mode:

```tsx
{
  Component: MarketingLayout,
  children: [
    { index: true, Component: LandingPage },
    { path: 'pricing', Component: PricingPage },
  ],
}
```

Framework Mode:

```tsx
layout('./marketing/layout.tsx', [
  index('./marketing/home.tsx'),
  route('pricing', './marketing/pricing.tsx'),
]);
```

### Index routes

Index routes render at the parent route's exact URL.

Data Mode:

```tsx
{ index: true, Component: HomePage }
```

Declarative Mode:

```tsx
<Route index element={<HomePage />} />
```

Framework Mode:

```tsx
index('./home.tsx')
```

Do not give index routes a `path`.

### Dynamic segments

Use `:paramName` for route params.

```tsx
{ path: 'users/:userId', Component: UserPage }
```

Read params:

```tsx
import { useParams } from 'react-router';

function UserPage() {
  const { userId } = useParams();

  if (!userId) {
    throw new Error('Missing userId route param');
  }

  return <UserDetails userId={userId} />;
}
```

In Framework Mode, prefer generated route props:

```tsx
import type { Route } from './+types/user';

export default function UserPage({ params }: Route.ComponentProps) {
  return <UserDetails userId={params.userId} />;
}
```

### Optional segments

Optional segments may be used where supported by route patterns. Use carefully because they can make matching behavior harder to reason about.

Prefer explicit sibling routes when clarity matters.

### Splats

Use splats for catch-all segments.

```tsx
{ path: 'files/*', Component: FileBrowser }
```

Read the splat from params using `"*"`.

```tsx
const params = useParams();
const path = params['*'];
```

Good use cases:

- File browsers.
- CMS-style catch-all pages.
- Not found routes.

---

## Navigation

### `<Link>`

Use `<Link>` for client-side navigation.

```tsx
import { Link } from 'react-router';

<Link to="/projects">Projects</Link>
```

Use links for navigation instead of buttons with `navigate()` when the user is going to another URL.

### `<NavLink>`

Use `<NavLink>` when active or pending state should affect styling.

```tsx
import { NavLink } from 'react-router';

<NavLink
  to="/projects"
  className={({ isActive, isPending }) =>
    isActive ? 'active' : isPending ? 'pending' : undefined
  }
>
  Projects
</NavLink>
```

### `useNavigate`

Use `useNavigate` for imperative navigation, usually after an event or side effect.

```tsx
import { useNavigate } from 'react-router';

function SaveButton() {
  const navigate = useNavigate();

  async function handleSave() {
    await save();
    navigate('/projects');
  }

  return <button onClick={handleSave}>Save</button>;
}
```

Guidance:

- Prefer `<Link>` or `<Form>` when possible.
- Use `navigate(-1)` for browser-history back behavior.
- Use `replace: true` for redirects that should not leave a back-stack entry.

```tsx
navigate('/login', { replace: true });
```

### `<Navigate>`

Use `<Navigate>` for render-time redirects in Declarative Mode.

```tsx
import { Navigate } from 'react-router';

function ProtectedPage({ isAuthenticated }: { isAuthenticated: boolean }) {
  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  return <Dashboard />;
}
```

In Data or Framework Mode, prefer `redirect()` from loaders/actions for route-level redirects.

### `redirect`

Use `redirect` inside loaders and actions.

```tsx
import { redirect } from 'react-router';

export async function loader({ request }: Route.LoaderArgs) {
  const user = await getUser(request);

  if (!user) {
    return redirect('/login');
  }

  return { user };
}
```

Good use cases:

- Auth gates.
- Redirect after create/update/delete.
- Canonical URL redirects.

---

## Location, params, and search params

### `useLocation`

Use `useLocation` to read the current location object.

```tsx
const location = useLocation();
```

Good use cases:

- Analytics.
- Preserving return URLs.
- Reading `pathname`, `search`, `hash`, or `state`.

### `useParams`

Use `useParams` to read dynamic route params in Declarative or Data Mode.

```tsx
const { projectId } = useParams();
```

In TypeScript, validate required params before use:

```tsx
if (!projectId) {
  throw new Error('Missing projectId');
}
```

In Framework Mode, prefer `Route.ComponentProps` or `Route.LoaderArgs`.

### `useSearchParams`

Use `useSearchParams` to read and write URL query params.

```tsx
const [searchParams, setSearchParams] = useSearchParams();

const query = searchParams.get('q') ?? '';

function updateQuery(nextQuery: string) {
  setSearchParams((current) => {
    current.set('q', nextQuery);
    return current;
  });
}
```

Be careful mutating the existing `URLSearchParams` object. Prefer returning a new object when clarity matters:

```tsx
setSearchParams((current) => {
  const next = new URLSearchParams(current);
  next.set('q', nextQuery);
  return next;
});
```

Use search params for state that should be shareable in the URL, such as:

- Filters.
- Search text.
- Sort order.
- Pagination.
- Tabs when deep-linking matters.

Do not store sensitive values in search params.

---

## Data loading

### Loaders in Data Mode

Use route loaders to fetch data before rendering a route.

```tsx
import { createBrowserRouter, useLoaderData } from 'react-router';

type Project = {
  id: string;
  name: string;
};

async function projectLoader({ params }: { params: { projectId?: string } }) {
  if (!params.projectId) {
    throw new Response('Missing projectId', { status: 400 });
  }

  const project = await getProject(params.projectId);
  return { project };
}

function ProjectPage() {
  const { project } = useLoaderData() as { project: Project };

  return <h1>{project.name}</h1>;
}

export const router = createBrowserRouter([
  {
    path: '/projects/:projectId',
    loader: projectLoader,
    Component: ProjectPage,
  },
]);
```

Guidance:

- Load route data in loaders instead of `useEffect` where possible.
- Validate params in the loader.
- Throw or return route-appropriate responses for not found/unauthorized states.
- Use request signals when making fetches so navigation can cancel old requests.

```tsx
async function loader({ request, params }: Route.LoaderArgs) {
  return fetch(`/api/projects/${params.projectId}`, {
    signal: request.signal,
  });
}
```

### Loaders in Framework Mode

Framework Mode route modules can export loaders.

```tsx
import type { Route } from './+types/project';

export async function loader({ params }: Route.LoaderArgs) {
  const project = await getProject(params.projectId);
  return { project };
}

export default function Project({ loaderData }: Route.ComponentProps) {
  return <h1>{loaderData.project.name}</h1>;
}
```

Guidance:

- Prefer generated `Route.*` types.
- Prefer route component props over hooks when generated types are available.
- Keep server-only code out of client-only modules.

### `useLoaderData`

Use `useLoaderData` to read the nearest route loader's returned data in Data Mode.

```tsx
const data = useLoaderData() as LoaderData;
```

In Framework Mode, route component props are often better because they are automatically typed.

### `useRouteLoaderData`

Use `useRouteLoaderData(routeId)` to access loader data from a specific route by ID.

```tsx
const rootData = useRouteLoaderData('root') as RootLoaderData;
```

Good use cases:

- Accessing root auth/session data from nested routes.
- Reading layout-level data.

### `useMatches`

Use `useMatches` to inspect all current route matches and their data/handles.

Good use cases:

- Breadcrumbs.
- Page titles.
- Layout metadata.
- Cross-route UI based on matched routes.

---

## Actions and mutations

### Actions in Data Mode

Use route actions for mutations submitted through `<Form>`, `useFetcher`, or `useSubmit`.

```tsx
import { Form, useActionData } from 'react-router';

async function action({ request }: { request: Request }) {
  const formData = await request.formData();
  const title = String(formData.get('title') ?? '');

  if (!title.trim()) {
    return { error: 'Title is required.' };
  }

  await createProject({ title });
  return redirect('/projects');
}

function NewProjectPage() {
  const actionData = useActionData() as { error?: string } | undefined;

  return (
    <Form method="post">
      <label>
        Project title
        <input name="title" />
      </label>

      {actionData?.error ? <p role="alert">{actionData.error}</p> : null}

      <button type="submit">Create project</button>
    </Form>
  );
}
```

Route config:

```tsx
{
  path: '/projects/new',
  action,
  Component: NewProjectPage,
}
```

After an action completes, React Router revalidates loader data so the UI stays in sync.

### Actions in Framework Mode

Framework route modules can export `action` or `clientAction`.

```tsx
import { redirect } from 'react-router';
import type { Route } from './+types/new-project';

export async function action({ request }: Route.ActionArgs) {
  const formData = await request.formData();
  const title = String(formData.get('title') ?? '');

  const project = await createProject({ title });

  return redirect(`/projects/${project.id}`);
}
```

Use `clientAction` only for browser-only mutations in Framework Mode.

### `useActionData`

Use `useActionData` to read data returned by the latest action for the route.

Good use cases:

- Validation errors.
- Server-calculated form result.
- Non-redirecting mutation responses.

### `<Form>`

Use React Router's `<Form>` for route-aware submissions.

```tsx
import { Form } from 'react-router';

<Form method="post">
  <input name="title" />
  <button type="submit">Save</button>
</Form>
```

Guidance:

- Use `method="post"` for mutations.
- Use `method="get"` for URL search/filter forms.
- Let browser form semantics work where possible.
- Use named inputs.
- Prefer accessible labels.

### `useSubmit`

Use `useSubmit` for programmatic form submission.

```tsx
const submit = useSubmit();

function handleChange(event: React.ChangeEvent<HTMLSelectElement>) {
  submit(event.currentTarget.form);
}
```

Good use cases:

- Submit filters when a field changes.
- Programmatic autosave.
- Custom form controls.

Prefer `<Form>` for normal forms.

---

## Fetchers

### `useFetcher`

Use `useFetcher` for route actions/loaders that should not cause navigation.

```tsx
import { useFetcher } from 'react-router';

function FavoriteButton({ projectId }: { projectId: string }) {
  const fetcher = useFetcher();

  const isSubmitting = fetcher.state !== 'idle';

  return (
    <fetcher.Form method="post" action={`/projects/${projectId}/favorite`}>
      <button type="submit" disabled={isSubmitting}>
        {isSubmitting ? 'Saving...' : 'Favorite'}
      </button>
    </fetcher.Form>
  );
}
```

Good use cases:

- Inline mutations.
- Toggle buttons.
- Autocomplete.
- Loading data for a popover.
- Submitting forms without changing the current route.
- Multiple independent pending states on a single page.

### `useFetchers`

Use `useFetchers` to inspect all active fetchers.

Good use cases:

- Global loading indicators.
- Optimistic UI across multiple fetcher submissions.
- Coordinating pending mutations.

Use sparingly.

---

## Pending UI

### `useNavigation`

Use `useNavigation` to show pending state for navigations and form submissions.

```tsx
import { useNavigation } from 'react-router';

function RootLayout() {
  const navigation = useNavigation();
  const isNavigating = navigation.state !== 'idle';

  return (
    <>
      {isNavigating ? <GlobalProgressBar /> : null}
      <Outlet />
    </>
  );
}
```

Navigation states commonly include:

- `idle`
- `loading`
- `submitting`

Use this for global route-level pending UI.

### Fetcher pending state

For local non-navigation mutations, use the fetcher state instead:

```tsx
const fetcher = useFetcher();
const isSaving = fetcher.state !== 'idle';
```

Do not use global navigation state for local fetcher-only pending UI.

---

## Error handling

### Route error boundaries

In Data Mode, route objects can define error boundaries. In Framework Mode, route modules can export an `ErrorBoundary`.

Framework Mode example:

```tsx
import {
  isRouteErrorResponse,
  useRouteError,
} from 'react-router';

export function ErrorBoundary() {
  const error = useRouteError();

  if (isRouteErrorResponse(error)) {
    return (
      <section>
        <h1>{error.status}</h1>
        <p>{error.statusText}</p>
      </section>
    );
  }

  return <p>Something went wrong.</p>;
}
```

Guidance:

- Use route error boundaries for loader/action/render errors.
- Use `isRouteErrorResponse` to detect thrown route responses.
- Avoid exposing sensitive server errors in UI.
- Provide useful recovery paths.

### `useRouteError`

Use `useRouteError` inside an error boundary to read the thrown error.

### `isRouteErrorResponse`

Use `isRouteErrorResponse` to distinguish route response errors from unknown errors.

---

## Redirects and auth

### Loader-based auth redirect

Prefer loader redirects in Data or Framework Mode.

```tsx
import { redirect } from 'react-router';

export async function loader({ request }: Route.LoaderArgs) {
  const user = await requireUser(request);

  if (!user) {
    throw redirect('/login');
  }

  return { user };
}
```

Good pattern:

- Check auth in parent layout loaders.
- Load session/user once at a root or protected layout.
- Redirect before rendering protected UI.
- Do not rely only on client-side guards for security.

### Declarative protected route

In Declarative Mode, use a wrapper component.

```tsx
function RequireAuth({ children }: { children: React.ReactNode }) {
  const auth = useAuth();

  if (!auth.user) {
    return <Navigate to="/login" replace />;
  }

  return children;
}
```

---

## TypeScript guidance

### Route params

Validate params before using them.

```tsx
const { projectId } = useParams();

if (!projectId) {
  throw new Error('Missing projectId');
}
```

Framework Mode preferred:

```tsx
import type { Route } from './+types/project';

export async function loader({ params }: Route.LoaderArgs) {
  return getProject(params.projectId);
}
```

### Loader data

Data Mode often needs explicit typing around `useLoaderData`.

```tsx
type LoaderData = {
  project: Project;
};

const { project } = useLoaderData() as LoaderData;
```

Framework Mode should use generated route props:

```tsx
export default function Project({ loaderData }: Route.ComponentProps) {
  return <h1>{loaderData.project.name}</h1>;
}
```

### Action data

```tsx
type ActionData = {
  error?: string;
};

const actionData = useActionData() as ActionData | undefined;
```

### Handles and matches

Use typed route handles when building breadcrumbs or metadata.

```tsx
type BreadcrumbHandle = {
  breadcrumb?: (match: unknown) => React.ReactNode;
};
```

Keep this project-specific rather than forcing a generic abstraction.

---

## Route modules in Framework Mode

A route module can export route-specific behavior.

Common exports:

- `default` route component.
- `loader`
- `clientLoader`
- `action`
- `clientAction`
- `ErrorBoundary`
- `HydrateFallback`
- `headers`
- `links`
- `meta`
- `shouldRevalidate`
- `handle`

Example:

```tsx
import type { Route } from './+types/team';

export async function loader({ params }: Route.LoaderArgs) {
  const team = await getTeam(params.teamId);
  return { team };
}

export function meta({ data }: Route.MetaArgs) {
  return [{ title: data ? `${data.team.name} | Teams` : 'Team' }];
}

export default function Team({ loaderData }: Route.ComponentProps) {
  return <h1>{loaderData.team.name}</h1>;
}

export function ErrorBoundary() {
  return <p>Unable to load team.</p>;
}
```

Guidance:

- Prefer generated `Route.*` types.
- Keep route module exports colocated with route UI.
- Do not call React Hooks in loaders/actions.
- Loaders/actions run outside component render.
- Server-only loaders/actions should not depend on browser APIs.
- Client loaders/actions can use browser APIs but only run in the browser.

---

## Declarative Mode API patterns

### Basic route tree

```tsx
import { Route, Routes } from 'react-router';

<Routes>
  <Route path="/" element={<RootLayout />}>
    <Route index element={<HomePage />} />
    <Route path="projects" element={<ProjectsPage />} />
    <Route path="projects/:projectId" element={<ProjectPage />} />
    <Route path="*" element={<NotFoundPage />} />
  </Route>
</Routes>
```

### Nested layout

```tsx
function RootLayout() {
  return (
    <>
      <Header />
      <Outlet />
    </>
  );
}
```

### Not found route

```tsx
<Route path="*" element={<NotFoundPage />} />
```

---

## Data Mode API patterns

### Router creation

Create the router outside React components.

```tsx
export const router = createBrowserRouter([
  {
    path: '/',
    Component: RootLayout,
    children: [
      { index: true, Component: HomePage },
      { path: 'projects', Component: ProjectsPage },
    ],
  },
]);
```

Render it once:

```tsx
createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <RouterProvider router={router} />
  </StrictMode>
);
```

### Route object with loader, action, and error boundary

```tsx
{
  path: 'projects/:projectId',
  loader: projectLoader,
  action: projectAction,
  Component: ProjectPage,
  ErrorBoundary: ProjectErrorBoundary,
}
```

Use `Component` in Data Mode route objects. Avoid mixing `element` and `Component` unless the existing codebase already does.

---

## Framework Mode API patterns

### Route config

```tsx
import {
  type RouteConfig,
  index,
  layout,
  prefix,
  route,
} from '@react-router/dev/routes';

export default [
  layout('./layouts/root.tsx', [
    index('./routes/home.tsx'),
    route('projects', './routes/projects.tsx'),
    route('projects/:projectId', './routes/project.tsx'),
  ]),
] satisfies RouteConfig;
```

### File-system routes

If the project uses `@react-router/fs-routes`, follow that convention instead of replacing it with explicit route config.

### Route component props

Prefer:

```tsx
export default function Project({ loaderData, params }: Route.ComponentProps) {
  return <ProjectView project={loaderData.project} projectId={params.projectId} />;
}
```

Over:

```tsx
const data = useLoaderData();
const params = useParams();
```

when generated route types are available.

---

## Forms

### GET forms for filtering/search

```tsx
<Form method="get">
  <label>
    Search
    <input name="q" defaultValue={q} />
  </label>
  <button type="submit">Search</button>
</Form>
```

Use GET forms when the result should be reflected in the URL.

### POST forms for mutations

```tsx
<Form method="post">
  <label>
    Title
    <input name="title" required />
  </label>
  <button type="submit">Create</button>
</Form>
```

Use POST forms when data changes.

### Validation

Validate in the action, not only in the component.

```tsx
export async function action({ request }: Route.ActionArgs) {
  const formData = await request.formData();
  const title = String(formData.get('title') ?? '');

  if (!title.trim()) {
    return { fieldErrors: { title: 'Title is required.' } };
  }

  await createProject({ title });
  return redirect('/projects');
}
```

---

## State management with React Router

Use URL state for state that should survive refresh or be shareable:

- Search terms.
- Filters.
- Sort order.
- Pagination.
- Selected tab if URL-addressable.

Use loader data for server data:

- Current user.
- Route resources.
- Lists and detail pages.
- Parent layout data.

Use fetchers for independent async mutations or background loads:

- Toggle favorite.
- Inline delete.
- Autocomplete.
- Popover details.

Use component state for local UI state:

- Open/closed menu.
- Unsaved form field state.
- Local hover/selection.
- Client-only display toggles.

Avoid duplicating loader data into component state unless you need an editable draft.

---

## Progressive enhancement

React Router forms and actions are designed to align with web platform form behavior.

Guidance:

- Use real `<form>` semantics through React Router's `<Form>`.
- Use named inputs.
- Prefer server/action validation for mutations.
- Avoid replacing all form behavior with click handlers.
- Make loading and error states accessible.

---

## Scroll restoration

Use `<ScrollRestoration />` where the router/framework expects it, typically in root layouts for Framework Mode.

```tsx
import { ScrollRestoration } from 'react-router';

export function Layout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <body>
        {children}
        <ScrollRestoration />
      </body>
    </html>
  );
}
```

Follow existing project placement.

---

## Metadata and resource links

Framework Mode supports route module exports such as `meta` and `links`.

Example:

```tsx
import type { Route } from './+types/project';

export function meta({ data }: Route.MetaArgs) {
  return [
    { title: data ? `${data.project.name} | Projects` : 'Project' },
    { name: 'description', content: 'Project details' },
  ];
}
```

Use project conventions for document metadata. If the app uses a different framework or head manager, do not introduce React Router metadata exports.

---

## Testing

React Router provides `createRoutesStub` for testing components that depend on router context, loader data, action data, or route matches.

Use it for unit testing reusable components that need React Router context.

Guidance:

- Do not overuse `createRoutesStub` for full application route tests.
- In Framework Mode, be careful testing route components directly with generated `Route.*` types because those types depend on the real app route tree.
- Prefer integration tests for route flows when possible.

---

## Common agent mistakes to avoid

### Do not mix router modes accidentally

Bad:

```tsx
<BrowserRouter>
  <RouterProvider router={router} />
</BrowserRouter>
```

Use one top-level router strategy.

### Do not create Data Routers inside components

Bad:

```tsx
function App() {
  const router = createBrowserRouter(routes);
  return <RouterProvider router={router} />;
}
```

Good:

```tsx
const router = createBrowserRouter(routes);

function App() {
  return <RouterProvider router={router} />;
}
```

### Do not use `useEffect` for route data when loaders are available

Bad in Data/Framework Mode:

```tsx
useEffect(() => {
  fetchProject(projectId).then(setProject);
}, [projectId]);
```

Prefer:

```tsx
export async function loader({ params }: Route.LoaderArgs) {
  return { project: await getProject(params.projectId) };
}
```

### Do not use buttons for navigation

Bad:

```tsx
<button onClick={() => navigate('/projects')}>Projects</button>
```

Prefer:

```tsx
<Link to="/projects">Projects</Link>
```

Use buttons for actions. Use links for navigation.

### Do not trust client-side auth guards

Client guards are useful for UX, but server/data loaders/actions must enforce authorization for protected data and mutations.

### Do not store shareable route state only in component state

For filters/search/pagination, prefer search params.

### Do not use internal APIs

Avoid APIs beginning with:

- `UNSAFE_`
- `unstable_`

unless the project already uses them and the task specifically requires it.

---

## Common imports

### Declarative Mode

```tsx
import {
  BrowserRouter,
  Link,
  NavLink,
  Navigate,
  Outlet,
  Route,
  Routes,
  useLocation,
  useNavigate,
  useParams,
  useSearchParams,
} from 'react-router';
```

### Data Mode

```tsx
import {
  Form,
  Link,
  NavLink,
  Outlet,
  RouterProvider,
  createBrowserRouter,
  redirect,
  useActionData,
  useFetcher,
  useFetchers,
  useLoaderData,
  useMatches,
  useNavigate,
  useNavigation,
  useParams,
  useRouteError,
  useRouteLoaderData,
  useSearchParams,
} from 'react-router';
```

### Framework Mode

```tsx
import type { Route } from './+types/route-name';

import {
  Form,
  Link,
  NavLink,
  Outlet,
  redirect,
  useFetcher,
  useNavigation,
  useRouteError,
} from 'react-router';
```

Route config:

```tsx
import {
  type RouteConfig,
  index,
  layout,
  prefix,
  route,
} from '@react-router/dev/routes';
```

---

## Quick API index

### Components

- `<Await>`
- `<Form>`
- `<Link>`
- `<Links>`
- `<Meta>`
- `<NavLink>`
- `<Navigate>`
- `<Outlet>`
- `<PrefetchPageLinks>`
- `<Route>`
- `<Routes>`
- `<Scripts>`
- `<ScrollRestoration>`

### Declarative routers

- `<BrowserRouter>`
- `<HashRouter>`
- `<MemoryRouter>`
- `<Router>`
- `<StaticRouter>`
- `unstable_HistoryRouter`

### Data routers

- `createBrowserRouter`
- `createHashRouter`
- `createMemoryRouter`
- `createStaticHandler`
- `createStaticRouter`
- `<RouterProvider>`
- `<StaticRouterProvider>`

### Framework routers

- `<ServerRouter>`

### Hooks

- `useActionData`
- `useAsyncError`
- `useAsyncValue`
- `useBeforeUnload`
- `useBlocker`
- `useFetcher`
- `useFetchers`
- `useFormAction`
- `useHref`
- `useInRouterContext`
- `useLinkClickHandler`
- `useLoaderData`
- `useLocation`
- `useMatch`
- `useMatches`
- `useNavigate`
- `useNavigation`
- `useNavigationType`
- `useOutlet`
- `useOutletContext`
- `useParams`
- `useResolvedPath`
- `useRevalidator`
- `useRouteError`
- `useRouteLoaderData`
- `useRoutes`
- `useSearchParams`
- `useSubmit`
- `useViewTransitionState`

Avoid using unstable hooks unless required:

- `unstable_usePrompt`
- `unstable_useRouterState`

### Utilities

- `createPath`
- `createRoutesFromChildren`
- `createRoutesFromElements`
- `createRoutesStub`
- `createSearchParams`
- `data`
- `generatePath`
- `href`
- `isRouteErrorResponse`
- `matchPath`
- `matchRoutes`
- `parsePath`
- `redirect`
- `redirectDocument`
- `renderMatches`
- `replace`
- `resolvePath`

---

## Official documentation links

Primary docs:

- `https://reactrouter.com/home`
- `https://reactrouter.com/start/modes`
- `https://api.reactrouter.com/v7/modules/react-router.html`

Mode docs:

- `https://reactrouter.com/start/declarative/installation`
- `https://reactrouter.com/start/data/installation`
- `https://reactrouter.com/start/framework/installation`

Routing:

- `https://reactrouter.com/start/data/routing`
- `https://reactrouter.com/start/framework/routing`

Data and mutations:

- `https://reactrouter.com/start/data/data-loading`
- `https://reactrouter.com/start/data/actions`
- `https://reactrouter.com/start/framework/data-loading`
- `https://reactrouter.com/start/framework/actions`

Testing:

- `https://reactrouter.com/start/data/testing`
- `https://reactrouter.com/start/framework/testing`

---

## Prompt snippet for coding agents

Use this when asking an LLM to modify routing code:

```text
Use the React Router LLM Reference file. First identify whether this project uses Declarative Mode, Data Mode, or Framework Mode, then stay within that mode. Do not mix <BrowserRouter>/<Routes> with createBrowserRouter/<RouterProvider>. Prefer route loaders/actions/fetchers over useEffect data fetching when the project uses Data or Framework Mode. Use generated Route.* types in Framework Mode. Use <Link> or <NavLink> for navigation, <Form> for route-aware form submissions, and redirect() inside loaders/actions for route-level redirects. Avoid unstable, UNSAFE, or internal React Router APIs unless explicitly requested.
```
