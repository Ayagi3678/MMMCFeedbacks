using System;
using System.Threading;
using MagicTween;
using UnityEngine;
using UnityEngine.UI;

namespace MMMCFeedbacks.Core
{
    [Serializable]
    public class ImageFillAmountFX : Feedback
    {
        public override int Order => 8;
        public override string MenuString => "UI/Image/FillAmount (Image)";
        public override Color TagColor => FeedbackStyling.UIFXColor;
        public override Tween Tween => _tween;
        [Space(10)]
        [SerializeField] private Image target;
        [SerializeField] private bool resetToInitial = true;
        [Header("FillAmount")]
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Ease)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Curve)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private float zero = 1;
        [SerializeField] private float one;
        [SerializeField] private float duration=1;

        private float _initialFillAmount;
        private Action _onInitialCache;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onInitialCache = () => { if (resetToInitial) target.fillAmount = _initialFillAmount; };
        }
        protected override void OnReset()
        {
            if (_tween.IsActive()) _tween.Kill();
        }

        protected override void OnPlay(CancellationToken token)
        {
            _initialFillAmount=target.fillAmount;
            _tween = target.TweenFillAmount(zero, one, duration)
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