#if UNITY_EDITOR
using System;
using Object = UnityEngine.Object;
using UnityEditor;


namespace YIUIFramework
{
    /// <summary> Unity提示框 </summary>
    public static class UnityTipsHelper
    {
        /// <summary>展示提示 </summary>
        public static void Show(string content)
        {
            EditorUtility.DisplayDialog("提示", content, "确认");
        }

        /// <summary> 提示同时error 报错 </summary>
        public static void ShowError(string message)
        {
            Show(message);
            Logger.Error(message);
        }

        /// <summary> 提示 同时error 报错 </summary>
        public static void ShowErrorContext(Object context, string message)
        {
            Show(message);
            Logger.ErrorContext(context, message);
        }

        /// <summary> 确定 取消 回调的提示框 </summary>
        public static void CallBack(string content, Action okCallBack, Action cancelCallBack = null)
        {
            var selectIndex = EditorUtility.DisplayDialogComplex("提示", content, "确认", "取消", null);
            if (selectIndex == 0) //确定
            {
                try
                {
                    okCallBack?.Invoke();
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                    throw;
                }
            }
            else
            {
                try
                {
                    cancelCallBack?.Invoke();
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                    throw;
                }
            }
        }

        /// <summary> 只有确定的提示框 </summary>
        public static void CallBackOk(string content, Action okCallBack, Action cancelCallBack = null)
        {
            var result = EditorUtility.DisplayDialog("提示", content, "确认");
            if (result) //确定
            {
                try
                {
                    okCallBack?.Invoke();
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                    throw;
                }
            }
            else
            {
                try
                {
                    cancelCallBack?.Invoke();
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                    throw;
                }
            }
        }
    }
}
#endif // UNITY_EDITOR