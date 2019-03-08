using System;

namespace Apkd.Internal
{
    public struct WaitForSecondsRealtime : IAwaitInstruction
    {
        readonly float finishTime;
        readonly UnityEngine.Object owner;

        bool IAwaitInstruction.IsCompleted() => owner && AsyncManager.CurrentUnscaledTime >= finishTime;

        /// <summary>
        /// Waits for the specified number of (unscaled) seconds to pass before continuing.
        /// </summary>
        public WaitForSecondsRealtime(float seconds, UnityEngine.Object owner = null)
        {
            finishTime = AsyncManager.CurrentUnscaledTime + seconds;
            this.owner = owner ?? AsyncManager.Instance;
        }

        public Continuation<WaitForSecondsRealtime> GetAwaiter() => new Continuation<WaitForSecondsRealtime>(this);
    }
}