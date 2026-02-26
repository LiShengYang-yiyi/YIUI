using System;
using System.Collections.Generic;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// 文档: https://lib9kmxvq7k.feishu.cn/wiki/XzyawmryHitNVNk9QVtcDAftn5O
    /// </summary>
    //主要用于在GM包上测试功能
    //当前包没有强制引用GM包
    //如果没有引用GM包  请删除这个文件
    [GM(EGMType.RedDot, 1, "打开红点调试界面")]
    public class GM_OpenRedDotPanel: IGMCommand
    {
        public List<GMParamInfo> GetParams()
        {
            return new();
        }

        public async ETTask<bool> Run(Scene clientScene, ParamVo paramVo)
        {
            await clientScene.YIUIRoot().OpenPanelAsync<RedDotPanelComponent>();
            return true;
        }
    }
}