﻿using System;
using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using MagicTween;
using MMMCFeedbacks.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MMMCFeedbacks.Core
{
    [Serializable] public class CinemachineImpulseFX : Feedback
    {
        public override int Order => 10;
        public override string MenuString => "Camera/Cinemachine Impulse";
        public override Color TagColor => FeedbackStyling.CameraFXColor;
        public override Tween Tween => Tween.Empty(0);

        [Header("Cinemachine")] [SerializeField]
        private ImpulseMode mode;
        [Space(5)]
        [SerializeField] [DisplayIf(nameof(mode),(int)ImpulseMode.ImpulseSource)]
        private CinemachineImpulseSource impulseSource;
        [SerializeField][DisplayIf(nameof(mode),(int)ImpulseMode.CollisionImpulseSource)]
        private CinemachineCollisionImpulseSource impulseCollisionSource;
        [SerializeField][DisplayIf(nameof(mode),(int)ImpulseMode.ImpulseDefinition)]
        private CinemachineImpulseDefinition impulseDefinition = new ();
        
        protected override void OnPlay(CancellationToken token)
        {
            var camera = Camera.main;
            CinemachineImpulseManager.Instance.IgnoreTimeScale = ignoreTimeScale;
            switch(mode)
            {
                case ImpulseMode.ImpulseSource:
                    impulseSource.GenerateImpulse();
                    break;
                case ImpulseMode.CollisionImpulseSource:
                    impulseCollisionSource.GenerateImpulse();
                    break;
                case ImpulseMode.ImpulseDefinition:
                    impulseDefinition.CreateEvent(camera.transform.position, Random.onUnitSphere);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            };
        }
        protected override void OnStop()
        {
            CinemachineImpulseManager.Instance.Clear();
        }

        private enum ImpulseMode
        {
            ImpulseSource,
            CollisionImpulseSource,
            ImpulseDefinition
        }
    }
}