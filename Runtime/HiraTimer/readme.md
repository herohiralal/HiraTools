# HiraTimer

### What?

If you've ever used ``MonoBehaviour.Invoke`` to time a function, you know how limited it is.

You can't pause the timer, modify it after having called it, there's no real way to ignore the TimeScale and what not.

Enter HiraTimer, which gives you some of that control. Not a lot, but better than none.

## How To Use

#### 1. Start the Timer

> Yeah, there's no real steps, there's just one method:
```c#
var tracker = HiraTimerEvents.RequestPing(() => Debug.Log("Invoked"), 10, true, false);
```

> The first parameter is the callback after the timer has expired.

> The second parameter is the duration of the timer in seconds. In the given example, it is 10 seconds.

> The third parameter is whether or not you wish for the timer to start automatically.
> If you do not wish to start automatically, you can start it later by using:
```c#
if (!tracker.HasStarted) tracker.Start();
```

> The fourth parameter determines whether or not you wish to ignore the timescale.

#### 2. Pausing the Timer

> Just call the appropriate method:
```c#
tracker.Pause();
```

**NOTE: Realtime (timescale-ignored) timers cannot be paused once they're started.**

#### 3. Resuming the Timer after pausing it

> Just call the appropriate method:
```c#
tracker.Resume();
```

> If you chose to not start the timer automatically, this can be used to start it.

> You can also check whether the timer is paused:
```c#
if(tracker.IsPaused) tracker.Resume();
```

#### 4. Stopping the Timer

> Just call the appropriate method:
```c#
tracker.Stop();
```

> You can choose to explicitly enable the callback.
```c#
tracker.Stop(true);
```

> Also note that stopping the timer, or the timer running out will render your tracker useless. You can query its validity with:
```c#
if(tracker.IsValid) { /* something */ }
```

#### 5. Accessing the time remaining

> Often times as game developers, we need to determine how much time exactly remains until the callback is invoked.

> You can do so using the property:
```c#
var timeRemaining = tracker.Timer;
``` 

> You can also modify the timer if you so choose.
```c#
tracker.Timer += 15;
```