using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;

namespace VsTranslator.Settings
{
    [Guid(GuidList.TranslateOptions)]
    public class TranslateOptions : DialogPage
    {
        protected override IWin32Window Window
        {
            get
            {
                TranslateOptionsControl translateOptions = new TranslateOptionsControl { TranslateOptions = this };
                translateOptions.Initialize();
                return translateOptions;
            }
        }

    }
}