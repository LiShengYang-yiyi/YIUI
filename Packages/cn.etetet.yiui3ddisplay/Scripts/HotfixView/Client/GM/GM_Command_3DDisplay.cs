using System.Collections.Generic;
using YIUIFramework;

namespace ET.Client
{
    //主要用于在GM包上测试功能
    //当前包没有强制引用GM包
    //如果没有引用GM包  请删除这个文件
    [GM(EGMType.Display3D, 1, "3D显示测试")]
    public class GM_DisplayTest1 : IGMCommand
    {
        public List<GMParamInfo> GetParams()
        {
            return new();
        }

        public async ETTask<bool> Run(Scene clientScene, ParamVo paramVo)
        {
            TipsHelper.OpenSync<ModelDisplayDemoViewComponent>(clientScene);
            await ETTask.CompletedTask;
            return true;
        }
    }
}