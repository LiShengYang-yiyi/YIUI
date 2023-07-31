using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// OpenView 由BasePanel调用
    /// </summary>
    public partial class BaseView
    {
        #region Open 由BasePanel调用

        /// <summary>
        /// 使用基础Open 打开类
        /// </summary>
        /// <returns></returns>
        private async UniTask<bool> UseBaseOpen()
        {
            if (!WindowCanUseBaseOpen)
            {
                Debug.LogError($"当前传入的参数不支持 并未实现这个打开方式 且不允许使用基础Open打开 请检查");
                return false;
            }

            return await Open();
        }

        public async UniTask<bool> Open()
        {
            SetActive(true);

            var success = false;

            if (!WindowHaveIOpenAllowOpen && this is IYIUIOpen)
            {
                Debug.LogError($"当前View 有其他IOpen 接口 需要参数传入 不允许直接调用Open");
                return false;
            }

            try
            {
                success = await OnOpen();
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={UIResName}, err={e.Message}{e.StackTrace}");
            }

            if (success)
            {
                await InternalOnWindowOpenTween();
            }

            return success;
        }

        public async UniTask<bool> Open(ParamVo param)
        {
            if (WindowBanParamOpen)
            {
                Debug.LogError($"当前禁止使用ParamOpen 请检查");
                return false;
            }

            SetActive(true);

            var success = false;

            try
            {
                success = await OnOpen(param);
            }
            catch (Exception e)
            {
                Debug.LogError($"ResName={UIResName}, err={e.Message}{e.StackTrace}");
            }

            if (success)
            {
                await InternalOnWindowOpenTween();
            }

            return success;
        }

        public async UniTask<bool> Open<P1>(P1 p1)
        {
            SetActive(true);

            var success = false;

            if (this is IYIUIOpen<P1> view)
            {
                try
                {
                    success = await view.OnOpen(p1);
                }
                catch (Exception e)
                {
                    Debug.LogError($"ResName{UIResName}, err={e.Message}{e.StackTrace}");
                }
            }
            else
            {
                return await UseBaseOpen();
            }

            if (success)
            {
                await InternalOnWindowOpenTween();
            }

            return success;
        }

        public async UniTask<bool> Open<P1, P2>(P1 p1, P2 p2)
        {
            SetActive(true);

            var success = false;

            if (this is IYIUIOpen<P1, P2> view)
            {
                try
                {
                    success = await view.OnOpen(p1, p2);
                }
                catch (Exception e)
                {
                    Debug.LogError($"ResName{UIResName}, err={e.Message}{e.StackTrace}");
                }
            }
            else
            {
                return await UseBaseOpen();
            }

            if (success)
            {
                await InternalOnWindowOpenTween();
            }

            return success;
        }

        public async UniTask<bool> Open<P1, P2, P3>(P1 p1, P2 p2, P3 p3)
        {
            SetActive(true);

            var success = false;

            if (this is IYIUIOpen<P1, P2, P3> view)
            {
                try
                {
                    success = await view.OnOpen(p1, p2, p3);
                }
                catch (Exception e)
                {
                    Debug.LogError($"ResName{UIResName}, err={e.Message}{e.StackTrace}");
                }
            }
            else
            {
                return await UseBaseOpen();
            }

            if (success)
            {
                await InternalOnWindowOpenTween();
            }

            return success;
        }

        public async UniTask<bool> Open<P1, P2, P3, P4>(P1 p1, P2 p2, P3 p3, P4 p4)
        {
            SetActive(true);

            var success = false;

            if (this is IYIUIOpen<P1, P2, P3, P4> view)
            {
                try
                {
                    success = await view.OnOpen(p1, p2, p3, p4);
                }
                catch (Exception e)
                {
                    Debug.LogError($"ResName{UIResName}, err={e.Message}{e.StackTrace}");
                }
            }
            else
            {
                return await UseBaseOpen();
            }

            if (success)
            {
                await InternalOnWindowOpenTween();
            }

            return success;
        }

        public async UniTask<bool> Open<P1, P2, P3, P4, P5>(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
        {
            SetActive(true);

            var success = false;

            if (this is IYIUIOpen<P1, P2, P3, P4, P5> view)
            {
                try
                {
                    success = await view.OnOpen(p1, p2, p3, p4, p5);
                }
                catch (Exception e)
                {
                    Debug.LogError($"ResName{UIResName}, err={e.Message}{e.StackTrace}");
                }
            }
            else
            {
                return await UseBaseOpen();
            }

            if (success)
            {
                await InternalOnWindowOpenTween();
            }

            return success;
        }

        #endregion
    }
}