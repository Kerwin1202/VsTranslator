using System;
using Microsoft.VisualStudio.Text.Editor;

namespace VsBackground.Services
{
    /// <summary>
    /// 当代码编辑区域创建的时候监听事件
    /// </summary>
    public class ListenFactory : IWpfTextViewCreationListener
    {
        public void TextViewCreated(IWpfTextView textView)
        {
            //Console.WriteLine(textView.Background);
        }
    }
}