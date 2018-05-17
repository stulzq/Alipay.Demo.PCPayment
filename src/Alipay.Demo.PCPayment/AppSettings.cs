// #region File Annotation
// 
// Author：Zhiqiang Li
// 
// FileName：AppSettings.cs
// 
// Project：Alipay.Demo.PCPayment
// 
// CreateDate：2018/05/17
// 
// Note: The reference to this document code must not delete this note, and indicate the source!
// 
// #endregion

using System;

namespace Alipay.Demo.PCPayment
{
    public class AppSettings
    {
        public static DateTime ApplicationStartTime { get; set; } = DateTime.Now;

        public static string RunningStr
        {
            get
            {
                var ts = DateTime.Now- ApplicationStartTime;
                return $"{ts.Days}天{ts.Hours}时{ts.Minutes}分{ts.Seconds}秒";
            }
        }
    }
}