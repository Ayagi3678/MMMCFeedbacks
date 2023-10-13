using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MagicTween;
using UnityEngine;
using Object = System.Object;

namespace MMMCFeedbacks.Core
{
    [Serializable] public class ObjectActiveFX : Feedback
    {
        public override string MenuString => "Object/Active";
        public override Color TagColor => FeedbackStyling.ObjectFXColor;
        public override Tween Tween => Tween.Empty(0);
        [Space(10)] [SerializeField] private GameObject target;
        [SerializeField] private bool active = true;
        protected override void OnPlay(CancellationToken token)
        {
            target.SetActive(active);
        }
    }
}