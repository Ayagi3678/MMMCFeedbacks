using System;
using System.Threading;
using MagicTween;
using UnityEngine;

namespace MMMCFeedbacks.Core
{
    [Serializable]
    public class ShakeRotationFX : Feedback
    {
         public override string MenuString =>"Transform/Shake Rotation";
        public override Color TagColor => FeedbackStyling.TransformFXColor;
        public override Tween Tween => _tween;

        [Space(10)]
        [SerializeField] private SimulationSpace simulationSpace;
        [Space(5)]
        [SerializeField] private Transform target;
        [SerializeField] private bool isRelative = true;
        [SerializeField] private bool resetToInitial;
        [Header("Shake Rotation")]
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),0)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),1)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private int frequency=10;
        [SerializeField] private Vector3 strength=Vector3.one;
        [SerializeField] private float duration=1;
        
        private Action _onInitialCacheWorld;
        private Action _onInitialCacheLocal;
        
        private Vector3 _initialRotation;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onInitialCacheWorld = () => { if (resetToInitial) target.transform.eulerAngles = _initialRotation; };
            _onInitialCacheLocal = () => { if (resetToInitial) target.transform.localEulerAngles = _initialRotation; };
        }
        protected override void OnReset()
        {
            if (_tween.IsActive()) _tween.Kill();
        }

        protected override void OnPlay(CancellationToken token)
        {
            _initialRotation = simulationSpace==SimulationSpace.World?target.eulerAngles:target.localEulerAngles;
            _tween = simulationSpace switch
            {
                SimulationSpace.World => target.ShakeEulerAngles(strength, duration)
                    .SetIgnoreTimeScale(ignoreTimeScale)
                    .SetRelative(isRelative)
                    .SetFrequency(frequency)
                    .OnKill(_onInitialCacheWorld)
                    .OnComplete(_onInitialCacheWorld),
                SimulationSpace.Local => target.ShakeLocalEulerAngles(strength, duration)
                    .SetIgnoreTimeScale(ignoreTimeScale)
                    .SetRelative(isRelative)
                    .SetFrequency(frequency)
                    .OnKill(_onInitialCacheLocal)
                    .OnComplete(_onInitialCacheLocal),
                _ => throw new ArgumentOutOfRangeException()
            };

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