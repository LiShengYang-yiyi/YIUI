using System.Collections.Generic;

namespace ET
{
    public partial class AIConfigCategory
    {
        public Dictionary<int, SortedDictionary<int, AIConfig>> AIConfigs = new();

        public SortedDictionary<int, AIConfig> GetAI(int aiConfigId)
        {
            return this.AIConfigs[aiConfigId];
        }

        partial void PostInit()
        {
            foreach (var kv in this.DataMap)
            {
                SortedDictionary<int, AIConfig> aiNodeConfig;
                if (!this.AIConfigs.TryGetValue(kv.Value.AIConfigId, out aiNodeConfig))
                {
                    aiNodeConfig = new SortedDictionary<int, AIConfig>();
                    this.AIConfigs.Add(kv.Value.AIConfigId, aiNodeConfig);
                }

                aiNodeConfig.Add(kv.Key, kv.Value);
            }
        }
    }
}