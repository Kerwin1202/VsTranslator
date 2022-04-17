using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace Visual_Studio_Translator
{
    public static class Global
    {
        public static DTE Dte = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(DTE)) as DTE;

        public static AsyncPackage Package = null;
    }
}