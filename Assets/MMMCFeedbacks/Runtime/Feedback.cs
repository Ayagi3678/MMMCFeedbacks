using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MagicTween;
using UnityEngine;

namespace MMMCFeedbacks.Core
{

    [Serializable]
    public abstract class Feedback
    {
        public virtual int Order => 0;
        public abstract string MenuString { get; }
        public string Label => StringConversionUtility.SplitLast(MenuString);
        public abstract Color TagColor { get; }
        public bool IsActive { get; set; } = true;
        
        public abstract Tween Tween { get; }

        [SerializeField] protected Timing timing;
        [SerializeField] protected bool ignoreTimeScale;

        private CancellationToken _token;
        public void Play(CancellationToken token)
        {
            _token = token;
            if (timing.delayTime != 0)
            {
                PlayAsync(token).Forget();
            }
            else
            {
                OnPlay(token);
            }
        }
        public void Stop()
        {
            OnStop();
        }

        public void Enable(GameObject gameObject)
        {
            OnEnable(gameObject);
        }
        public void Destroy() => OnDestroy();
        public void Reset() => OnReset();

        // ReSharper disable Unity.PerformanceAnalysis
        private async UniTaskVoid PlayAsync(CancellationToken token)
        {
            await UniTask.WaitForSeconds(timing.delayTime, ignoreTimeScale,cancellationToken : token);
            OnPlay(token);
        }
        protected virtual void OnReset(){}
        protected virtual void OnPlay(CancellationToken token){}
        protected virtual void OnStop(){}
        protected virtual void OnEnable(GameObject gameObject){}
        protected virtual void OnDestroy(){}
    }
}