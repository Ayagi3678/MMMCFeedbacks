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
    public class ColorAdjustmentsSaturationFX : Feedback
    {
        public override int Order => 7;
        public override string MenuString => "Volume/Color Adjustments/Saturation (Color Adjustments)";
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
        [SerializeField][Range(-100,100)] private float zero=100;
        [SerializeField][Range(-100,100)] private float one;
        [SerializeField] private float duration=1;
        private Action _onInitialCache;
        private ColorAdjustments _colorAdjustments;
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
            _colorAdjustments ??= target.TryGetVolumeComponent<ColorAdjustments>();
            if (resetToInitial) SetInitial();
            _colorAdjustments.EnableVolumeParameter(_colorAdjustments.saturation);
            _tween = _colorAdjustments.TweenSaturation(zero, one, duration)
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
            _colorAdjustments.active=_initialActive;
            _colorAdjustments.saturation.value = _initialValue;
            _colorAdjustments.saturation.overrideState=_initialOverrideState;
        }

        private void SetInitial()
        {
            _initialActive=_colorAdjustments.active;
            _initialValue=_colorAdjustments.saturation.value;
            _initialOverrideState=_colorAdjustments.saturation.overrideState;
        }
    }
}