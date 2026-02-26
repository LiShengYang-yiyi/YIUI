namespace ET.Client
{
    //GM命令分类
    //将会根据枚举生成页签
    //GMGroup特性标记转换为显示名称
    //UniqueId特性标记唯一ID
    //ID会从小到大排序
    [UniqueId]
    public static partial class EGMType
    {
        [GMGroup("通用")]
        public const int Common = PackageType.YIUI * 1000 + 1;

        [GMGroup("测试")]
        public const int Test = PackageType.YIUI * 1000 + 2;
    }
}