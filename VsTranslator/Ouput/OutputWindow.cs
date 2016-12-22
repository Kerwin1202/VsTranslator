using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace VsTranslator.Ouput
{
    [Guid(GuidList.OutputWindow)]
    public sealed class OutputWindow : ToolWindowPane
    {
        public OutputWindow() : base(null)
        {
            Caption = "Translate";
            BitmapResourceID = 301;
            BitmapIndex = 1;
            Content = new OutputControl();
            ToolBarLocation = (int)VSTWT_LOCATION.VSTWT_TOP;
        }

    }
}