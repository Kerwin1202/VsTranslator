using System;
using System.Windows.Media.Imaging;

namespace VsBackground.Interfaces
{
    /// <summary>
    /// 图片接口
    /// </summary>
    public interface IImageProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        BitmapSource GetBitmapSource();

    }
}