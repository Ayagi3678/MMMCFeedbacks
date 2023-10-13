using System;
using MagicTween;
using UnityEngine;

namespace MMMCFeedbacks.Core
{
    [Serializable]
    public class FloatTweenParameter : TweenParameter
    {
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Ease)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Curve)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private float zero=1;
        [SerializeField] private float one;
        [SerializeField] private float duration=1;

        public Tween ExecuteTween(bool ignoreTimeScale,TweenGetter<float> getter,TweenSetter<float> setter)
        {
            if (!IsActive) return Tween.Empty(0);
            var tween = Tween.FromTo(setter,one,zero,duration)
                .SetIgnoreTimeScale(ignoreTimeScale);
            if (mode == EaseMode.Ease) 
                tween.SetEase(ease);
            else 
                tween.SetEase(curve);
            return tween;
        }
        public FloatTweenParameter(bool showActiveBox=false) => ShowActiveBox = showActiveBox;

        public FloatTweenParameter(bool isActive,bool showActiveBox=false)
        {
            IsActive = isActive;
            ShowActiveBox = showActiveBox;
        }
        public FloatTweenParameter(FloatTweenParameter parameter)
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