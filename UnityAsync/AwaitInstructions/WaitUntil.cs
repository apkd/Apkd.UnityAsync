using System;

namespace Apkd.Internal
{
    public struct WaitUntil : IAwaitInstruction
    {
        readonly Func<bool> condition;
        readonly UnityEngine.Object owner;

        bool IAwaitInstruction.IsCompleted() => owner && condition();

        /// <summary>
        /// Waits until the condition returns true before continuing.
        /// </summary>
        public WaitUntil(Func<bool> condition, UnityEngine.Object owner = null)
        {
#if UNITY_EDITOR
            if (condition == null)
            {
                condition = () => true;
                UnityEngine.Debug.LogError($"{nameof(condition)} should not be null. This check will only appear in edit mode.");
            }
#endif

            this.condition = condition;
            this.owner = owner ?? AsyncManager.Instance;
        }

        public Continuation<WaitUntil> GetAwaiter() => new Continuation<WaitUntil>(this);
    }
}