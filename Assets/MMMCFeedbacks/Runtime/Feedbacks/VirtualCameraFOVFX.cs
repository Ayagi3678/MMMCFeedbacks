using System;
using System.Threading;
using Cinemachine;
using MagicTween;
using UnityEngine;

namespace MMMCFeedbacks.Core
{
    [Serializable] public class VirtualCameraFOVFX : Feedback
    {
        public override int Order => 10;
        public override string MenuString => "Camera/Virtual Camera FOV";
        public override Color TagColor => FeedbackStyling.CameraFXColor;
        public override Tween Tween => _tween;
        [Space(10)]
        [SerializeField] private CinemachineVirtualCamera target;
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
            _setterCache = x => target.m_Lens.FieldOfView = x;
            _onInitialCache = () => { if (resetToInitial) target.m_Lens.FieldOfView = _initialFOV; };
        }
        protected override void OnReset()
        {
            if (_tween.IsActive()) _tween.Kill();
        }

        protected override void OnPlay(CancellationToken token)
        {
            _initialFOV = target.m_Lens.FieldOfView;
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