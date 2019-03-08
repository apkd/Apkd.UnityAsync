#if UNITY_2018_2_OR_NEWER
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Apkd.Internal.Awaiters
{
    public struct AsyncOperationAwaiter : INotifyCompletion
    {
        readonly AsyncOperation op;

        public AsyncOperationAwaiter(AsyncOperation op)
            => this.op = op;

        public void GetResult() { }
        public bool IsCompleted => op.isDone;

        public void OnCompleted(Action action) => op.completed += _ => action();
    }
}
#endif