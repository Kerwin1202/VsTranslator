﻿using EnvDTE;

namespace Visual_Studio_2019_Translator
{
    public static class Global
    {
        public static DTE Dte = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(DTE)) as DTE;
   
    }
}