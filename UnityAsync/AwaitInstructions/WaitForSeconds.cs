using System;

namespace Apkd.Internal
{
    public struct WaitForSeconds : IAwaitInstruction
    {
        readonly float finishTime;

        readonly UnityEngine.Object owner;

        bool IAwaitInstruction.IsCompleted() => owner && AsyncManager.CurrentTime >= finishTime;

        /// <summary>
        /// Waits for the specified number of seconds to pass before continuing.
        /// </summary>
        public WaitForSeconds(float seconds, UnityEngine.Object owner = null)
        {
            finishTime = AsyncManager.CurrentTime + seconds;
            this.owner = owner ?? AsyncManager.Instance;
        }

        public Continuation<WaitForSeconds> GetAwaiter() => new Continuation<WaitForSeconds>(this);
    }
}