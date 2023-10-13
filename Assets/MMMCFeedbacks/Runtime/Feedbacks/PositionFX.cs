using System;
using System.Threading;
using MagicTween;
using UnityEngine;

namespace MMMCFeedbacks.Core
{
    [Serializable]
    public class PositionFX : Feedback
    {
        public override string MenuString =>"Transform/Position";
        public override Color TagColor => FeedbackStyling.TransformFXColor;
        public override Tween Tween => _tween;

        [Space(10)]
        [SerializeField] private SimulationSpace simulationSpace;
        [Space(5)]
        [SerializeField] private Transform target;
        [SerializeField] private bool isRelative = true;
        [SerializeField] private bool resetToInitial;
        [Header("Position")]
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),0)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),1)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private Vector3 zero;
        [SerializeField] private Vector3 one;
        [SerializeField] private float duration=1;
        
        private Action _onInitialCacheWorld;
        private Action _onInitialCacheLocal;
        
        private Vector3 _initialPosition;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onInitialCacheWorld = () => { if (resetToInitial) target.transform.position = _initialPosition; };
            _onInitialCacheLocal = () => { if (resetToInitial) target.transform.localPosition = _initialPosition; };
        }
        protected override void OnReset()
        {
            if (_tween.IsActive()) _tween.Kill();
        }

        protected override void OnPlay(CancellationToken token)
        {
            _initialPosition = simulationSpace==SimulationSpace.World?target.position:target.localPosition;
            _tween = simulationSpace switch
            {
                SimulationSpace.World => target.TweenPosition(zero, one, duration)
                    .SetIgnoreTimeScale(ignoreTimeScale)
                    .SetRelative(isRelative)
                    .OnKill(_onInitialCacheWorld)
                    .OnComplete(_onInitialCacheWorld),
                SimulationSpace.Local => target.TweenLocalPosition(zero, one, duration)
                    .SetIgnoreTimeScale(ignoreTimeScale)
                    .SetRelative(isRelative)
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