using System;
using System.Threading;
using MagicTween;
using UnityEngine;

namespace MMMCFeedbacks.Core
{
    [Serializable]
    public class AudioSourcePlayFX : Feedback
    {
        public override int Order => 11;
        public override string MenuString => "AudioSource/Play (AudioSource)";
        public override Color TagColor => FeedbackStyling.AudioFXColor;
        public override Tween Tween => Tween.Empty(0);

        [Space(10)] 
        [SerializeField] private AudioSource target;

        protected override void OnPlay(CancellationToken token)
        {
            target.Play();
        }

        protected override void OnStop()
        {
            target.Stop();
        }
    }
}