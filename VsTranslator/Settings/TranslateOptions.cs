using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;

namespace VsTranslator.Settings
{
    [Guid("00000000-0000-0000-0000-000000000000")]
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