using System;
using System.Threading;
using MagicTween;
using UnityEngine;

namespace MMMCFeedbacks.Core
{

    [Serializable] public class ParticlePlayFX : Feedback
    {
        public override int Order => 4;
        public override string MenuString => "Particles/Particle Play";
        public override Color TagColor => FeedbackStyling.ParticlesFXColor;
        public override Tween Tween => Tween.Empty(0);
        [Space(10)] [SerializeField] private ParticleSystem particle;

        protected override void OnPlay(CancellationToken token)
        {
            particle.Play(true);
        }

        protected override void OnStop()
        {
            particle.Stop();
        }
    }
}
