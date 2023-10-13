using System;
using System.Threading;
using MagicTween;
using UnityEngine;

namespace MMMCFeedbacks.Core
{
    [Serializable]
    public class CanvasGroupAlphaFX : Feedback
    {
        public override int Order => 8;
        public override string MenuString => "UI/Canvas Group/Alpha (Canvas Group)";
        public override Color TagColor => FeedbackStyling.UIFXColor;
        public override Tween Tween => _tween;

        [Space(10)]
        [SerializeField] private CanvasGroup target;
        [SerializeField] private bool resetToInitial = true;
        [Header("Alpha")]
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Ease)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Curve)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private float zero = 1;
        [SerializeField] private float one;
        [SerializeField] private float duration=1;

        private float _initialAlpha;
        private Action _onInitialCache;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onInitialCache = () => { if (resetToInitial) target.alpha = _initialAlpha; };

        }
        protected override void OnReset()
        {
            if (_tween.IsActive()) _tween.Kill();
        }

        protected override void OnPlay(CancellationToken token)
        {
            _initialAlpha=target.alpha;
            _tween = target.TweenAlpha(zero, one, duration)
                .SetIgnoreTimeScale(ignoreTimeScale)
                .OnKill(_onInitialCache)
                .OnComplete(_onInitialCache);
            if (mode == EaseMode.Ease) 
                _tween.SetEase(ease);
            else 
                _tween.SetEase(curve);
        }
        protected override void OnStop()
        {
            if (_tween.IsActive()) _tween.Pause();
        }
    }
}