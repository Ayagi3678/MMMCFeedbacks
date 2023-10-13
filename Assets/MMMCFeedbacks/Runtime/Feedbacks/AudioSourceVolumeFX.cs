using System;
using System.Threading;
using MagicTween;
using UnityEngine;

namespace MMMCFeedbacks.Core
{
    [Serializable]
    public class AudioSourceVolumeFX : Feedback
    {
        public override int Order => 11;
        public override string MenuString => "AudioSource/Volume (AudioSource)";
        public override Color TagColor => FeedbackStyling.AudioFXColor;
        public override Tween Tween => _tween;

        [Space(10)] 
        [SerializeField] private AudioSource target;
        [SerializeField] private bool resetToInitial = true;
        [Header("Volume")]
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Ease)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Curve)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private float zero = 1;
        [SerializeField] private float one;
        [SerializeField] private float duration=1;
        private Action _onInitialCache;
        private float _initialVolume;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onInitialCache = () => { if (resetToInitial) target.volume = _initialVolume; };
        }
        protected override void OnReset()
        {
            if (_tween.IsActive()) _tween.Kill();
        }

        protected override void OnPlay(CancellationToken token)
        {
            _initialVolume = target.volume;
            _tween = target.TweenVolume(zero, one, duration)
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