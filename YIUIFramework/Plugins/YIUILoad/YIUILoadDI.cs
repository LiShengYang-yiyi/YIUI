// ========== File README ==========
/*
//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------
 * Modified By: Mizudanngo
 * Modify Time: 2024-05-08
 */
// ========== File README ==========


using System;
using Object = UnityEngine.Object;
using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
  /*
      // 关联YIUI工具中自动生成绑定代码
      UIBindHelper.InternalGameGetUIBindVoFunc = YIUICodeGenerated.UIBindProvider.Get;
      // 绑定YIUI所需方法
      YIUILoadDI.LoadAssetFunc           = LoadAsset;
      YIUILoadDI.LoadAssetAsyncFunc      = LoadAssetAsync;
      YIUILoadDI.VerifyAssetValidityFunc = VerifyAssetValidityFunc;
      YIUILoadDI.ReleaseAction           = ReleaseAction;
      YIUILoadDI.ReleaseAllAction        = ReleaseAllAction;
      
      private (Object, int) LoadAsset(string pkgName, string resName, Type resType) { throw new NotImplementedException(); }
      private async UniTask<(Object, int)> LoadAssetAsync(string pkgName, string resName, Type resType) { throw new NotImplementedException(); }
      private bool VerifyAssetValidityFunc(string pkgName, string resName) { throw new NotImplementedException(); }
      private void ReleaseAction(int hashCode) { }
      private void ReleaseAllAction() { }
      
      ------------------------------------------------------------------------------------------------------------------
      
      // 关联YIUI工具中自动生成绑定代码
      UIBindHelper.InternalGameGetUIBindVoFunc = YIUICodeGenerated.UIBindProvider.Get;
      // 绑定YIUI所需方法
      YIUILoadDI.LoadAssetFunc           = YooAssetMgr.LoadAsset;
      YIUILoadDI.LoadAssetAsyncFunc      = YooAssetMgr.LoadAssetAsync;
      YIUILoadDI.VerifyAssetValidityFunc = YooAssetMgr.VerifyAssetValidityFunc;
      YIUILoadDI.ReleaseAction           = YooAssetMgr.ReleaseAction;
      YIUILoadDI.ReleaseAllAction        = YooAssetMgr.ReleaseAllAction;
   */
  
  
    /// <summary> 注入加载方法 </summary>
    public static partial class YIUILoadDI
    {
        /// <summary> 同步加载方法 <br/>
        /// 参数1: pkgName 包名 <br/>
        /// 参数2: resName 资源名 <br/>
        /// 参数3: Type 需要加载的资源类型 <br/>
        /// 返回值: obj对象与对象资源句柄ID
        /// </summary>
        public static Func<string, string, Type, (Object, int)> LoadAssetFunc { internal get; set; }
        
        /// <summary> 异步加载方法 <br/>
        /// 参数1: pkgName 包名 <br/>
        /// 参数2: resName 资源名 <br/>
        /// 参数3: Type 需要加载的资源类型 <br/>
        /// 返回值: obj对象与对象资源句柄ID
        /// </summary>
        public static Func<string, string, Type, UniTask<(Object, int)>> LoadAssetAsyncFunc { internal get; set; }
        
        /// <summary> 验证指定包里资源是否有效 <br/>
        /// 参数1: pkgName 包名 <br/>
        /// 参数2: resName 资源名 <br/>
        /// </summary>
        public static Func<string, string, bool> VerifyAssetValidityFunc { internal get; set; }

        /// <summary> 释放指定资源 <br/>
        /// 参数1：handle Id 用于释放资源的唯一句柄ID
        /// </summary>
        public static Action<int> ReleaseAction { internal get; set; }

        /// <summary> 释放所有资源 </summary>
        public static Action ReleaseAllAction { internal get; set; }
    }
}