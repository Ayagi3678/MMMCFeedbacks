using System;
using System.Threading;
using MagicTween;
using TMPro;
using UnityEngine;

namespace MMMCFeedbacks.Core
{
    [Serializable]
    public class TMPColorFX : Feedback
    {
        public override int Order => 5;
        public override string MenuString => "UI/TMP/Color (TMP)";
        public override Color TagColor => FeedbackStyling.UIFXColor;
        public override Tween Tween => _tween;
        [Space(10)]
        [SerializeField] private TMP_Text target;
        [SerializeField] private bool resetToInitial;
        [Header("Text Color")]
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Ease)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Curve)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private Color zero=Color.white;
        [SerializeField] private Color one;
        [SerializeField] private float duration = 1;

        private Color _initialColor;
        private Action _onInitialCache;
        private Tween _tween;

        protected override void OnEnable(GameObject gameObject)
        {
            _onInitialCache = () => { if (resetToInitial) target.color=_initialColor; };
        }

        protected override void OnReset()
        {
            if(_tween.IsActive()) _tween.Kill();
        }

        protected override void OnPlay(CancellationToken token)
        {
            _initialColor = target.color;
            _tween= target.TweenColor(zero,one,duration)
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
            if(_tween.IsActive()) _tween.Pause();
        }
    }
}