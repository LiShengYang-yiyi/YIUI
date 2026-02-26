using System.Collections.Generic;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    //1 所在页签
    //2 命令等级 当执行人等级不足时无法使用
    //3 命令名称 页面上显示的名字
    //4 命令描述 有描述时显示描述 没有时不显示
    [GM(EGMType.Test, 1, "测试案列", "测试案列描述.....")]
    public class GM_Test : IGMCommand
    {
        public List<GMParamInfo> GetParams()
        {
            return new()
            {
                //目前有6种参数类型可选 长度无限
                //根据需求这里设定对应参数类型与描述 将在页面上显示
                new GMParamInfo(EGMParamType.Enum, "参数0 枚举", "Bool", "ET.Client.EGMParamType"), //枚举案例 一定要写枚举全名
                new GMParamInfo(EGMParamType.String, "参数1 字符串", "字符串"),
                new GMParamInfo(EGMParamType.Bool, "参数2 布尔", "true"),
                new GMParamInfo(EGMParamType.Float, "参数3 小数", "0.0125"),
                new GMParamInfo(EGMParamType.Int, "参数4 整数", "123"),
                new GMParamInfo(EGMParamType.Long, "参数5 64整数", "456"),
            };
        }

        //这里等待时间是等待执行
        //返回true代表关闭GM窗口 否则不会关闭GM窗口
        //返回的时机决定了GM窗口的关闭时间 立即返回就立即关闭 延迟返回就延迟关闭
        //根据需求合理的返回时机可以提升GM的体验
        public async ETTask<bool> Run(Scene clientScene, ParamVo paramVo)
        {
            //通过paramvo 可以取出所有参数
            var paramEnum   = paramVo.Get<EGMParamType>(0);
            var paramString = paramVo.Get<string>(1);
            var paramBool   = paramVo.Get<bool>(2);
            var paramFloat  = paramVo.Get<float>(3);
            var paramInt    = paramVo.Get<int>(4);
            var paramLong   = paramVo.Get<long>(5);

            //最后根据参数自行处理命令
            Debug.LogError($"参数0: {paramEnum}");
            Debug.LogError($"参数1: {paramString}");
            Debug.LogError($"参数2: {paramBool}");
            Debug.LogError($"参数3: {paramFloat}");
            Debug.LogError($"参数4: {paramInt}");
            Debug.LogError($"参数5: {paramLong}");

            await ETTask.CompletedTask;
            return true;
        }
    }
}