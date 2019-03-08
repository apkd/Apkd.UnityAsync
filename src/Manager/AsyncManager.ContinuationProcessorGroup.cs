using System.Collections.Generic;

namespace Apkd.Internal
{
    public sealed partial class AsyncManager
    {
        sealed partial class ContinuationProcessorGroup
        {
            interface IContinuationProcessor
            {
                void Process();
            }

            readonly List<IContinuationProcessor> processors;

            public ContinuationProcessorGroup(int initialCapacity = 16)
                => processors = new List<IContinuationProcessor>(initialCapacity);

            public void Add<T>(in T cont) where T : IContinuation
            {
                var p = ContinuationProcessor<T>.instance;

                if (p == null)
                {
                    p = ContinuationProcessor<T>.instance = new ContinuationProcessor<T>();
                    processors.Add(ContinuationProcessor<T>.instance);
                }

                p.Add(cont);
            }

            public void Process()
            {
                for (int i = 0; i < processors.Count; ++i)
                    processors[i].Process();
            }
        }
    }
}