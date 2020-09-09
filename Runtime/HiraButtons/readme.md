# HiraButton

### What?

A way to quickly add buttons to an editor for a MonoBehaviour.

### How?

#### 1. Setup

> Create a private method to be called, and create a HiraButtonToken variable as the object that the button will be shown for.
> The name of the variable will be the text on the button.
> Add the attribute HiraButtonAttribute to the field, and pass in the name of the method as the parameter.

```c#
public class MonoBehaviour1 : MonoBehaviour
{
#if UNITY_EDITOR
    [HiraButton(nameof(DoSomething))] public HiraButtonToken doSomething = default;
    private void DoSomething()
    {
        // Do something
    }
#endif
}
```

> It's a good idea to wrap it up in Editor-only code like this because you probably don't need it in your runtime code.