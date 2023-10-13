using System;
using UniRx;

namespace MMMCFeedbacks.Core
{
    public interface ITimeRequest
    {
        public int Priority { get; }
        public float TimeScale { get; }
        public event Action OnDiscard;
    }
}