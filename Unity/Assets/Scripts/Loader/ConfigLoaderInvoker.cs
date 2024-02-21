using Luban;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ET
{
    [Invoke]
    public class GetAllConfigBytes : AInvokeHandler<ConfigLoader.GetAllConfigBytes, ETTask<Dictionary<Type, ByteBuf>>>
    {
        public override async ETTask<Dictionary<Type, ByteBuf>> Handle(ConfigLoader.GetAllConfigBytes args)
        {
            Dictionary<Type, ByteBuf> output = new Dictionary<Type, ByteBuf>();
            HashSet<Type> configTypes = CodeTypes.Instance.GetTypes(typeof(ConfigAttribute));

            if (Define.IsEditor)
            {
                string ct = "cs";
                GlobalConfig globalConfig = Resources.Load<GlobalConfig>("GlobalConfig");
                CodeMode codeMode = globalConfig.CodeMode;
                ct = codeMode switch
                {
                    CodeMode.Client => "c",
                    CodeMode.Server => "s",
                    CodeMode.ClientServer => "cs",
                    _ => throw new ArgumentOutOfRangeException()
                };

                List<string> startConfigs = new()
                {
                    "StartMachineConfigCategory", "StartProcessConfigCategory", "StartSceneConfigCategory", "StartZoneConfigCategory"
                };
                foreach (Type configType in configTypes)
                {
                    string configFilePath;
                    if (startConfigs.Contains(configType.Name))
                    {
                        configFilePath = $"../Config/Excel/{ct}/{Options.Instance.StartConfig}/{configType.Name}.bytes";
                    }
                    else
                    {
                        configFilePath = $"../Config/Excel/{ct}/{configType.Name}.bytes";
                    }

                    output[configType] = new ByteBuf(File.ReadAllBytes(configFilePath));
                }
            }
            else
            {
                foreach (Type type in configTypes)
                {
                    TextAsset v = await ResourcesComponent.Instance.LoadAssetAsync<TextAsset>(type.Name);
                    output[type] = new ByteBuf(v.bytes);
                }
            }

            return output;
        }
    }

    [Invoke]
    public class GetOneConfigBytes : AInvokeHandler<ConfigLoader.GetOneConfigBytes, ETTask<ByteBuf>>
    {
        public override async ETTask<ByteBuf> Handle(ConfigLoader.GetOneConfigBytes args)
        {
            string ct = "cs";
            GlobalConfig globalConfig = Resources.Load<GlobalConfig>("GlobalConfig");
            CodeMode codeMode = globalConfig.CodeMode;
            ct = codeMode switch
            {
                CodeMode.Client => "c",
                CodeMode.Server => "s",
                CodeMode.ClientServer => "cs",
                _ => throw new ArgumentOutOfRangeException()
            };

            List<string> startConfigs = new()
            {
                "StartMachineConfigCategory", "StartProcessConfigCategory", "StartSceneConfigCategory", "StartZoneConfigCategory"
            };

            string configName = args.ConfigName;

            string configFilePath;
            if (startConfigs.Contains(configName))
            {
                configFilePath = $"../Config/Excel/{ct}/{Options.Instance.StartConfig}/{configName}.bytes";
            }
            else
            {
                configFilePath = $"../Config/Excel/{ct}/{configName}.bytes";
            }

            await ETTask.CompletedTask;
            return new ByteBuf(File.ReadAllBytes(configFilePath));
        }
    }
}