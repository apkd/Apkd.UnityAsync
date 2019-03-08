# Apkd.UnityAsync

Async/Await library for Unity, based on [muckSponge/UnityAsync](https://github.com/muckSponge/UnityAsync).

# Usage


```csharp
using UnityEngine;
using Apkd;

sealed class Test : MonoBehaviour
{
    async void AsyncDelayExample()
    {
        Debug.Log(Time.time); // 0.01

        // resumes execution after 5 seconds
        await this.AsyncDelay(5);

        Debug.Log(Time.time); // 5.01
    }

    void Start() => AsyncUpdatesExample();
}
```

The awaitable operations are implemented as extension methods to `UnityEngine.Object`. This captures the reference to the current class instance (`this`) and ensures that the asynchronous method will stop executing when the object is destroyed.

Available methods:

```csharp
this.AsyncDelay(float seconds);
this.AsyncDelayUnscaled(float seconds);
this.AsyncNextUpdate();
this.AsyncUpdates(int count);
this.AsyncUpdatesFixed(int count);
this.AsyncUpdatesLate(int count);
this.AsyncUntil(Func<bool> condition);
this.AsyncWhile(Func<bool> condition);
```

# More examples

```csharp
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Apkd;

sealed class Examples : MonoBehaviour
{
    async void AsyncUpdatesExample()
    {
        Debug.Log(Time.frameCount); // 1
        await this.AsyncNextUpdate();
        Debug.Log(Time.frameCount); // 2
    }

    async void AsyncDelayExample()
    {
        Debug.Log(Time.time); // 0.01

        await this.AsyncDelay(5); // resumes execution after 5 seconds
        Debug.Log(Time.time); // 5.01

        await this.AsyncDelayUnscaled(5); // resumes execution after 5 seconds (unscaled time)
        Debug.Log(Time.time); // 10.01
    }

    async void AsyncWhileExample()
    {
        await this.AsyncWhile(() => isActiveAndEnabled); // waits for the condition to become false
        Debug.Log("The component is no longer active");

        await this.AsyncUntil(() => isActiveAndEnabled); // waits for the condition to become true
        Debug.Log("The component has been reactivated");
    }

    async void AsyncContextSwitchingExample()
    {
        Debug.Log($"id={Thread.CurrentThread.ManagedThreadId} pool={Thread.CurrentThread.IsThreadPoolThread}"); // 1 false

        await UnityAsync.BackgroundSyncContext; // switches to a a background context (thread pool)
        Debug.Log($"id={Thread.CurrentThread.ManagedThreadId} pool={Thread.CurrentThread.IsThreadPoolThread}"); // 70 true

        await UnityAsync.UnitySyncContext; // switches back to the unity context
        Debug.Log($"id={Thread.CurrentThread.ManagedThreadId} pool={Thread.CurrentThread.IsThreadPoolThread}"); // 1 false
    }

    static void AsyncInStaticMethodExample()
    {
        // this will bind the awaiter to the AsyncManager singleton instance
        // (no "this" reference available here)
        await UnityAsync.Static.AsyncDelay(5);
    }

    async Task<string> TaskWithResultExample()
    {
        await this.AsyncDelay(1f);
        return "Some result";
    }

    async void Start()
    {
        // the four methods below will run concurrently
        AsyncUpdatesExample();
        AsyncDelayExample();
        AsyncWhileExample();
        AsyncContextSwitchingExample();

        // the call below will be awaited
        string someResult = await TaskWithResultExample();
    }
}

```

## Installation

### Unity Package Manager *(Unity 2018.3+)*

Add a reference to the repository in the [`Packages\manifest.json`](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@1.8/manual/index.html#project-manifests) file in your project directory:

```json
{
  "dependencies": {
    "com.unity.postprocessing": "2.1.3",
    "pl.apkd.unityasync": "https://github.com/apkd/Apkd.UnityAsync.git"
  }
}
```

### Manual

#### From source *(Unity 2018.3+)*

Clone/download this repository into the `Assets` directory of your project.

#### Using compiled DLL *(Unity 2017.1+)*

Download and extract the latest [release zip](https://github.com/apkd/Apkd.UnityAsync/releases) into the `Assets` directory of your project.