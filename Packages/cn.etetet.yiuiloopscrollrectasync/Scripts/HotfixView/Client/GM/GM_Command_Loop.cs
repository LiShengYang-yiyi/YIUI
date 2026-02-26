using System.Collections.Generic;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    //主要用于在GM包上测试功能
    //当前包没有强制引用GM包
    //如果没有引用GM包  请删除这个文件
    [GM(EGMType.LoopDemo, 1, "LoopDemo")]
    public class GM_LoopDemo_1 : IGMCommand
    {
        public List<GMParamInfo> GetParams()
        {
            return new()
            {
                new GMParamInfo(EGMParamType.Int, "Tab", "3"),
            };
        }

        public async ETTask<bool> Run(Scene clientScene, ParamVo paramVo)
        {
            var tab = paramVo.Get<int>();

            clientScene.YIUIRoot()
                       .OpenPanelAsync<LoopScrollRectDemoPanelComponent, ELoopScrollRectDemoPanelViewEnum>((ELoopScrollRectDemoPanelViewEnum)tab)
                       .NoContext();

            await ETTask.CompletedTask;
            return true;
        }
    }
}