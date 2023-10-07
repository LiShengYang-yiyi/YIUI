using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    [FriendOf(typeof (GMCommandComponent))]
    public static class GMCommandComponentSystem
    {
        [ObjectSystem]
        public class GMCommandComponentAwakeSystem: AwakeSystem<GMCommandComponent>
        {
            protected override void Awake(GMCommandComponent self)
            {
                self.GMTypeName     = new Dictionary<string, string>();
                self.AllCommandInfo = new Dictionary<EGMType, List<GMCommandInfo>>();
                self.InitGMType();
                self.Init();
                YIUIMgrComponent.Inst.OpenPanelAsync<GMPanelComponent>().Coroutine();
            }
        }

        [ObjectSystem]
        public class GMCommandComponentDestroySystem: DestroySystem<GMCommandComponent>
        {
            protected override void Destroy(GMCommandComponent self)
            {
            }
        }

        private static void InitGMType(this GMCommandComponent self)
        {
            Type        enumType = typeof (EGMType);
            FieldInfo[] fields   = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (FieldInfo field in fields)
            {
                LabelTextAttribute attribute = (LabelTextAttribute)Attribute.GetCustomAttribute(field, typeof (LabelTextAttribute));
                if (attribute != null)
                {
                    string labelText = attribute.Text;
                    self.GMTypeName.Add(field.Name, labelText);
                }
            }
        }

        private static void Init(this GMCommandComponent self)
        {
            var types = EventSystem.Instance.GetTypes(typeof (GMAttribute));
            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces();
                if (interfaces.Length <= 0)
                {
                    Debug.LogError($"错误 没有继承接口 {type.Name}");
                    continue;
                }

                var  haveGMInterfaces = false;
                Type gmInterfaces     = null;
                foreach (var interfacesType in interfaces)
                {
                    if (interfacesType.Name.Contains("IGMCommand"))
                    {
                        if (gmInterfaces != null)
                        {
                            Debug.LogError($"错误 继承多个IGMCommand接口 {type.Name}");
                            haveGMInterfaces = false;
                            break;
                        }
                        else
                        {
                            gmInterfaces     = interfacesType;
                            haveGMInterfaces = true;
                        }
                    }
                }

                if (!haveGMInterfaces)
                {
                    Debug.LogError($"错误 没有继承IGMCommand接口 {type.Name}");
                    continue;
                }

                var gmCommandInfo = new GMCommandInfo();

                object[] attrs = type.GetCustomAttributes(typeof (GMAttribute), false);
                if (attrs.Length >= 2)
                {
                    Debug.LogError($"有多个相同特性 只允许有一个 GMAttribute");
                }

                foreach (object attr in attrs)
                {
                    var eventAttribut = (GMAttribute)attr;
                    var obj           = (IGMCommand)Activator.CreateInstance(type);
                    gmCommandInfo.GMType = eventAttribut.GMType;
                    gmCommandInfo.GMTypeName = self.GMTypeName.TryGetValue(eventAttribut.GMType.ToString(), out var name)
                            ? name : eventAttribut.GMType.ToString();
                    gmCommandInfo.GMLevel       = eventAttribut.GMLevel;
                    gmCommandInfo.GMName        = eventAttribut.GMName;
                    gmCommandInfo.GMDesc        = eventAttribut.GMDesc;
                    gmCommandInfo.Command       = obj;
                    gmCommandInfo.ParamInfoList = obj.GetParams();
                }

                self.AddInfo(gmCommandInfo);
            }
        }

        private static void AddInfo(this GMCommandComponent self, GMCommandInfo info)
        {
            if (!self.AllCommandInfo.ContainsKey(info.GMType))
            {
                self.AllCommandInfo.Add(info.GMType, new List<GMCommandInfo>());
            }

            var listInfo = self.AllCommandInfo[info.GMType];

            listInfo.Add(info);
        }

        public static async ETTask Run(this GMCommandComponent self, GMCommandInfo info)
        {
            /*
             * TODO 判断inof中的 GM命令需求等级
             * 取自身数据中的等级  判断是否符合要求
             */

            var paramList = info.ParamInfoList;
            var objData   = new List<object>();
            for (int i = 0; i < paramList.Count; i++)
            {
                var paramInfo = paramList[i];
                var objValue  = paramInfo.ParamType.TryToValue(paramInfo.Value);
                objData.Add(objValue);
            }

            var banClickCode = YIUIMgrComponent.Inst.BanLayerOptionForever();
            var paramVo      = ParamVo.Get(objData);
            var closeGM      = await info.Command.Run(self.ClientScene(), paramVo);
            ParamVo.Put(paramVo);
            if (closeGM)
                await YIUIEventSystem.Event(new OnGMEventClose());
            YIUIMgrComponent.Inst.RecoverLayerOptionForever(banClickCode);
        }
    }
}