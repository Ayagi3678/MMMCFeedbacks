using System;
using System.Threading;
using MagicTween;
using TMPro;
using UnityEngine;

namespace MMMCFeedbacks.Core
{
    [Serializable]
    public class TMPWaveCharRotationFX : Feedback
    {
         public override int Order => 5;
        public override string MenuString => "UI/TMP/Wave Char Rotation (TMP)";
        public override Color TagColor => FeedbackStyling.UIFXColor;
        public override Tween Tween => _sequence;
        [Space(10)]
        [SerializeField] private TMP_Text target;
        [SerializeField] private bool resetToInitial;
        [Header("Char Rotation")]
        [SerializeField] private EaseMode mode = EaseMode.Curve;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Ease)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Curve)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve= new (new(0,0), new(0.5f,1), new(1,0));
        [SerializeField] private Vector3 zero;
        [SerializeField] private Vector3 one=Vector3.one*360;
        [SerializeField] private float duration=1;

        [Header("Wave")] [NormalizedAnimationCurve()] [SerializeField]
        private AnimationCurve waveCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private float waveDuration=1;

        private Action<int> _onInitializeCache;
        private Sequence _sequence;
        private TMPTweenAnimator _tweenAnimator;

        protected override void OnEnable(GameObject gameObject)
        {
            _onInitializeCache = x =>
            {
                if (resetToInitial) _tweenAnimator.SetCharEulerAngles(x,Vector3.zero);
            };
            _tweenAnimator = target.GetTMPTweenAnimator();
        }

        protected override void OnReset()
        {
            if(_sequence.IsActive()) _sequence.Kill();
        }

        protected override void OnPlay(CancellationToken token)
        {
            _sequence = Sequence.Create();
            for (int i = 0; i < _tweenAnimator.GetCharCount(); i++)
            {
                var i1 = i;
                var tween= _tweenAnimator.TweenCharEulerAngles(i, zero, one, duration)
                    .SetDelay(waveCurve.Evaluate(i1 / (float) _tweenAnimator.GetCharCount()) * waveDuration)
                    .SetIgnoreTimeScale(ignoreTimeScale)
                    .OnKill(()=>_onInitializeCache(i1))
                    .OnComplete(()=>_onInitializeCache(i1));
                if (mode == EaseMode.Ease) 
                    tween.SetEase(ease);
                else 
                    tween.SetEase(curve);
                _sequence.Join(tween);
            }
        }
        protected override void OnStop()
        {
            target.ResetCharTweens();
        }
    }
}