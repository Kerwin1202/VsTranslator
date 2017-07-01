using System;

namespace VsBackground.Enums
{
    /// <summary>
    /// 图片类型
    /// </summary>
    //[Flags]
    public enum BackgroundType
    {
        /// <summary>
        /// 整个vs ide
        /// </summary>
        EntireIde = 0x01,
        /// <summary>
        /// 仅仅只是代码编辑区域
        /// </summary>
        CodeView = 0x02,
        /// <summary>
        /// 两个都
        /// </summary>
        Both = 0x03
    }
}