namespace ET
{
    public static class UnitConfigCategorySystem
    {
        [EntitySystem]
        public class UnitConfigCategory_ConfigSystem: ConfigSystem<UnitConfigCategory>
        {
            protected override void Initialized(UnitConfigCategory self)
            {
                //全部初始化完毕后的调用 自行根据需求处理
                foreach (var data in self.DataList)
                {
                    Initialized(data);
                }
            }

            //需要初始化每个配置的字段
            private void Initialized(UnitConfig self)
            {
                //根据自定义初始化对应字段
                self.InitTest(0);
            }
        }
    }
}