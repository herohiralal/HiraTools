# HiraTools / SerializedInstance

### What?

This one time I was supposed to make a system where I needed to let the designers pick from a list what kind of damage mitigation system they want to use.

For faster iterations, they wanted to have several such methods, and test them out comparatively, switching them repeatedly.

So I came up with this.

Note that this was before SerializedReference was a thing in Unity.

I made an interface:
```c#
public interface IDamageMitigationCalculator
{
    float GetMitigationValue(float damageValue, float armor);
}
```

The designers could make as many classes implementing the interface as they want to come up with as many formulae as they want to calculate the damage mitigation.

And they could select the type of damage mitigation they wanted from a dropdown menu.

## How To Use

> No tutorial, just create an interface (or an abstract class, or even a regular class) and hook up a serialized field like this:
```c#
[SerializedField] [AssignableFrom(IDamageMitigationCalculator)] private SerializedReference damageMitigationCalculatorReference = null;
```

> And then call this function:
```c#
void Foo()
{
    var calculator = damageMitigationCalculatorReference.Get<IDamageMitigationcalculator>();
    var mitigation = calculator.GetMitigationValue(damage, armor); 
}
```

**NOTE: The system caches a single instance of any object of a given type, so it's advisable to not store any state, and instead rely upon more composition-based design patterns.**
> If there are three classes (or structs) that implement the given interface (or abstract class, or just a class) they will all have a single instance each.

> This method requires a parameterless constructor for the target type.