using System;
using MagicTween;
using UnityEngine;

namespace MMMCFeedbacks.Core
{
    [Serializable]
    public class IntTweenParameter : TweenParameter
    {
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Ease)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Curve)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private int zero=1;
        [SerializeField] private int one;
        [SerializeField] private float duration=1;
        
        public Tween DoTween(bool ignoreTimeScale,TweenGetter<int> getter,TweenSetter<int> setter)
        {
            if (!IsActive) return Tween.Empty(0);
            var tween = Tween.FromTo(setter, zero, one, duration)
                .SetIgnoreTimeScale(ignoreTimeScale);
            if (mode == EaseMode.Ease) 
                tween.SetEase(ease);
            else 
                tween.SetEase(curve);
            
            return tween;
        }
        public IntTweenParameter(bool showActiveBox=false) => ShowActiveBox = showActiveBox;

        public IntTweenParameter(bool isActive,bool showActiveBox=false)
        {
            IsActive = isActive;
            ShowActiveBox = showActiveBox;
        }
        public IntTweenParameter(IntTweenParameter parameter)
        {
            IsActive = parameter.IsActive;
            mode = parameter.mode;
            ease = parameter.ease;
            curve = parameter.curve;
            zero = parameter.zero;
            one = parameter.one;
            duration = parameter.duration;
        }
    }
}