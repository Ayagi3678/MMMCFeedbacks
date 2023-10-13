using System;
using System.Threading;
using MagicTween;
using UnityEngine;

namespace MMMCFeedbacks.Core
{
    [Serializable]
    public class AnchoredPositionFX : Feedback
    {
        public override string MenuString =>"RectTransform/Anchored Position";
        public override Color TagColor => FeedbackStyling.RectTransformFXColor;
        public override Tween Tween => _tween;

        [Space(10)]
        [SerializeField] private RectTransform target;
        [SerializeField] private bool isRelative = true;
        [SerializeField] private bool resetToInitial;
        [Header("Anchored Position")]
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),0)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),1)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private Vector3 zero;
        [SerializeField] private Vector3 one;
        [SerializeField] private float duration=1;
        
        private Action _onInitialCache;

        private Vector3 _initialPosition;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onInitialCache = () => { if (resetToInitial) target.anchoredPosition3D = _initialPosition; };
        }
        protected override void OnReset()
        {
            if (_tween.IsActive()) _tween.Kill();
        }

        protected override void OnPlay(CancellationToken token)
        {
            _initialPosition = target.anchoredPosition3D;
            _tween = target.TweenAnchoredPosition3D(zero, one, duration)
                .SetIgnoreTimeScale(ignoreTimeScale)
                .SetRelative(isRelative)
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