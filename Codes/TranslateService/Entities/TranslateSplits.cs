using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TranslateService.Enums;

namespace TranslateService.Entities
{
    public class TranslateSplits
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UId { get; set; }
        /// <summary>
        /// 翻译类型
        /// </summary>
        public TranslateTypes TranslateType { get; set; }
        /// <summary>
        /// 匹配的正则
        /// </summary>
        public string MatchRegex { get; set; }
        /// <summary>
        /// 替换的正则
        /// </summary>
        public string ReplaceRegex { get; set; }
        /// <summary>
        /// 测试匹配和替换正则的例子
        /// </summary>
        public string ExampleRegex { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
        /// <summary>
        /// 分割例子的正则
        /// </summary>
        public SplitStates State { get; set; }
    }
}