using System;

namespace Translate.Core.Translator.Youdao.Enums
{
    [Flags]
    internal enum TranslateType
    {
        /// <summary>
        /// Gets the Dictionary data 
        /// </summary>
        Dict = 1,
        /// <summary>
        /// Get translation data
        /// </summary>
        Translate = 2
    }
}