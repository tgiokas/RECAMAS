# Copilot Instructions

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Folder Responsibilities

### `folder-structure/`

Feature-based (feature-sliced) structure. Code is grouped by **domain feature**, not by technical type. Everything a feature needs lives inside `src/features/<feature>/`; only genuinely shared code lives at the `src/` level.

Rules:

- A feature owns its `api/`, `components/`, `config/`, `hooks/`, slice, selectors and types. Keep them inside the feature until a second feature actually needs them.
- Feature `config/` holds that feature's static data. App-wide design tokens and routes belong in `src/constants/`.
- Promote code out of a feature only when it is genuinely shared: shared UI → `src/components/`, shared hooks → `src/hooks/`, shared types → `src/types/`, shared helpers → `src/utils/`.
- A feature that exposes state, hooks, providers or config to the rest of the app declares its public API in `index.ts`, and consumers import through that barrel rather than reaching into its internals.

  ✅ `import { useAuth } from '@/features/auth';`

  ❌ `import { useAuth } from '@/features/auth/hooks/useAuth';`

- A feature that is only a component (or a few components) rendered by one page or layout does not need a barrel — import the component by its path. Add an `index.ts` once the feature exposes a slice, a hook, or config to the rest of the app, or gains a second consumer.
- Features must not import from each other's internals. Cross-feature usage goes through the other feature's `index.ts`; if two features need the same thing, move it to a shared folder.
- `pages/` owns the route components themselves — a page is a real component, not a pointer at one. What stays in a feature is the reusable part: `api/`, `hooks/`, `config/`, slice, types, and sub-components with more than one caller. Features must never import from `pages/`.
- `app/` only wires things together (store, root reducer, router). No business logic, no UI.
- Use the `@/` alias for all cross-folder imports (`@/features/auth`, `@/components/button`). Relative imports only inside the same folder.

Naming:

- Feature folders: `camelCase` (`auth`, `userManagement`).
- Component folders: `camelCase` (`loginForm`, `button`); the component file itself is `PascalCase` (`LoginForm.tsx`).
- Page folders and files: `PascalCase`, folder and file the same name (`LoginPage/LoginPage.tsx`), and the exported component takes that name too (`export default function LoginPage()`). Page folders get no `index.ts` — import the file (`@pages/LoginPage/LoginPage`).
- Feature-level files are prefixed with the feature name: `authSlice.ts`, `authSelectors.ts`, `authTypes.ts`, `authApi.ts`.
- Hooks: `use<Thing>.ts` (`useAuth.ts`).

When adding a new feature, create the folder with only the parts it needs — do not scaffold empty `api/`, `services/`, `utils/` folders "just in case".

The folders described below (`components/`, `pages/`, `hooks/`, `api/`, `services/`, `utils/`) follow the same rules whether they appear at the `src/` level or inside a feature.

---

### `components/`

Reusable UI components

```text
components/
  UserCard.tsx
  UserTable.tsx
  UserAvatar.tsx
  UserDeleteDialog.tsx
```

- Focus on rendering.
- Keep business logic out of components.
- Receive prepared data via props.
- Avoid fetching data directly inside UI components.

✅

```tsx
<UserCard user={user} />
```

❌

```tsx
<UserCard userId={id} />
```

### `pages/`

Route components. One folder per page, named after the page, holding the component and its stylesheet.

```text
pages/
  UsersPage/
    UsersPage.tsx
    UsersPage.module.css
  UserDetailsPage/
    UserDetailsPage.tsx
```

Only `app/router.tsx` imports from here.

Responsibilities:

- **Be the route component.** The page is where the screen actually lives — its state, its handlers, its data hooks and its layout. It is not a wrapper around a component kept somewhere else.
- Compose feature components and call feature hooks.
- Read and write through `features/*/api` — never `fetch` directly.

Rules:

- **No re-export files.** A page that is one line pointing at another module is indirection with no payoff: it hides where the screen lives and costs a hop every time someone opens it.

  ✅

  ```tsx
  // pages/DashboardPage/DashboardPage.tsx
  export default function DashboardPage() {
    const { truncated, totalUsers } = useDashboardData();
    return <section>…</section>;
  }
  ```

  ❌

  ```tsx
  // pages/DashboardPage.tsx
  export { default } from '@features/dashboard/components/dashboard/Dashboard';
  ```

- A page holding real logic is expected, not a smell. Local state, effects, dialog handling and grid configuration all belong here.
- Move code out to `features/` when something else needs it, or when one page grows several genuinely independent parts. A sub-component used only by its own page can stay in the page file — keep it module-private so fast refresh keeps working.
- Keep HTTP in `features/*/api` and cross-cutting rules in `services/`. A long page is fine; a page that talks to the network itself is not.

### `hooks/`

Feature-specific custom hooks.

```text
hooks/
  useUsers.ts
  useUser.ts
  useDeleteUser.ts
```

Responsibilities:

- Encapsulate React-specific logic.
- Coordinate UI interactions.

Avoid hooks that simply wrap `useState` without adding meaningful behavior.

---

### `api/`

HTTP communication only.

```text
api/
  getUsers.ts
  createUser.ts
  updateUser.ts
  deleteUser.ts
```

Example:

```ts
export async function getUsers() {
  return apiClient.get('/users');
}
```

Rules:

- No business logic.
- No React hooks.
- No data transformations.
- Thin wrapper around the HTTP client.

---

### `services/`

Business logic layer.

```text
services/
  userService.ts
```

Responsibilities:

- Validation.
- Combining multiple API calls.
- Data transformations.
- Domain/business rules.

Only create services when they provide real value.

---

### `utils/`

Pure helper functions.

```text
utils/
  formatUserName.ts
  calculateAge.ts
```

Rules:

- No React.
- No API calls.
- No browser side effects.
- Pure functions only.

---

## SOLID

Apply SOLID pragmatically — it exists to keep features easy to change, not to justify extra layers. Prefer functions, hooks and props over classes; the principles still apply.

### S — Single Responsibility

One module, one reason to change.

- A component renders. A hook coordinates React state. An `api/` function talks HTTP. A `service` holds business rules. A `util` computes.
- If a file mixes fetching, transforming and rendering, split it along the folder responsibilities above.
- If you cannot describe a file's job in one sentence without "and", split it.

✅

```text
hooks/useUsers.ts          // coordinates state
api/getUsers.ts            // HTTP only
components/UserTable.tsx   // renders rows
```

❌

```text
UserTable.tsx              // fetches, filters, sorts, formats and renders
```

### O — Open/Closed

Extend behaviour without editing existing code.

- Extend components through props (`variant`, `renderItem`, `children`, slots) instead of adding `if (context === 'admin')` branches inside them.
- Replace growing conditionals with a lookup map or config object.
- Adding a new case should mean adding an entry, not editing a `switch` in five places.

✅

```tsx
<Button variant="danger" />
```

❌

```tsx
<Button isDanger isAdminPage isCompactOnUsersPage />
```

### L — Liskov Substitution

A replacement must honour the original contract.

- A wrapper component must keep the props and behaviour of what it wraps (a custom `Input` still accepts `value`, `onChange`, `disabled` and forwards the rest).
- Don't accept a prop and then silently ignore it, and don't narrow a shared type so a valid value stops working.
- Variants of a component must be interchangeable at the call site.

### I — Interface Segregation

Depend only on what you use.

- Give a component the smallest props it needs, not the whole domain object.
- Split fat prop interfaces and fat hooks into focused ones; several small hooks beat one `useUserEverything`.
- Selectors should return the slice a component needs, not whole state trees.

✅

```tsx
<UserAvatar name={user.name} imageUrl={user.imageUrl} />
```

❌

```tsx
<UserAvatar user={user} permissions={permissions} session={session} />
```

### D — Dependency Inversion

Depend on abstractions, not concrete implementations.

- Components depend on hooks; hooks depend on `services`/`api`. Never the other way round.
- Never call `fetch` or the HTTP client directly from a component — go through `api/`, which goes through the shared client in `services/api/`.
- Inject behaviour as props or hook arguments (`onSubmit`, `onSuccess`) so the caller decides what happens; pass values in rather than reaching for globals.
- Keep the dependency direction one-way: `pages/` → `features/` → `api/`/`services/` → `utils/`.

### Applying it

- Refactor toward SOLID when a file gets hard to change, not preemptively. Duplication is cheaper than the wrong abstraction.
- Do not add interfaces, factories or wrapper layers with a single implementation.
- These principles never override the folder rules above — they explain why those rules exist.

---

## TypeScript

### Avoiding `any`

- Avoid the `any` type. It disables type checking and hides real bugs. `@typescript-eslint/no-explicit-any` is an **error**.
- Prefer precise types: domain DTOs/interfaces, `unknown` (then narrow), generics, or union types.
- For third-party libraries (e.g. DevExtreme), use the types the library ships rather than falling back to `any` — e.g. `DataChange<TRowData, TKey>` from `devextreme/common/grids` for grid editing changes.
- If a value's shape is genuinely unknown, use `unknown` and narrow it, not `any`.
- Only use `any` as a last resort when no reasonable type exists; when you must, scope it as narrowly as possible.
- Do not silence errors with `as any`, `@ts-ignore`, or `@ts-expect-error`. Fix the type. If a suppression is unavoidable, use `@ts-expect-error` with a one-line reason — never `@ts-ignore`.
- Watch for `no-unsafe-assignment` / `no-unsafe-member-access` / `no-unsafe-call` / `no-unsafe-return` warnings — they mean an `any` leaked in from an untyped boundary (usually an API response). Type the boundary instead of ignoring the warning.

### Type imports and module syntax

- `verbatimModuleSyntax` is on and `consistent-type-imports` is enforced with `fixStyle: 'separate-type-imports'`. Type-only imports must be separate `import type` statements.

  ✅

  ```ts
  import { useState } from 'react';
  import type { User } from '@/features/users';
  ```

  ❌

  ```ts
  import { useState, type ReactNode } from 'react'; // inline type modifier
  import { User } from '@/features/users'; // type imported as a value
  ```

- Re-export types with `export type { ... }` from a feature's `index.ts`.
- Use ES modules only — no `require`, no `import =`, no namespaces.

### No enums or non-erasable syntax

`erasableSyntaxOnly` is on: TypeScript-only runtime constructs are compile errors. No `enum`, no `namespace`, no constructor parameter properties.

✅

```ts
export const UserStatus = {
  Active: 'ACTIVE',
  Disabled: 'DISABLED',
} as const;

export type UserStatus = (typeof UserStatus)[keyof typeof UserStatus];
```

❌

```ts
export enum UserStatus {
  Active = 'ACTIVE',
}
```

### `type` vs `interface`

- `interface` for object shapes that may be extended or implemented (DTOs, component props).
- `type` for unions, intersections, mapped/conditional types, function signatures and aliases.
- Pick one per shape and stay consistent within a feature; don't declare both.

### Null, undefined and narrowing

- Use `??` instead of `||` for defaults so `0`, `''` and `false` survive (`prefer-nullish-coalescing`).
- Use optional chaining `?.` instead of manual `&&` chains (`prefer-optional-chain`).
- Avoid non-null assertions (`value!`) — `no-non-null-assertion` warns. Narrow with a guard or an early return instead.
- Prefer narrowing over type assertions. `as` does not check anything at runtime; unnecessary assertions are an error (`no-unnecessary-type-assertion`).
- Model "absent" with `undefined` (optional properties) and reserve `null` for values the API genuinely returns as `null`.
- Write code as if `strict` were on: no implicit `any` parameters, handle `undefined` from lookups and array access explicitly.

### Async and promises

- Never leave a promise unhandled — `no-floating-promises` is an error. `await` it, `return` it, or mark fire-and-forget with `void`.
- Don't pass an `async` function where a `void`-returning callback is expected (`no-misused-promises`). In event handlers, call the async work from inside a sync handler.

  ✅

  ```tsx
  <Button onClick={() => void handleSave()} />
  ```

  ❌

  ```tsx
  <Button onClick={handleSave} /> // handleSave is async
  ```

- Only `await` actual thenables (`await-thenable`).
- Type what an `api/` function resolves to — `Promise<User[]>`, not an inferred `Promise<any>`.
- Throw `Error` instances only (`only-throw-error`, `prefer-promise-reject-errors`). In `catch`, the value is `unknown` — narrow it before use.

### Functions, props and generics

- Return types on functions are not required (inference is fine), but annotate exported `api/`, `services/` and `utils/` functions where the return type is non-obvious.
- Declare component props as a named exported `interface`, not an inline object literal, once they exceed a couple of fields.
- Prefer `ReactNode` for children and the library's own event types (`ChangeEvent<HTMLInputElement>`) over `any`.
- Use generics only when a type genuinely varies with the input; a single-use generic is usually a sign it should be a concrete type.
- Keep shared domain types in the owning feature's `<feature>Types.ts`; only cross-feature types belong in `src/types/`.

### Unused and dead code

- `noUnusedLocals` / `noUnusedParameters` are on. Delete unused code rather than commenting it out.
- If an argument must exist but is unused, prefix it with `_` (`_event`, `_error`) — the lint config ignores that prefix for args, vars and caught errors.
- `switch` statements may not fall through (`noFallthroughCasesInSwitch`). For exhaustive unions, add a `default` that asserts `never`.

---

## React Compiler

This project builds with **React Compiler** enabled — `babel-plugin-react-compiler` runs through `@rolldown/plugin-babel` in `vite.config.ts`.

Official documentation: https://react.dev/learn/react-compiler

### What it does

The compiler memoizes components and hooks automatically at build time, caching JSX, derived values and functions. It replaces hand-written memoization: its output is usually as precise or more precise, and it also caches intermediate values inside a component, which `React.memo` alone does not.

- Do not hand-write `useMemo`, `useCallback` or `React.memo` by default.
- Write plain, readable components and let the compiler memoize them.
- Never add memoization pre-emptively "for performance". Add it only for a specific reason from the list below.

✅

```tsx
function UserList({ users, sortBy }: UserListProps) {
  const sorted = [...users].sort((a, b) => a[sortBy] - b[sortBy]);
  return <UserTable users={sorted} />;
}
```

❌

```tsx
function UserList({ users, sortBy }: UserListProps) {
  // the compiler already does this
  const sorted = useMemo(() => [...users].sort((a, b) => a[sortBy] - b[sortBy]), [users, sortBy]);
  return <UserTable users={sorted} />;
}
```

### It only optimizes code that follows the Rules of React

The compiler memoizes only what it can prove is safe. When a component breaks the Rules of React (https://react.dev/reference/rules) it **skips that component entirely** and leaves it exactly as written — with no memoization at all. Skipping is safe for the rest of the codebase, but it is invisible at runtime: the source looks identical either way.

So **"the compiler handles it" is not a safe assumption.** For anything performance-sensitive, confirm the component is actually being compiled.

Common bail-out causes:

- Mutating props, state, or any value after it was created — `user.name = x`, `.push()` on a prop or state array.
- Side effects during render — writing to module-level or external variables, subscribing, mutating a ref.
- Reading or writing `ref.current` during render.
- Conditional or nested hook calls.

**How to check:** run `npm run lint`. `eslint-plugin-react-hooks` is configured with `recommended-latest`, which enables the compiler lints — `react-hooks/purity`, `immutability`, `refs`, `set-state-in-render`, `set-state-in-effect`, `static-components`, `unsupported-syntax`. A report from any of those means that component is not being optimized.

Prefer fixing the violation over leaving a component uncompiled: copy before you sort, move effects into `useEffect`, touch refs only in effects and handlers.

### When to still memoize by hand

"This component contains logic" is **not** by itself a reason to memoize. Deriving, sorting, filtering, computing and defining handlers is exactly what the compiler memoizes. Add manual memoization only for a concrete reason:

1. **The value is an effect dependency** — memoize so a `useEffect` does not re-fire when its dependencies have not meaningfully changed. Prefer first to create the value _inside_ the effect and drop the dependency altogether.
2. **Something outside React compares by reference** — third-party libraries
   (DevExtreme props, imperative APIs) and external stores cannot use the
   compiler's cache, so memoize what you hand to them.
   `react-hooks/incompatible-library` flags these. Weigh it against the cost of
   the identity change: a large `dataSource`, a column list or anything the
   widget rebuilds from is worth memoizing; a handful of static option objects
   is not.
3. **A hook returns a value that is part of its public API** — callers may put it in a dependency array, so its identity must be stable no matter how the caller is compiled (see `useLocalize.ts`, `useLang.ts`).
4. **The component cannot be compiled and is genuinely hot** — a large list or a frequently re-rendering subtree. If the rule violation cannot be fixed, memoize by hand and say why in the PR.
5. **You need precise control** over when a value recomputes and the compiler's heuristics are not what you want.

### Do not strip existing memoization

- Leave existing `useMemo` / `useCallback` / `React.memo` in place. The compiler preserves manual memoization deliberately; removing it changes the compiled output and can change behaviour. `react-hooks/preserve-manual-memoization` is an **error**.
- The existing `useMemo` / `useCallback` calls in the feature components are mostly cheap derived data that the compiler now subsumes. They are still not drive-by cleanup: remove them only deliberately, one at a time, with testing — never while doing unrelated work.

### Escape hatch

`'use no memo'` opts a single function out of compilation. It is a **temporary debugging tool**, not a fix — use it to confirm the compiler is the cause of a bug, then fix the underlying rule violation and remove the directive. Explain any committed use in the PR description.

---

## Architecture Principles

- Keep UI and business logic separate.
- Keep HTTP communication inside `api/`.
- Use `services/` only when business logic exists.
- Keep components small and focused.
- Avoid prop drilling.
- Keep helper functions pure.
- Keep feature code self-contained.
- Avoid unnecessary abstraction and over-engineering.
- Introduce layers only when they solve a real problem.