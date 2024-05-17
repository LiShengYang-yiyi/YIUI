// ========== File README ==========
/*  
 * Author: YIUIFramework
 * Source: SourceLink
 * 
 * Modified By: Mizudanngo
 * Modify Time: 2024-05-17
 */
// ========== File README ==========

using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace YIUIFramework
{
  /// <summary> 日志封装 </summary>
  public static class Log
  {
    private const string ENABLE_DEBUG_LOG = "ENABLE_DEBUG_LOG";
    
    public static void Error(object message)
    {
      UnityEngine.Debug.LogError(message);
    }
    
    /// <summary> 打印错误 同时高亮指定obj资产对象 </summary>
    public static void Error(Object obj, object message)
    {
      SelectObj(obj);
      UnityEngine.Debug.LogError(message);
    }

    /// <summary> 打印错误 同时高亮上下文面板对象 </summary>
    public static void ErrorContext(Object context, object message)
    {
      UnityEngine.Debug.LogError(message, context);
    }
    
    [Conditional("UNITY_EDITOR")]
    public static void SelectObj(Object obj)
    {
      Selection.activeObject = obj;
    }

    [Conditional(ENABLE_DEBUG_LOG)]
    public static void Debug(params object[] msg)
    {
      UnityEngine.Debug.Log(msg);
    }
    
    [Conditional(ENABLE_DEBUG_LOG)]
    public static void Debug(Object context, object msg)
    {
      UnityEngine.Debug.Log(msg, context);
    }
    
    [Conditional(ENABLE_DEBUG_LOG)]
    public static void DebugRed(string format)
    {
      Debug($"[{format}]", EColor.Red);
    }
    
    [Conditional(ENABLE_DEBUG_LOG)]
    public static void DebugGreen(string format)
    {
      Debug($"[{format}]", EColor.Green);
    }
    
    [Conditional(ENABLE_DEBUG_LOG)]
    public static void DebugBlue(string format)
    {
      Debug($"[{format}]", EColor.Blue);
    }
    
    [Conditional(ENABLE_DEBUG_LOG)]
    public static void DebugYellow(object msg)
    {
      Debug($"[{msg}]", EColor.Yellow);
    }
    
    /// <summary>
    /// 对控制台打印的文本转换为有颜色的富文本，使用方法 (颜色标记是中括号 [] )
    /// <para> Log.Debug($"嘿哈-[蓝色：]-哦豁-[红色：][呀哈喽]诶嘿嘿", EColor.Blue, EColor.Red, EColor.Green, EColor.Orange, EColor.Pink); </para>
    /// 多余的中括号标记会被忽略不上色，而多余色彩参数也是会被无视 
    /// </summary>
    [Conditional(ENABLE_DEBUG_LOG)]
    public static void Debug(string format, params EColor[] msg)
    {
      // 匹配字符串中标记了中括号[]的[富文本]
      const string pattern = @"\[([^\[\]]+)\]";
      MatchCollection result = Regex.Matches(format, pattern);

      StringBuilder str = new StringBuilder(format.Length); 
      int formatCurIndex = 0;
      int maxIndex = msg.Length > result.Count ? result.Count : msg.Length;

      for (int i = 0; i < maxIndex; i++)
      {
        // 添加不需要上颜色的文本
        string s = format.Substring(formatCurIndex, result[i].Index - formatCurIndex);
        str.Append(s);
        
        string colorStr = format.Substring(result[i].Index + 1, result[i].Length - 2);
        // 添加改变为富颜色的文本
        colorStr = Colors.To(colorStr, msg[i]);
        str.Append(colorStr);

        formatCurIndex = result[i].Index + result[i].Length;
      }

      // 添加剩下的末尾文本
      if (formatCurIndex < format.Length) str.Append(format.Substring(formatCurIndex, format.Length - formatCurIndex));
      
      UnityEngine.Debug.Log(str);
    }
  }

  public enum EColor
  {
    Red,
    Green,
    Blue,
    Yellow,
    /// <summary> 紫色 </summary>
    Purple,
    Pink,
    Orange,
    /// <summary> 灰色 </summary>
    Grey
  }

  public static class Colors
  {
    /// <summary> 使得文本富有化 <color>str</color> </summary>
    public static string To(string str, EColor color)
    {
      return $"{GetColorStr(color)}{str}{ColorEnd}";
    }

    private static string GetColorStr(EColor color)
    {
      switch (color)
      {
        case EColor.Red: return Red;
        case EColor.Green: return Green;
        case EColor.Blue: return Blue;
        case EColor.Yellow: return Yellow;
        case EColor.Purple: return Purple;
        case EColor.Pink: return Pink;
        case EColor.Orange: return Orange;
        case EColor.Grey: return Grey;
        default: throw new ArgumentOutOfRangeException(nameof(color), color, null);
      }
    }

    private const string ColorEnd = "</color>";
    private const string Red = "<color=#F00000>";
    private const string Green = "<color=#3FFF00>";
    private const string Blue = "<color=#0080FF>";
    private const string Yellow = "<color=#F6FF45>";
    private const string Purple = "<color=#502FFF>";
    private const string Pink = "<color=#F16CFF>";
    private const string Orange = "<color=#FF873E>";
    private const string Grey = "<color=#8C8C8C>";
  }
}