using System;
using System.Threading;
using MagicTween;
using UnityEngine;
using UnityEngine.UI;

namespace MMMCFeedbacks.Core
{
    [Serializable]
    public class AudioSourcePlayOneShotFX : Feedback
    {
        public override int Order => 11;
        public override string MenuString => "AudioSource/Play One Shot (AudioSource)";
        public override Color TagColor => FeedbackStyling.AudioFXColor;
        public override Tween Tween => Tween.Empty(0);

        [Space(10)] 
        [SerializeField] private AudioSource target;
        [SerializeField] private AudioClip clip;
        [SerializeField] private float volumeScale = 1;
        protected override void OnPlay(CancellationToken token)
        {
            target.PlayOneShot(clip,volumeScale);
        }
    }
}