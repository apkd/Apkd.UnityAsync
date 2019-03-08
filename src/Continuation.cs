using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Object = UnityEngine.Object;

namespace Apkd.Internal
{
    public interface IContinuation
    {
        bool Evaluate();
        FrameScheduler Scheduler { get; }
    }

    /// <summary>
    /// Encapsulates an <see cref="Apkd.IAwaitInstruction"/> with additional information about how the instruction
    /// will be queued and executed. Continuations are intended to be awaited after or shortly after instantiation.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Apkd.IAwaitInstruction"/> to encapsulate.</typeparam>
    public struct Continuation<T> : IContinuation, INotifyCompletion where T : IAwaitInstruction
    {
        T instruction;
        CancellationToken cancellationToken;
        Action continuation;

        /// <summary>
        /// Evaluate the encapsulated <see cref="Apkd.IAwaitInstruction"/> to determine whether the continuation
        /// is finished and can continue. Will evaluate to true if its owner is destroyed or its cancellation token has
        /// been cancelled.
        /// </summary>
        /// <returns></returns>
        public bool Evaluate()
        {
            if (cancellationToken.IsCancellationRequested || instruction.IsCompleted())
            {
                continuation();
                return true;
            }

            return false;
        }

        public FrameScheduler Scheduler { get; private set; }

        public Continuation(in T inst)
        {
            instruction = inst;
            continuation = null;
            Scheduler = FrameScheduler.Update;
        }

        public Continuation(in T inst, FrameScheduler scheduler)
        {
            instruction = inst;
            continuation = null;
            Scheduler = scheduler;
        }

        public Continuation(in T inst, CancellationToken cancellationToken, FrameScheduler scheduler)
        {
            instruction = inst;
            continuation = null;
            this.cancellationToken = cancellationToken;
            Scheduler = scheduler;
        }

        public bool IsCompleted => false;

        public void OnCompleted(Action continuation)
        {
            this.continuation = continuation;
            AsyncManager.AddContinuation(this);
        }

        /// <summary>
        /// Configure the type of update cycle it should be evaluated on.
        /// </summary>
        /// <returns>A new continuation with updated params.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Continuation<T> ConfigureAwait(FrameScheduler scheduler)
        {
            Scheduler = scheduler;
            return this;
        }

        /// <summary>
        /// Link the continuation's lifespan to a <see cref="System.Threading.CancellationToken"/> and configure the
        /// type of update cycle it should be evaluated on.
        /// </summary>
        /// <returns>A new continuation with updated params.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Continuation<T> ConfigureAwait(CancellationToken cancellationToken, FrameScheduler scheduler)
        {
            this.cancellationToken = cancellationToken;
            Scheduler = scheduler;
            return this;
        }

        /// <summary>
        /// Link the continuation's lifespan to a <see cref="System.Threading.CancellationToken"/>.
        /// </summary>
        /// <returns>A new continuation with updated params.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Continuation<T> ConfigureAwait(CancellationToken cancellationToken)
        {
            this.cancellationToken = cancellationToken;
            return this;
        }

        public void GetResult() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Continuation<T> GetAwaiter() => this;
    }
}