//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof (YIUIEventComponent))]
    public static partial class YIUIEventComponentSystem
    {
        [ObjectSystem]
        public class YIUIEventComponentAwakeSystem: AwakeSystem<YIUIEventComponent>
        {
            protected override void Awake(YIUIEventComponent self)
            {
                YIUIEventComponent.Inst = self;
                self.Init();
            }
        }

        [ObjectSystem]
        public class YIUIEventComponentDestroySystem: DestroySystem<YIUIEventComponent>
        {
            protected override void Destroy(YIUIEventComponent self)
            {
            }
        }

        private static void Init(this YIUIEventComponent self)
        {
            self._AllEventInfo = new();

            var types = EventSystem.Instance.GetTypes(typeof (YIUIEventAttribut));
            foreach (var type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof (YIUIEventAttribut), false);
                foreach (object attr in attrs)
                {
                    var eventAttribut = (YIUIEventAttribut)attr;
                    var obj           = (IYIUICommonEvent)Activator.CreateInstance(type);
                    var eventType     = eventAttribut.EventType;
                    var componentName = eventAttribut.ComponentType.Name;
                    var info          = new YIUIEventInfo(eventType, componentName, obj);

                    if (!self._AllEventInfo.ContainsKey(eventAttribut.EventType))
                    {
                        self._AllEventInfo.Add(eventAttribut.EventType, new Dictionary<string, List<YIUIEventInfo>>());
                    }

                    if (!self._AllEventInfo[eventAttribut.EventType].ContainsKey(componentName))
                    {
                        self._AllEventInfo[eventAttribut.EventType].Add(componentName, new List<YIUIEventInfo>());
                    }

                    var infoList = self._AllEventInfo[eventAttribut.EventType][componentName];

                    infoList.Add(info);
                }
            }
        }

        public static async ETTask Run<T>(this YIUIEventComponent self, string componentName, T data)
        {
            var eventType = typeof (T);
            if (!self._AllEventInfo.TryGetValue(eventType, out var componentDic))
            {
                return;
            }

            if (!componentDic.TryGetValue(componentName, out var eventInfos))
            {
                return;
            }

            foreach (var info in eventInfos)
            {
                await info.UIEvent.Run(self.DomainScene(), data);
            }
        }
    }
}