using System;
using TranslateService.Enums;

namespace TranslateService.Entities
{
    public class TranslateSettings
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
        /// 翻译源
        /// </summary>
        public TranslateTypes TranslateType { get; set; }
        /// <summary>
        /// 当前选择的源语言的索引
        /// </summary>
        public int SourceLanguageIndex { get; set; }
        /// <summary>
        /// 当前选择的目标语言索引
        /// </summary>
        public int TargetLanguageIndex { get; set; }
        /// <summary>
        /// 当前选择的如果翻译源语言为中文(目前写死默认中文) 的时候翻译的目标语言的索引
        /// </summary>
        public int LastTargatLanguageIndex { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public SettingStates State { get; set; }
    }
}