using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MagicTween;
using UnityEngine;

namespace MMMCFeedbacks.Core
{
    [Serializable] public class ParticleStopFX : Feedback
    {
        public override int Order => 4;
        public override string MenuString => "Particles/Particle Stop";
        public override Color TagColor => FeedbackStyling.ParticlesFXColor;
        public override Tween Tween => Tween.Empty(0);
        [Space(10)] [SerializeField] private ParticleSystem particle;

        protected override void OnPlay(CancellationToken token)
        {
            particle.Stop(true);
        }
        protected override void OnStop()
        {
            particle.Stop();
        }
    }
}