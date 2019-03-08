using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Apkd.Internal;
using Object = UnityEngine.Object;

namespace Apkd
{
    public static class UnityAsync
    {
        public static class Static
        {
            static readonly Continuation<WaitForFrames> nextUpdate = new Continuation<WaitForFrames>(new WaitForFrames(1));

            /// <summary>
            /// Quick access to Unity's <see cref="System.Threading.SynchronizationContext"/>.
            /// </summary>
            public static SynchronizationContext UnitySyncContext
                => AsyncManager.UnitySyncContext;

            /// <summary>
            /// Quick access to the background <see cref="System.Threading.SynchronizationContext"/>.
            /// </summary>
            public static SynchronizationContext BackgroundSyncContext
                => AsyncManager.BackgroundSyncContext;

            /// <summary>
            /// Convenience function to skip a single frame, equivalent to Unity's <c>yield return null</c>.
            /// </summary>
            public static ref readonly Continuation<WaitForFrames> NextUpdate
                => ref nextUpdate;

            /// <summary>
            /// Convenience function to skip a number of frames, equivalent to multiple <c>yield return null</c>s.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Continuation<WaitForFrames> Updates(int count)
                => new Continuation<WaitForFrames>(new WaitForFrames(count));

            /// <summary>
            /// Convenience function to skip multiple frames and continue in the LateUpdate loop.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Continuation<WaitForFrames> UpdatesLate(int count)
                => new Continuation<WaitForFrames>(new WaitForFrames(count), FrameScheduler.LateUpdate);

            /// <summary>
            /// Convenience function to skip multiple FixedUpdate frames and continue in the FixedUpdate loop.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Continuation<WaitForFrames> UpdatesFixed(int count)
                => new Continuation<WaitForFrames>(new WaitForFrames(count), FrameScheduler.FixedUpdate);

            /// <summary>
            /// Convenience function to wait for a number of in-game seconds before continuing, equivalent to Unity's
            /// <see cref="UnityEngine.WaitForSeconds"/>.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static WaitForSeconds Delay(float seconds)
                => new WaitForSeconds(seconds);

            /// <summary>
            /// Convenience function to wait for a number of unscaled seconds before continuing. Equivalent to Unity's
            /// <see cref="UnityEngine.WaitForSecondsRealtime"/>.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static WaitForSecondsRealtime DelayUnscaled(float seconds)
                => new WaitForSecondsRealtime(seconds);

            /// <summary>
            /// Convenience function to wait for a condition to return true. Equivalent to Unity's
            /// <see cref="UnityEngine.WaitUntil"/>.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static WaitUntil Until(Func<bool> condition)
                => new WaitUntil(condition);

            /// <summary>
            /// Convenience function to wait for a condition to return false. Equivalent to Unity's
            /// <see cref="UnityEngine.WaitWhile"/>.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static WaitWhile While(Func<bool> condition)
                => new WaitWhile(condition);
        }

        /// <summary>
        /// Quick access to Unity's <see cref="System.Threading.SynchronizationContext"/>.
        /// </summary>
        public static SynchronizationContext UnitySyncContext
            => AsyncManager.UnitySyncContext;

        /// <summary>
        /// Quick access to the background <see cref="System.Threading.SynchronizationContext"/>.
        /// </summary>
        public static SynchronizationContext BackgroundSyncContext
            => AsyncManager.BackgroundSyncContext;

        /// <summary>
        /// Convenience function to skip a single frame, equivalent to Unity's <c>yield return null</c>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Continuation<WaitForFrames> AsyncNextUpdate(this Object obj)
            => new Continuation<WaitForFrames>(new WaitForFrames(1, obj));

        /// <summary>
        /// Convenience function to skip a number of frames, equivalent to multiple <c>yield return null</c>s.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Continuation<WaitForFrames> AsyncUpdates(this Object obj, int count)
            => new Continuation<WaitForFrames>(new WaitForFrames(count, obj));

        /// <summary>
        /// Convenience function to skip multiple frames and continue in the LateUpdate loop.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Continuation<WaitForFrames> AsyncUpdatesLate(this Object obj, int count)
            => new Continuation<WaitForFrames>(new WaitForFrames(count, obj), FrameScheduler.LateUpdate);

        /// Convenience function to skip multiple FixedUpdate frames and continue in the FixedUpdate loop.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Continuation<WaitForFrames> AsyncUpdatesFixed(this Object obj, int count)
            => new Continuation<WaitForFrames>(new WaitForFrames(count, obj), FrameScheduler.FixedUpdate);

        /// <summary>
        /// Convenience function to wait for a number of in-game seconds before continuing, equivalent to Unity's
        /// <see cref="UnityEngine.WaitForSeconds"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Continuation<WaitForSeconds> AsyncDelay(this Object obj, float seconds)
            => new Continuation<WaitForSeconds>(new WaitForSeconds(seconds, obj));

        /// <summary>
        /// Convenience function to wait for a number of unscaled seconds before continuing. Equivalent to Unity's
        /// <see cref="UnityEngine.WaitForSecondsRealtime"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Continuation<WaitForSecondsRealtime> AsyncDelayUnscaled(this Object obj, float seconds)
            => new Continuation<WaitForSecondsRealtime>(new WaitForSecondsRealtime(seconds, obj));

        /// <summary>
        /// Convenience function to wait for a condition to return true. Equivalent to Unity's
        /// <see cref="UnityEngine.WaitUntil"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Continuation<WaitUntil> AsyncUntil(this Object obj, Func<bool> condition)
            => new Continuation<WaitUntil>(new WaitUntil(condition, obj));

        /// <summary>
        /// Convenience function to wait for a condition to return false. Equivalent to Unity's
        /// <see cref="UnityEngine.WaitWhile"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Continuation<WaitWhile> AsyncWhile(this Object obj, Func<bool> condition)
            => new Continuation<WaitWhile>(new WaitWhile(condition, obj));
    }
}