
namespace ET
{
    //扩展案例
    //注意按照ET的 3个模式规则 如果这个扩展在其他地方也需要 那么是要同步的
    //也就是说可能有多份
    public sealed partial class UnitConfig
    {
        //案例字段 使用get set 属性
        public int TestInt { get; private set; }

        //使用方法初始化 更多使用规范查看文档
        public void InitTest(int testInt)
        {
            TestInt = testInt;
        }
    }
}
