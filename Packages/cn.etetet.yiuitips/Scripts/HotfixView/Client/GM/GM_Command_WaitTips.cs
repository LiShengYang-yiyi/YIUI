using System.Collections.Generic;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    [GM(EGMType.Tips, 0, "等待窗口测试 不关闭会一直等待逻辑 Success")]
    public class GM_TipsTest0 : IGMCommand
    {
        public List<GMParamInfo> GetParams()
        {
            return new();
        }

        public async ETTask<bool> Run(Scene clientScene, ParamVo paramVo)
        {
            Test(clientScene).NoContext();
            await ETTask.CompletedTask;
            return true;
        }

        private async ETTask Test(Scene clientScene)
        {
            Log.Error($"打开 等待弹窗测试");
            var result = await TipsHelper.OpenWait<TipsMessageViewComponent>(clientScene, "回调测试");
            Log.Error($"等待弹窗测试等待完毕 继续执行: {result}");
        }
    }

    [GM(EGMType.Tips, 0, "等待窗口测试 取消测试 Cancel")]
    public class GM_TipsTest0_1 : IGMCommand
    {
        public List<GMParamInfo> GetParams()
        {
            return new()
            {
                new GMParamInfo(EGMParamType.Long, "取消时间", "2000"),
            };
        }

        public async ETTask<bool> Run(Scene clientScene, ParamVo paramVo)
        {
            var cancelTime = paramVo.Get<long>();
            var cancel = new ETCancellationToken();
            Test(clientScene).WithContext(cancel);
            WaitCancel().WithContext(cancel);
            await ETTask.CompletedTask;
            return true;

            async ETTask WaitCancel()
            {
                //模拟等待X秒后取消
                await clientScene.Root().GetComponent<TimerComponent>().WaitAsync(cancelTime);
                cancel.Cancel();
            }
        }

        private async ETTask Test(Scene clientScene)
        {
            Log.Error($"打开 等待弹窗测试 取消测试");
            var result = await TipsHelper.OpenWait<TipsMessageViewComponent>(clientScene, "回调测试");
            Log.Error($"等待弹窗测试等待完毕 继续执行 取消测试: {result}");
        }
    }

    [GM(EGMType.Tips, 0, "等待窗口测试 超时测试 TimeOut")]
    public class GM_TipsTest0_2 : IGMCommand
    {
        public List<GMParamInfo> GetParams()
        {
            return new()
            {
                new GMParamInfo(EGMParamType.Long, "超时时间", "2000"),
            };
        }

        public async ETTask<bool> Run(Scene clientScene, ParamVo paramVo)
        {
            var timeout = paramVo.Get<long>();
            var cancel = new ETCancellationToken();
            Test(clientScene, timeout).WithContext(cancel);
            await ETTask.CompletedTask;
            return true;
        }

        private async ETTask Test(Scene clientScene, long timeout)
        {
            Log.Error($"打开 等待弹窗测试 超时测试");
            var result = await TipsHelper.OpenWait<TipsMessageViewComponent>(clientScene, "回调测试").TimeoutAsync(timeout);
            Log.Error($"等待弹窗测试等待完毕 继续执行 超时测试: {result}");
        }
    }

    [GM(EGMType.Tips, 0, "等待窗口测试 取消超时测试 Cancel+TimeOut")]
    public class GM_TipsTest0_3 : IGMCommand
    {
        public List<GMParamInfo> GetParams()
        {
            return new()
            {
                new GMParamInfo(EGMParamType.Long, "取消时间", "2000"),
                new GMParamInfo(EGMParamType.Long, "超时时间", "3000"),
            };
        }

        public async ETTask<bool> Run(Scene clientScene, ParamVo paramVo)
        {
            var cancelTime = paramVo.Get<long>();
            var timeout = paramVo.Get<long>(1);
            var cancel = new ETCancellationToken();
            Test(clientScene, timeout).WithContext(cancel);
            WaitCancel().WithContext(cancel);
            await ETTask.CompletedTask;
            return true;

            async ETTask WaitCancel()
            {
                //模拟等待X秒后取消
                await clientScene.Root().GetComponent<TimerComponent>().WaitAsync(cancelTime);
                cancel.Cancel();
            }
        }

        private async ETTask Test(Scene clientScene, long timeout)
        {
            Log.Error($"打开 等待弹窗测试 取消超时测试");
            var result = await TipsHelper.OpenWait<TipsMessageViewComponent>(clientScene, "回调测试").TimeoutAsync(timeout);
            Log.Error($"等待弹窗测试等待完毕 继续执行 取消超时测试: {result}");
        }
    }

    [GM(EGMType.Tips, 0, "连续异步回调写法")]
    public class GM_TipsTest0_4 : IGMCommand
    {
        public List<GMParamInfo> GetParams()
        {
            return new()
            {
                new GMParamInfo(EGMParamType.String, "消息内容", "连续异步回调写法"),
            };
        }

        public async ETTask<bool> Run(Scene clientScene, ParamVo paramVo)
        {
            var paramString = paramVo.Get<string>();
            Test(clientScene, paramString).NoContext();
            await ETTask.CompletedTask;
            return true;
        }

        private async ETTask Test(Scene clientScene, string tipsContent)
        {
            Debug.LogError($"打开 等待弹窗测试");
            var result = await TipsHelper.OpenWait<TipsMessageViewComponent>(clientScene, tipsContent);
            Debug.LogError($"等待弹窗测试等待完毕 继续执行: {result}");
            if (result == EHashWaitError.Success)
            {
                Debug.LogError($"回调执行, 确定逻辑");
            }
            else
            {
                Debug.LogError($"回调执行, 取消逻辑");
            }
        }
    }
}