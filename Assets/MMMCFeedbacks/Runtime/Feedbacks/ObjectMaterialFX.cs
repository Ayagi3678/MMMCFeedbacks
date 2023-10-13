using System;
using System.Threading;
using MagicTween;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

namespace MMMCFeedbacks.Core
{
    [Serializable]
    public class ObjectMaterialFX : Feedback
    {
        public override int Order => 5;
        public override string MenuString => "Object/Material (Object)";
        public override Color TagColor => FeedbackStyling.ObjectFXColor; 
        public override Tween Tween => _tween;
        [Space(10)]
        [SerializeField] private Material target;    
        [Header("Material")]
        [SerializeField] private string propertyName;
        [SerializeField] private ParameterType propertyType;
        [Space(10)]
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Ease)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Curve)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField,DisplayIf(nameof(propertyType),0)] private float floatZero = 1;
        [SerializeField,DisplayIf(nameof(propertyType),0)] private float floatOne;
        [SerializeField,DisplayIf(nameof(propertyType),1)] private int intZero = 1;
        [SerializeField,DisplayIf(nameof(propertyType),1)] private int intOne;
        [SerializeField,DisplayIf(nameof(propertyType),2)] private Color colorZero = Color.white;
        [SerializeField,DisplayIf(nameof(propertyType),2)] private Color colorOne;
        [SerializeField,DisplayIf(nameof(propertyType),3)] private Vector3 vectorZero = Vector3.one;
        [SerializeField,DisplayIf(nameof(propertyType),3)] private Vector3 vectorOne;
        [SerializeField,DisplayIf(nameof(propertyType),4)] private Vector2 offsetZero = Vector2.one;
        [SerializeField,DisplayIf(nameof(propertyType),4)] private Vector2 offsetOne;
        [SerializeField,DisplayIf(nameof(propertyType),5)] private Vector2 scaleZero = Vector2.one;
        [SerializeField,DisplayIf(nameof(propertyType),5)] private Vector2 scaleOne;
        [SerializeField]private float duration=1;
        
        private Tween _tween;

        protected override void OnReset()
        {
            if (_tween.IsActive()) _tween.Kill();
        }

        protected override void OnPlay(CancellationToken token)
        {
            _tween = propertyType switch
            {
                ParameterType.Float => target.TweenFloat(propertyName, floatZero, floatOne, duration),
                ParameterType.Int => target.TweenInt(propertyName, intZero, intOne, duration),
                ParameterType.Color => target.TweenColor(propertyName, colorZero, colorOne, duration),
                ParameterType.Vector => target.TweenVector(propertyName, vectorZero, vectorOne, duration),
                ParameterType.TextureOffset => target.TweenTextureOffset(propertyName, offsetZero, offsetOne, duration),
                ParameterType.TextureScale => target.TweenTextureScale(propertyName, scaleZero, scaleOne, duration),
                _ => throw new ArgumentOutOfRangeException()
            };
            _tween.SetIgnoreTimeScale(ignoreTimeScale);
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