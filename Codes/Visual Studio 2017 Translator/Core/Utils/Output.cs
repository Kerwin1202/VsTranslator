using System;
using EnvDTE;

namespace Visual_Studio_2017_Translator.Core.Utils
{
    public class Output
    {
        public static void OutputString(string message)
        {
            var w = (EnvDTE.Window)Global.Dte.Windows.Item(EnvDTE.Constants.vsWindowKindOutput);
            if (!w.Visible)
            {
                w.Visible = true;
            }
            OutputWindow ow = (OutputWindow)w.Object;
            OutputWindowPane owp;
            try
            {
                owp = ow.OutputWindowPanes.Item("VsTranslate");
            }
            catch (Exception exception)
            {
                owp = ow.OutputWindowPanes.Add("VsTranslate");
            }
            owp.Activate();
            owp.OutputString($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\r\n{message}\r\n\r\n");
        }
    }
}