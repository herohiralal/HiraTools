# HiraEvents

### What?

You know how Unity doesn't really support generics in its reflection, in the way UE4 does, and you have to create a dummy UnityEvent every time you want a different dynamic invokation signature?

Yeah here's all the boilerplate for that with support for a few types. It's not everything, but it's better than nothing.

### How?

It's all placed in ``UnityEngine.Events`` namespace so use it in your ``*.cs`` file and you're good to go. It's also the default namespace if you wanna use a ``UnityEvent`` anyway, so it prevents quite a lot of namespace-hogging.

The current list includes:

- BoolEvent
- ByteEvent
- CharEvent
- ColorEvent
- CurveEvent
- FloatEvent
- GradientEvent
- IntEvent
- QuaternionEvent
- StringEvent
- Vector2Event
- Vector3Event