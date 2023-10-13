using System;
using System.Threading;
using MagicTween;
using MMMCFeedbacks.Core.Extension;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MMMCFeedbacks.Core
{
    [Serializable]
    public class VignetteIntensityFX : Feedback
    {
        public override int Order => 7;
        public override string MenuString => "Volume/Vignette/Intensity (Vignette)";
        public override Color TagColor => FeedbackStyling.VolumeFXColor;
        public override Tween Tween => _tween;
        [Space(10)]
        [SerializeField] private Volume target;
        [SerializeField] private bool resetToInitial;
        [Header("Intensity")]
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),0)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),1)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private float zero=1;
        [SerializeField] private float one;
        [SerializeField] private float duration=1;
        private Action _onInitialCache;
        private Vignette _vignette;
        private float _initialValue;
        private bool _initialOverrideState;
        private bool _initialActive;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onInitialCache= () =>
            {
                if (resetToInitial) Initialize();
            };
        }
        protected override void OnReset()
        {
            if (_tween.IsActive()) _tween.Kill();
        }

        protected override void OnPlay(CancellationToken token)
        {
            _vignette ??= target.TryGetVolumeComponent<Vignette>();
            if (resetToInitial) SetInitial();
            _vignette.EnableVolumeParameter(_vignette.intensity);
            _tween = _vignette.TweenIntensity(zero, one, duration)
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
        private void Initialize()
        {
            _vignette.active=_initialActive;
            _vignette.intensity.value = _initialValue;
            _vignette.intensity.overrideState=_initialOverrideState;
        }

        private void SetInitial()
        {
            _initialActive=_vignette.active;
            _initialValue=_vignette.intensity.value;
            _initialOverrideState=_vignette.intensity.overrideState;
        }
    }
}