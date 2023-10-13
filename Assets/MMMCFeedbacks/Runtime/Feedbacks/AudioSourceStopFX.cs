using System;
using System.Threading;
using MagicTween;
using UnityEngine;

namespace MMMCFeedbacks.Core
{
    [Serializable]
    public class AudioSourceStopFX : Feedback
    {
        public override int Order => 11;
        public override string MenuString => "AudioSource/Stop (AudioSource)";
        public override Color TagColor => FeedbackStyling.AudioFXColor;
        public override Tween Tween => Tween.Empty(0);

        [Space(10)] 
        [SerializeField] private AudioSource target;

        protected override void OnPlay(CancellationToken token)
        {
            target.Stop(); 
        }
    }
}