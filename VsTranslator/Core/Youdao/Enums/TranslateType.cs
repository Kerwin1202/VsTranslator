﻿using System;

namespace VsTranslator.Core.Youdao.Enums
{
    [Flags]
    public enum TranslateType
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