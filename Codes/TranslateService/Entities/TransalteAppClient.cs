using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TranslateService.Enums;

namespace TranslateService.Entities
{
    public class TransalteAppClient
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
        /// 第三方的App key
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// 第三方的App key对应的密钥
        /// </summary>
        public string ClientSecret { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
        /// <summary>
        /// App Client状态 
        /// </summary>
        public AppClientStates State { get; set; }

    }
}