using Luban;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ET
{
    /// <summary>
    /// ConfigLoader会扫描所有的有ConfigAttribute标签的配置,加载进来
    /// </summary>
    public class ConfigLoader : Singleton<ConfigLoader>, ISingletonAwake
    {
        public struct GetAllConfigBytes
        {
        }

        public struct GetOneConfigBytes
        {
            public string ConfigName;
        }

        private readonly ConcurrentDictionary<Type, IConfig> allConfig = new();

        public void Awake()
        {
        }

        public async ETTask Reload(Type configType)
        {
            GetOneConfigBytes singleConfig = new() { ConfigName = configType.Name };
            ByteBuf oneConfigBytes = await EventSystem.Instance.Invoke<GetOneConfigBytes, ETTask<ByteBuf>>(singleConfig);

            object category = Activator.CreateInstance(configType, oneConfigBytes);
            this.allConfig[configType] = category as IConfig;
            this.allConfig[configType].ResolveRef();
            World.Instance.AddSingleton(category as ASingleton);
        }

        public async ETTask LoadAsync()
        {
            this.allConfig.Clear();
            Dictionary<Type, ByteBuf> configBytes = await EventSystem.Instance.Invoke<GetAllConfigBytes, ETTask<Dictionary<Type, ByteBuf>>>(new GetAllConfigBytes());

            using ListComponent<Task> listTasks = ListComponent<Task>.Create();

            foreach (Type type in configBytes.Keys)
            {
                ByteBuf oneConfigBytes = configBytes[type];
                Task task = Task.Run(() => LoadOneInThread(type, oneConfigBytes));
                listTasks.Add(task);
            }

            await Task.WhenAll(listTasks.ToArray());

            foreach (var targetConfig in this.allConfig.Values)
            {
                targetConfig.ResolveRef();
            }
        }

        private void LoadOneInThread(Type configType, ByteBuf oneConfigBytes)
        {
            object category = Activator.CreateInstance(configType, oneConfigBytes);

            lock (this)
            {
                this.allConfig[configType] = category as IConfig;
                World.Instance.AddSingleton(category as ASingleton);
            }
        }
    }
}