using System;
using System.Threading;
using MagicTween;
using UnityEngine;

namespace MMMCFeedbacks.Core
{
    [Serializable]
    public class SpriteRendererColorFX : Feedback
    {
        public override int Order => 4;
        public override string MenuString => "Renderer/Color (SpriteRenderer)";
        public override Color TagColor => FeedbackStyling.GraphicFXColor; 
        public override Tween Tween => _tween;
        [Space(10)]
        [SerializeField] private SpriteRenderer target;
        [SerializeField] private bool resetToInitial;
        [Header("Color")]
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),0)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),1)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private Color zero=Color.black;
        [SerializeField] private Color one=Color.white;
        [SerializeField] private float duration=1;

        private Action _onInitialCache;
        private Color _initialColor;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onInitialCache = () => { if (resetToInitial) target.color = _initialColor; };
        }
        protected override void OnReset()
        {
            if (_tween.IsActive()) _tween.Kill();
        }

        protected override void OnPlay(CancellationToken token)
        {
            _initialColor=target.color;
            _tween = target.TweenColor(zero, one, duration)
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