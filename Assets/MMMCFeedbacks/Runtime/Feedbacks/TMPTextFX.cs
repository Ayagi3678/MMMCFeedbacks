using System;
using System.Threading;
using MagicTween;
using TMPro;
using UnityEngine;

namespace MMMCFeedbacks.Core
{
    [Serializable]
    public class TMPTextFX : Feedback
    {
        public override int Order => 5;
        public override string MenuString => "UI/TMP/Text (TMP)";
        public override Color TagColor => FeedbackStyling.UIFXColor;
        public override Tween Tween => _tween;
        [Space(10)]
        [SerializeField] private TMP_Text target;
        [SerializeField] private bool resetToInitial;
        [Header("Text")] [SerializeField]
        private ScrambleMode scrambleMode;
        [SerializeField,DisplayIf(nameof(scrambleMode),5)] private string customText;
        
        [SerializeField] private bool richTextEnabled;

        [Space(10)]
        
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Ease)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Curve)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField,TextArea] private string zero;
        [SerializeField,TextArea] private string one;
        [SerializeField] private float duration=1;
        private string _initialText;
        private Action _onInitialCache;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onInitialCache = () => { if (resetToInitial) target.text=_initialText; };
        }
        protected override void OnReset()
        {
            if (_tween.IsActive()) _tween.Kill();
        }

        protected override void OnPlay(CancellationToken token)
        {
            _initialText = target.text;
            if (scrambleMode != ScrambleMode.Custom)
            {
                _tween = target.TweenText(zero, one, duration)
                    .SetScrambleMode(scrambleMode)
                    .SetRichTextEnabled(richTextEnabled);
            }
            else
            {
                _tween = target.TweenText(zero, one, duration)
                    .SetScrambleMode(customText)
                    .SetRichTextEnabled(richTextEnabled);
            }
            _tween.SetIgnoreTimeScale(ignoreTimeScale)
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