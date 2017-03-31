using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TranslateService.Enums
{
    /// <summary>
    /// Translate services' type
    /// </summary>
    public enum TranslateTypes
    {
        /// <summary>
        /// 谷歌
        /// </summary>
        Google = 1,
        /// <summary>
        /// 微软
        /// </summary>
        Bing = 2,
        /// <summary>
        /// 百度
        /// </summary>
        Baidu = 3,
        /// <summary>
        /// 有道
        /// </summary>
        Youdao = 4
    }
}