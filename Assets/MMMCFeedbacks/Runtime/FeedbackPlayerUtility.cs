using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MagicTween;

namespace MMMCFeedbacks.Core
{
    public static class FeedbackPlayerUtility
    {
        public static void ExecuteFeedbacks(FeedbackList list,int loopCount,CancellationToken token,ExecuteMode mode = ExecuteMode.Concurrent)
        {
            switch (mode)
            {
                case ExecuteMode.Concurrent:
                    ConcurrentExecute(list,token);
                    break;
                case ExecuteMode.Sequence:
                    SequenceExecute(list,token).Forget();
                    break;
                case ExecuteMode.Loop:
                    LoopExecute(list, loopCount,token).Forget();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
        private static void ConcurrentExecute(FeedbackList list,CancellationToken token)
        {
            foreach (var t in list.List)
            {
                if(!t.IsActive) continue;
                t.Play(token);
            }
        }
        private static async UniTaskVoid SequenceExecute(FeedbackList list,CancellationToken token)
        {
            foreach (var t in list.List)
            {
                if(!t.IsActive) continue;
                t.Play(token);
                await UniTask.WaitUntil(()=>t.Tween.IsPlaying(),cancellationToken: token);
                await t.Tween.AwaitForComplete(cancellationToken: token);
            }
        }
        private static async UniTaskVoid LoopExecute(FeedbackList list,int loopCount,CancellationToken token)
        {
            for (int i = 0; i < loopCount; i++)
            {
                foreach (var t in list.List)
                {
                    if(!t.IsActive) continue;
                    t.Play(token);
                    await t.Tween.AwaitForComplete(cancellationToken: token);
                }
            }
        }
    }
}