# HiraManagers

### What?

An attribute to add to a MonoBehaviour to automate singleton creation.

## How To Use

#### 1. Set-Up

> Add the attribute to the MonoBehaviour. Note that this won't work for non-MonoBehaviour classes for obvious reasons.
```c#
[HiraManager]
public class SomeManager : MonoBehaviour
{
    public void DoSomething()
    {
    }
}
```

> You can include a static property named `Instance` or `Current` to access it later. Set its setter to private to avoid any clients changing the value. The system will still work fine as long as the properties are public and static.
```c#
[HiraManager]
public class SecondManager : MonoBehaviour
{
    public static SecondManager Instance { get; private set; }
}
```

#### 2. Access

> If you chose to add the static property `Instance` or `Current` you can use it now.
```c#
var secondManager = SecondManager.Instance;
```

> You can access the MonoBehaviour in this way, if you chose to not add the static property:
```c#
var someManager = HiraManagers.Get<SomeManager>();
someManager.DoSomething();
```

> It's advisable to cache the value, so as to not query the database repeatedly.

#### 3. Defaults

> You can add the address of the default prefab, if it's present in the Resources folder.
```c#
[HiraManager("Managers/SomeOtherManager")]
public class SomeOtherManager : MonoBehaviour
{
}
```

> If there are any issues with loading a prefab, a new clean instance will be created at runtime.