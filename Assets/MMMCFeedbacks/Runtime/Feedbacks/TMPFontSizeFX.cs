using System;
using System.Threading;
using MagicTween;
using TMPro;
using UnityEngine;

namespace MMMCFeedbacks.Core
{
    [Serializable]
    public class TMPFontSizeFX : Feedback
    {
        public override int Order => 5;
        public override string MenuString => "UI/TMP/Font Size (TMP)";
        public override Color TagColor => FeedbackStyling.UIFXColor;
        public override Tween Tween => _tween;
        [Space(10)]
        [SerializeField] private TMP_Text target;
        [SerializeField] private bool isRelative;
        [SerializeField] private bool resetToInitial;
        [Header("Char Spacing")]
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Ease)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Curve)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private float zero;
        [SerializeField] private float one = 20;
        [SerializeField] private float duration = 1;

        private float _initialSpacing;
        private Action _onInitialCache;
        private Tween _tween;

        protected override void OnEnable(GameObject gameObject)
        {
            _onInitialCache = () => { if (resetToInitial) target.characterSpacing=_initialSpacing; };
        }

        protected override void OnReset()
        {
            if(_tween.IsActive()) _tween.Kill();
        }

        protected override void OnPlay(CancellationToken token)
        {
            _initialSpacing = target.characterSpacing;
            _tween= target.TweenFontSize(zero,one,duration)
                .SetRelative(isRelative)
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