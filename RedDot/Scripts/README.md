### 红点系统操作方式：
### source: https://www.bilibili.com/video/BV1cz4y1s7QS?p=14

1. 先定义红点
打开yiui自动化红点工具，选中枚举页签，进行红点枚举的定义增加，
如果想添加比较多的枚举，则选择不生成的添加，否则每次生成都会重新编译代码，影响效率。
最后再选择自动生成枚举即可，生成对应的红点枚举
2. 配置红点
选中配置页签，再选新增，选择对应的红点枚举，对齐添加父级红点，即关联红点配置
如果想添加多个父级红点，即选中删改查页签，选择对应的红点配置，再新增父级红点
然后点击检查树结构是否有循环引用，再点击同步到Asset配置即可
3. 使用红点
在对应的UI上添加红点预制体`RedDotBindItem`，然后在预制体面板上选择该红点对应绑定的枚举即可，
当然也可根据需求自定义预制体和脚本，脚本根据`RedDotBind`进行修改即可
4. 代码进行红点操作
> ```csharp
> // 初始化红点
> await MgrCenter.Inst.Register(SchedulerMgr.Inst); // 依赖
> await MgrCenter.Inst.Register(RedDotMgr.Inst);
> 
> // 设置红点数量
> RedDotMgr.Inst.SetCount(Key, value);
> // 获取红点数量
> RedDotMgr.Inst.GetCount(Key, bool);
> ```
> 


> 编辑器/运行时下进行调试，绑定一个键或者按钮在代码中执行打开红点面板即可
> 在面板中可实时对红点进行调试，查看红点的状态
> ```csharp
> m_PanelMgr.OpenPanel<RedDotPanel>();
> ```

>  `UIBindProvider` 绑定信息提供器所需红点信息(运行时使用)
> ```csharp
> list[1] = new UIBindVo
> {
>   PkgName     = MizuYIUI.RedDot.RedDotPanelBase.PkgName,
>   ResName     = MizuYIUI.RedDot.RedDotPanelBase.ResName,
>   CodeType    = BasePanel,
>   BaseType    = typeof(MizuYIUI.RedDot.RedDotPanelBase),
>   CreatorType = typeof(MizuYIUI.RedDot.RedDotPanel),
> };
> list[13] = new UIBindVo
> {
>   PkgName     = MizuYIUI.RedDot.RedDotDataItemBase.PkgName,
>   ResName     = MizuYIUI.RedDot.RedDotDataItemBase.ResName,
>   CodeType    = BaseComponent,
>   BaseType    = typeof(MizuYIUI.RedDot.RedDotDataItemBase),
>   CreatorType = typeof(MizuYIUI.RedDot.RedDotDataItem),
> };
> list[14] = new UIBindVo
> {
>   PkgName     = MizuYIUI.RedDot.RedDotStackItemBase.PkgName,
>   ResName     = MizuYIUI.RedDot.RedDotStackItemBase.ResName,
>   CodeType    = BaseComponent,
>   BaseType    = typeof(MizuYIUI.RedDot.RedDotStackItemBase),
>   CreatorType = typeof(MizuYIUI.RedDot.RedDotStackItem),
> };
> ```
 