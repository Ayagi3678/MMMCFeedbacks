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
    public class BloomTintFX : Feedback
    {
        public override int Order => 7;
        public override string MenuString => "Volume/Bloom/Tint (Bloom)";
        public override Color TagColor => FeedbackStyling.VolumeFXColor;
        public override Tween Tween => _tween;

        [Space(10)]
        [SerializeField] private Volume target;
        [SerializeField] private bool resetToInitial;
        [Header("Tint")]
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),0)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),1)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private Color zero=Color.black;
        [SerializeField] private Color one=Color.white;
        [SerializeField] private float duration=1;
        private Action _onInitialCache;
        private Bloom _bloom;
        private Color _initialValue;
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
            _bloom ??= target.TryGetVolumeComponent<Bloom>();
            if (resetToInitial) SetInitial();
            _bloom.EnableVolumeParameter(_bloom.tint);
            _tween = _bloom.TweenTint(zero, one, duration)
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
            _bloom.active=_initialActive;
            _bloom.tint.value = _initialValue;
            _bloom.tint.overrideState=_initialOverrideState;
        }

        private void SetInitial()
        {
            _initialActive=_bloom.active;
            _initialValue=_bloom.tint.value;
            _initialOverrideState=_bloom.tint.overrideState;
        }
    }
}