using System;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;

namespace Visual_Studio_Translator.Core.Utils
{
    public class Output
    {
        private const string OUTPUT_PANE_TITLE = "VsTranslate";
        private static IVsOutputWindowPane _outputWindowPane;

        private static IVsOutputWindowPane OutputWindowPane => _outputWindowPane ?? (_outputWindowPane = GetOutputPane());
        
        public static void OutputString(string message)
        {
            if (OutputWindowPane is object)
            {
                string outputMessage = $"[{DateTime.Now.ToString("hh:mm:ss tt")}]{Environment.NewLine}{message}{Environment.NewLine}{Environment.NewLine}";
                OutputWindowPane.Activate();
                OutputWindowPane.OutputString(outputMessage);
            }
        }

        private static IVsOutputWindowPane GetOutputPane()
        {
            IServiceProvider ServiceProvider = Global.Package;
            if (ServiceProvider == null)
            {
                return null;
            }
            if (!(ServiceProvider.GetService(typeof(SVsOutputWindow)) is IVsOutputWindow outputWindow))
                return null;
            
            Guid outputPaneGuid = new Guid(GuidList.OutputWindow);

            outputWindow.CreatePane(ref outputPaneGuid, OUTPUT_PANE_TITLE, fInitVisible: 1, fClearWithSolution: 1);
            outputWindow.GetPane(ref outputPaneGuid, out IVsOutputWindowPane windowPane);

            return windowPane;
        }
    }
}