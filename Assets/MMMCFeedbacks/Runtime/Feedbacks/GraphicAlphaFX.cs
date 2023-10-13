using System;
using System.Threading;
using MagicTween;
using UnityEngine;
using UnityEngine.UI;

namespace MMMCFeedbacks.Core
{
    [Serializable]
    public class GraphicAlphaFX : Feedback
    {
        public override int Order => 5;
        public override string MenuString => "Graphic/Alpha (Graphic)";
        public override Color TagColor => FeedbackStyling.GraphicFXColor; 
        public override Tween Tween => _tween;
        [Space(10)]
        [SerializeField] private Graphic target;
        [SerializeField] private bool resetToInitial;
        [Header("Alpha")]
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Ease)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Curve)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private float zero = 1;
        [SerializeField] private float one;
        [SerializeField] private float duration=1;

        private Action _onInitialCache;
        private float _initialAlpha;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onInitialCache = () => { if (resetToInitial) target.color=new Color(target.color.r,target.color.g,target.color.b,_initialAlpha); };
        }
        protected override void OnReset()
        {
            if (_tween.IsActive()) _tween.Kill();
        }

        protected override void OnPlay(CancellationToken token)
        {
            _initialAlpha=target.color.a;
            _tween = target.TweenColorAlpha(zero, one, duration)
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