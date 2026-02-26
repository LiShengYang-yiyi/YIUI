using System.Collections.Generic;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    //主要用于在GM包上测试功能
    //当前包没有强制引用GM包
    //如果没有引用GM包  请删除这个文件
    [GM(EGMType.Tips, 1, "弹窗测试-消息弹窗")]
    public class GM_TipsTest1 : IGMCommand
    {
        public List<GMParamInfo> GetParams()
        {
            return new()
            {
                new GMParamInfo(EGMParamType.String, "消息内容", "测试消息内容"),
            };
        }

        public async ETTask<bool> Run(Scene clientScene, ParamVo paramVo)
        {
            var paramString = paramVo.Get<string>();
            TipsHelper.OpenSync<TipsMessageViewComponent>(clientScene, paramString);
            await ETTask.CompletedTask;
            return true;
        }
    }

    [GM(EGMType.Tips, 1, "弹窗测试-消息回调弹窗")]
    public class GM_TipsTest2 : IGMCommand
    {
        public List<GMParamInfo> GetParams()
        {
            return new();
        }

        public async ETTask<bool> Run(Scene clientScene, ParamVo paramVo)
        {
            TipsHelper.Open<TipsMessageViewComponent>(clientScene, "回调测试").NoContext();
            await ETTask.CompletedTask;
            return true;
        }
    }

    [GM(EGMType.Tips, 1, "弹窗测试-消息回调弹窗 只有确定")]
    public class GM_TipsTest3 : IGMCommand
    {
        public List<GMParamInfo> GetParams()
        {
            return new();
        }

        public async ETTask<bool> Run(Scene clientScene, ParamVo paramVo)
        {
            TipsHelper.Open<TipsMessageViewComponent>(clientScene, "只有确定 回调测试").NoContext();
            await ETTask.CompletedTask;
            return true;
        }
    }

    [GM(EGMType.Tips, 1, "弹窗测试-消息回调弹窗 修改按钮名称")]
    public class GM_TipsTest4 : IGMCommand
    {
        public List<GMParamInfo> GetParams()
        {
            return new();
        }

        public async ETTask<bool> Run(Scene clientScene, ParamVo paramVo)
        {
            TipsHelper.Open<TipsMessageViewComponent>(clientScene, "回调测试 修改按钮名称", new MessageTipsExtraData()
            {
                ConfirmName = "Confirm",
                CancelName = "Cancel"
            }).NoContext();
            await ETTask.CompletedTask;
            return true;
        }
    }

    [GM(EGMType.Tips, 1, "弹窗测试-飘字消息")]
    public class GM_TipsTest5 : IGMCommand
    {
        public List<GMParamInfo> GetParams()
        {
            return new()
            {
                new GMParamInfo(EGMParamType.String, "消息内容", "飘字消息测试内容"),
            };
        }

        public async ETTask<bool> Run(Scene clientScene, ParamVo paramVo)
        {
            var paramString = paramVo.Get<string>();
            TipsHelper.OpenSync<TipsTextViewComponent>(clientScene, paramString);
            await ETTask.CompletedTask;
            return true;
        }
    }
}