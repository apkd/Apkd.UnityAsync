namespace Apkd.Internal
{
    public struct WaitForFrames : IAwaitInstruction
    {
        readonly int finishFrame;
        readonly UnityEngine.Object owner;

        bool IAwaitInstruction.IsCompleted() => owner && finishFrame <= AsyncManager.CurrentFrameCount;

        /// <summary>
        /// Waits for the specified number of frames to pass before continuing.
        /// </summary>
        public WaitForFrames(int count, UnityEngine.Object owner = null)
        {
#if UNITY_EDITOR
            if (count <= 0)
            {
                count = 1;
                UnityEngine.Debug.LogError($"{nameof(count)} should be greater than 0. This check will only appear in edit mode.");
            }
#endif

            finishFrame = AsyncManager.CurrentFrameCount + count;
            this.owner = owner ?? AsyncManager.Instance;
        }

        public Continuation<WaitForFrames> GetAwaiter() => new Continuation<WaitForFrames>(this);
    }
}