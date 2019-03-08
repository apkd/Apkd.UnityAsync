using System.Collections.Concurrent;

namespace Apkd.Internal
{
    public sealed partial class AsyncManager
    {
        sealed partial class ContinuationProcessorGroup
        {
            const int MaxQueueSize = 4096;

            sealed class ContinuationProcessor<T> : IContinuationProcessor where T : IContinuation
            {
                public static ContinuationProcessor<T> instance;
                readonly ConcurrentQueue<T> queue = new ConcurrentQueue<T>();

                public void Process()
                {
                    for (int n = queue.Count; n > 0; --n)
                        if (queue.TryDequeue(out var continuation))
                            if (!continuation.Evaluate())
                                queue.Enqueue(continuation);
                }

                public void Add(in T cont) => queue.Enqueue(cont);
            }
        }
    }
}