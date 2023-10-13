using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MagicTween;
using MMMCFeedbacks.Core;
using UnityEngine;
using UnityEngine.Events;

namespace MMMCFeedbacks.Core
{
    [Serializable] public class EventFX : Feedback
    {
        public override int Order => -5;
        public override string MenuString => "etc/Event";
        public override Color TagColor => FeedbackStyling.EtcFXColor;
        public override Tween Tween => Tween.Empty(0);
        [Space(10)]
        [SerializeField] private UnityEvent @event;

        protected override void OnPlay(CancellationToken token)
        {
            @event.Invoke();
        }
    }
}