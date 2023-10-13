using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MagicTween;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MMMCFeedbacks.Core
{
    [Serializable] public class ObjectDestroyFX : Feedback
    {
        public override string MenuString => "Object/Destroy";
        public override Color TagColor => FeedbackStyling.ObjectFXColor;
        public override Tween Tween => Tween.Empty(0);
        [Space(10)] [SerializeField] private GameObject target;
        protected override void OnPlay(CancellationToken token)
        {
            Object.Destroy(target);
        }
    }
}