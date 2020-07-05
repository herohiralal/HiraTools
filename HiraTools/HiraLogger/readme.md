# HiraLogger

### What?

Generally, ``Debug.Log("Message")`` is much more expensive than people think it is. So logging every frame might not be a good idea.

But working in game dev, sometimes you need those per frame logs. HiraLogger provides an alternate way to do so, using C#'s ``Conditional`` method attribute.

### How?

Just replace ``Debug`` with ``HiraLogger`` (they're both in the same namespace, ``UnityEngine``). That's all on the code side of things.

Here are the scripting define symbols you need to add to Unity's project settings, according to your requirements. You can remove them before building, to disable the logging in a release build.

---

>(To open the related setting, go to *Edit* / *Project Setings*; go to *Player* tab, open the *Other Settings* section, and scroll down to *Scripting Define Symbols*)

---

- ``ENABLE_LOG_MESSAGE`` - **To enable logging of messages.**
> Methods affected: ``HiraLogger.Log``, ``HiraLogger.LogFormat``.
- ``ENABLE_LOG_WARNING`` - **To enable logging of warnings.**
> Methods affected: ``HiraLogger.LogWarning``, ``HiraLogger.LogWarningFormat``.
- ``ENABLE_LOG_ERROR`` - **To enable logging of errors.**
> Methods affected: ``HiraLogger.LogError``, ``HiraLogger.LogErrorFormat``.
- ``ENABLE_LOG_EXCEPTION`` - **To enable logging of exceptions.**
> Methods affected: ``HiraLogger.LogException``.