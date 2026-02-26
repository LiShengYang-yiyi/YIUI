//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;

namespace ET.Client
{
    /// <summary>
    /// UI事件分发
    /// </summary>
    [CodeProcess]
    public partial class YIUIEventComponent : Singleton<YIUIEventComponent>, ISingletonAwake
    {
        private readonly Dictionary<Type, Dictionary<string, List<YIUIEventInfo>>> _AllEventInfo = new();

        public void Awake()
        {
            var types = CodeTypes.Instance.GetTypes(typeof(YIUIEventAttribute));
            foreach (var type in types)
            {
                var eventAttribute = type.GetCustomAttribute<YIUIEventAttribute>(false);
                var obj            = (IYIUICommonEvent)Activator.CreateInstance(type);
                var eventType      = eventAttribute.EventType;
                var componentName  = eventAttribute.ComponentType.Name;
                var info           = new YIUIEventInfo(eventType, componentName, obj);

                if (!this._AllEventInfo.ContainsKey(eventAttribute.EventType))
                {
                    this._AllEventInfo.Add(eventAttribute.EventType, new Dictionary<string, List<YIUIEventInfo>>());
                }

                if (!this._AllEventInfo[eventAttribute.EventType].ContainsKey(componentName))
                {
                    this._AllEventInfo[eventAttribute.EventType].Add(componentName, new List<YIUIEventInfo>());
                }

                var infoList = this._AllEventInfo[eventAttribute.EventType][componentName];

                infoList.Add(info);
            }
        }

        public async ETTask Run<T>(Scene scene, string componentName, T data)
        {
            var eventType = typeof(T);
            if (!this._AllEventInfo.TryGetValue(eventType, out var componentDic))
            {
                return;
            }

            if (!componentDic.TryGetValue(componentName, out var eventInfos))
            {
                return;
            }

            EntityRef<Scene> sceneRef = scene;
            foreach (var info in eventInfos)
            {
                await info.UIEvent.Run(sceneRef, data);
            }
        }
    }
}