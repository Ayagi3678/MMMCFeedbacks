using System;
using System.Threading;
using MagicTween;
using UnityEngine;

namespace MMMCFeedbacks.Core
{
    [Serializable]
    public class ScaleFX : Feedback
    {
        public override string MenuString =>"Transform/Scale";
        public override Color TagColor => FeedbackStyling.TransformFXColor;
        public override Tween Tween => _tween;

        [Space(10)]
        [SerializeField] private Transform target;
        [SerializeField] private bool isRelative = true;
        [SerializeField] private bool resetToInitial;
        [Header("Scale")]
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Ease)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Curve)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private Vector3 zero;
        [SerializeField] private Vector3 one;
        [SerializeField] private float duration=1;
        
        private Action _onInitialCache;

        private Vector3 _initialScale;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onInitialCache = () => { if (resetToInitial) target.transform.localScale = _initialScale; };
        }
        protected override void OnReset()
        {
            if (_tween.IsActive()) _tween.Kill();
        }

        protected override void OnPlay(CancellationToken token)
        {
            _initialScale = target.localScale;
            _tween = target.TweenLocalScale(zero, one, duration)
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