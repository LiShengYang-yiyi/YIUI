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
            var oneConfigBytes =
                    await EventSystem.Instance.Invoke<GetOneConfigBytes, ETTask<ByteBuf>>(new GetOneConfigBytes { ConfigName = configType.Name });
            LoadOneConfig(configType, oneConfigBytes);
            ResolveRef(); //热重载某一个配置的时候也要触发所有配置否则可能会引起各种引用丢失问题 不确定是否还有潜在问题 热重载配置还需观察
        }

        public async ETTask LoadAsync()
        {
            this.allConfig.Clear();
            var configBytes = await EventSystem.Instance.Invoke<GetAllConfigBytes, ETTask<Dictionary<Type, ByteBuf>>>(new GetAllConfigBytes());

#if DOTNET || UNITY_STANDALONE //因为低端机开多线程加载会卡死
            using ListComponent<Task> listTasks = ListComponent<Task>.Create();
            foreach (Type type in configBytes.Keys)
            {
                ByteBuf oneConfigBytes = configBytes[type];
                Task task = Task.Run(() => LoadOneInThread(type, oneConfigBytes));
                listTasks.Add(task);
            }

            await Task.WhenAll(listTasks.ToArray());
#else
            foreach (var (type,buf) in configBytes)
            {
                LoadOneConfig(type, buf);
            }
#endif

            ResolveRef();
        }

        private void LoadOneConfig(Type configType, ByteBuf oneConfigBytes)
        {
            object category = Activator.CreateInstance(configType, oneConfigBytes);
            this.allConfig[configType] = category as IConfig;
            World.Instance.AddSingleton(category as ASingleton);
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

        private void ResolveRef()
        {
            foreach (var targetConfig in this.allConfig.Values)
            {
                targetConfig.ResolveRef();
            }

            foreach (var targetConfig in this.allConfig.Values)
            {
                Initialized(targetConfig);
            }
        }

        private void Initialized(IConfig configCategory)
        {
            var iConfigSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(configCategory.GetType(), typeof(IConfigSystem));
            if (iConfigSystems == null)
            {
                return;
            }

            foreach (IConfigSystem aConfigSystem in iConfigSystems)
            {
                if (aConfigSystem == null)
                {
                    continue;
                }

                try
                {
                    aConfigSystem.Initialized(configCategory);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}