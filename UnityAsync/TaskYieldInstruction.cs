using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Apkd.Internal
{
    /// <summary>
    /// Encapsulates a <see cref="System.Threading.Tasks.Task"/>, allowing it to be yielded in an IEnumerator coroutine.
    /// </summary>
    public sealed class TaskYieldInstruction : CustomYieldInstruction
    {
        readonly Task task;

        public TaskYieldInstruction(Task task)
            => this.task = task;

        public override bool keepWaiting => !task.IsCompleted;
    }

    /// <summary>
    /// Encapsulates a <see cref="System.Threading.Tasks.Task{TResult}"/>, allowing it to be yielded in an IEnumerator coroutine.
    /// </summary>
    public sealed class TaskYieldInstruction<TResult> : IEnumerator<TResult>
    {
        readonly Task<TResult> task;

        public TaskYieldInstruction(Task<TResult> task)
            => this.task = task;

        object IEnumerator.Current => Current;

        /// <summary>
        /// Returns the encapsulated <see cref="System.Threading.Tasks.Task{TResult}"/>'s result if it has completed, otherwise the
        /// default TResult value.
        /// </summary>
        public TResult Current => task.IsCompleted ? task.Result : default(TResult);

        public void Dispose() => task.Dispose();
        public bool MoveNext() => !task.IsCompleted;

        public void Reset() => throw new NotSupportedException();
    }
}