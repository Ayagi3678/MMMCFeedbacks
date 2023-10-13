using UnityEngine;

namespace MMMCFeedbacks.Core
{
    public abstract class TweenParameter
    {
        [HideInInspector] public bool IsActive = true;
        [HideInInspector] public bool ShowActiveBox;
    }
}