using System;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Visual_Studio_2017_Translator.Core.Utils
{
    public class StatusBarCmd
    {
        private static Package _package;

        private static IServiceProvider ServiceProvider => _package;

        public static void Initialize(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }
            _package = package;
        }

        public static void SetStatusTextWithFreeze(string statusText)
        {
            IVsStatusbar statusBar = (IVsStatusbar)ServiceProvider.GetService(typeof(SVsStatusbar));
            // Make sure the status bar is not frozen  
            int frozen;
            statusBar.IsFrozen(out frozen);
            if (frozen != 0)
            {
                statusBar.FreezeOutput(0);
            }
            // Set the status bar text and make its display static.  
            statusBar.SetText($"[VsTranslate] {statusText}");
            // Freeze the status bar.  
            statusBar.FreezeOutput(1);
        }

        public static void SetStatusTextWithoutFreeze(string statusText)
        {
            IVsStatusbar statusBar = (IVsStatusbar)ServiceProvider.GetService(typeof(SVsStatusbar));
            // Make sure the status bar is not frozen  
            int frozen;
            statusBar.IsFrozen(out frozen);
            if (frozen != 0)
            {
                statusBar.FreezeOutput(0);
            }
            // Set the status bar text and make its display static.  
            statusBar.SetText($"[VsTranslate] {statusText}");
            statusBar.FreezeOutput(0);
        }


        public static void ClearStatus()
        {
            IVsStatusbar statusBar = (IVsStatusbar)ServiceProvider.GetService(typeof(SVsStatusbar));
            statusBar.Clear();
        }
    }
}