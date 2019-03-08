using System.Runtime.CompilerServices;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Apkd.Internal;
using Apkd.Internal.Awaiters;

namespace Apkd
{
    public static class Extensions
    {
        /// <summary>
        /// Configure the type of update cycle it should be evaluated on.
        /// </summary>
        /// <returns>A continuation with updated params.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Continuation<T> ConfigureAwait<T>(this T @this, FrameScheduler scheduler) where T : IAwaitInstruction
            => new Continuation<T>(@this).ConfigureAwait(scheduler);

        /// <summary>
        /// Link the <see cref="Apkd.IAwaitInstruction"/>'s lifespan to a
        /// <see cref="System.Threading.CancellationToken"/> and configure the type of update cycle it should be
        /// evaluated on.
        /// </summary>
        /// <returns>A continuation with updated params.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Continuation<T> ConfigureAwait<T>(this T @this, CancellationToken cancellationToken, FrameScheduler scheduler) where T : IAwaitInstruction
            => new Continuation<T>(@this).ConfigureAwait(cancellationToken, scheduler);

        /// <summary>
        /// Link the <see cref="Apkd.IAwaitInstruction"/>'s lifespan to a <see cref="System.Threading.CancellationToken"/>.
        /// </summary>
        /// <returns>A continuation with updated params.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Continuation<T> ConfigureAwait<T>(this T @this, CancellationToken cancellationToken) where T : IAwaitInstruction
            => new Continuation<T>(@this).ConfigureAwait(cancellationToken);

        /// <summary>
        /// Encapsulate the <see cref="System.Threading.Tasks.Task"/> in a <see cref="UnityEngine.CustomYieldInstruction"/>
        /// so that it can be yielded in an IEnumerator coroutine.
        /// </summary>
        public static TaskYieldInstruction AsYieldInstruction(this Task @this) => new TaskYieldInstruction(@this);

        /// <summary>
        /// Encapsulate the <see cref="System.Threading.Tasks.Task{TResult}"/> in a <see cref="UnityEngine.CustomYieldInstruction"/>
        /// so that it can be yielded in an IEnumerator coroutine. The result can be obtained through
        /// <see cref="TaskYieldInstruction{T}.Current"/> after yielding.
        /// </summary>
        public static TaskYieldInstruction<T> AsIEnumerator<T>(this Task<T> @this) => new TaskYieldInstruction<T>(@this);

        public static SynchronizationContextAwaiter GetAwaiter(this SynchronizationContext @this) => new SynchronizationContextAwaiter(@this);
        public static IEnumeratorAwaiter GetAwaiter(this IEnumerator @this) => new IEnumeratorAwaiter(@this);
        public static YieldInstructionAwaiter GetAwaiter(this YieldInstruction @this) => new YieldInstructionAwaiter(@this);
#if UNITY_2018_2_OR_NEWER
        public static ResourceRequestAwaiter GetAwaiter(this ResourceRequest @this) => new ResourceRequestAwaiter(@this);
        public static AsyncOperationAwaiter GetAwaiter(this AsyncOperation @this) => new AsyncOperationAwaiter(@this);
#endif
    }
}