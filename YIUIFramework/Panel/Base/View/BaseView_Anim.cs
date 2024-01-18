using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace YIUIFramework
{
    public partial class BaseView
    {
        protected sealed override async UniTask SealedOnWindowOpenTween()
        {
            if (PanelMgr.IsLowQuality || WindowBanTween)
            {
                return;
            }

            var foreverCode = WindowAllowOptionByTween ? 0 : m_PanelMgr.BanLayerOptionForever();
            try
            {
                await OnOpenTween();
            }
            catch (Exception e)
            {
                Debug.LogError($"{UIResName} 打开动画执行报错 {e}");
            }
            finally
            {
                m_PanelMgr.RecoverLayerOptionForever(foreverCode);
                OnOpenTweenEnd();
            }
        }

        protected sealed override async UniTask SealedOnWindowCloseTween()
        {
            if (!ActiveSelf || PanelMgr.IsLowQuality || WindowBanTween)
            {
                return;
            }

            var foreverCode = WindowAllowOptionByTween ? 0 : m_PanelMgr.BanLayerOptionForever();
            try
            {
                await OnCloseTween();
            }
            catch (Exception e)
            {
                Debug.LogError($"{UIResName} 关闭动画执行报错 {e}");
            }
            finally
            {
                m_PanelMgr.RecoverLayerOptionForever(foreverCode);
                OnCloseTweenEnd();
            }
        }

        protected override async UniTask OnOpenTween()
        {
            await UniTask.CompletedTask;
        }

        protected override async UniTask OnCloseTween()
        {
            await UniTask.CompletedTask;
        }

        protected override void OnOpenTweenStart()
        {
            OwnerGameObject.SetActive(true);
        }

        protected override void OnOpenTweenEnd()
        {
        }

        protected override void OnCloseTweenStart()
        {
        }

        protected override void OnCloseTweenEnd()
        {
            OwnerGameObject.SetActive(false);
        }
    }
}