using System;
using System.Threading;
using MagicTween;
using UnityEngine;

namespace MMMCFeedbacks.Core
{
    [Serializable] public class CameraFOVFX : Feedback
    {
        public override int Order => 10;
        public override string MenuString => "Camera/Camera FOV";
        public override Color TagColor => FeedbackStyling.CameraFXColor;
        public override Tween Tween => _tween;

        [Space(10)]
        [SerializeField] private Camera target;
        [SerializeField] private bool resetToInitial;
        [Header("FOV")]
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),0)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),1)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private float zero = 90;
        [SerializeField] private float one = 60;
        [SerializeField] private float duration=1;


        private Action _onInitialCache;
        private TweenSetter<float> _setterCache;
        private float _initialFOV;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onInitialCache = () => { if (resetToInitial) target.fieldOfView = _initialFOV; };
            _setterCache = x => target.fieldOfView = x;
        }
        protected override void OnReset()
        {
            if (_tween.IsActive()) _tween.Kill();
        }
        protected override void OnPlay(CancellationToken token)
        {
            _initialFOV = target.fieldOfView;
            _tween = Tween.FromTo(_setterCache, zero, one, duration)
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