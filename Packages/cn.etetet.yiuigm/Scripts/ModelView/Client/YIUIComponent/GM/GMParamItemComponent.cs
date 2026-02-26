using System.Collections.Generic;
using UnityEngine.UI;
using YIUIFramework;

namespace ET.Client
{
    public partial class GMParamItemComponent : Entity
    {
        public GMParamInfo ParamInfo;
        public List<Dropdown.OptionData> OptionList;
        public Dictionary<string, string> OptionDic;
    }
}