using System;

namespace Apkd.Internal
{
    public struct WaitWhile : IAwaitInstruction
    {
        readonly Func<bool> condition;
        readonly UnityEngine.Object owner;

        bool IAwaitInstruction.IsCompleted() => owner && !condition();

        /// <summary>
        /// Waits until the condition returns false before continuing.
        /// </summary>
        public WaitWhile(Func<bool> condition, UnityEngine.Object owner = null)
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

        public Continuation<WaitWhile> GetAwaiter() => new Continuation<WaitWhile>(this);
    }
}