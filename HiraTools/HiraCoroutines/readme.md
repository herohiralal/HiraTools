# HiraCoroutines

### What?

``HiraCoroutines`` is a dummy MonoBehaviour singleton that gets created when it's needed, and is then used to run any coroutines that [**HiraTimer**](../HiraTimer) or [**HiraTweener**](../HiraTweener) might require.

There's also some boilerplate for those two tools.

> ~~There is nothing else to do here, unless you are looking for a template singleton that creates itself and maintains the reference in a safe way with minimal runtime checks.~~
> For a template singleton, you can check out HiraManagers.