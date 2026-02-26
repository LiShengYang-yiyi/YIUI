namespace ET.Client
{
    /// <summary>
    /// GM命令参数信息
    /// </summary>
    [EnableClass]
    public class GMParamInfo
    {
        public EGMParamType ParamType;    //类型
        public string       Desc;         //描述
        public string       Value;        //参数值
        public string       EnumFullName; //枚举时的全名,命名空间.名称

        public GMParamInfo(EGMParamType paramType, string desc, string value = "", string enumFullName = "")
        {
            ParamType    = paramType;
            Desc         = desc;
            Value        = value;
            EnumFullName = enumFullName;
        }
    }
}