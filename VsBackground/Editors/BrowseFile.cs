using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace VsBackground.Editors
{
    /// <summary>
    /// 浏览文件
    /// </summary>
    internal class BrowseFile : UITypeEditor
    {
        /// <summary>
        /// 设置编辑的样式 为弹窗浏览文件模式  1.Modal->弹窗 2.DropDown->侠岚 3.None->文本框
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        /// <summary>
        /// 点击编辑的时候 选择文件
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            //获取这个弹窗
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider?.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                var files = (string)value;
                using (var ofd = new OpenFileDialog() { Multiselect = true })
                {
                    ofd.Filter = "图片|*.jpg;*.png";
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(files))
                        {
                            ofd.InitialDirectory = Path.GetDirectoryName(files.Split('|')[0]);
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        files = string.Empty;
                        foreach (var fileName in ofd.FileNames)
                        {
                            files += fileName + "|";
                        }
                        if (files.Length > 0)
                        {
                            files = files.Substring(0, files.Length - 1);
                        }
                        return files;
                    }
                }
            }
            return value;
        }
        /// <summary>
        /// 是否支持绘图
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}