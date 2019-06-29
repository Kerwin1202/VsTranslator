using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using Visual_Studio_2019_Translator.Adornment.Translate;
using Visual_Studio_2019_Translator.Core.Utils;

namespace Visual_Studio_2019_Translator
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]


    [Guid(GuidList.PackageGuidString)]
    [ProvideToolWindow(typeof(TranslateClient))]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideAutoLoad(UIContextGuids.NoSolution)]//设置当VS打开的时候就运行本类
    [ProvideAutoLoad(UIContextGuids.SolutionExists)]//同上
    public sealed class VsTranslatorPackage : Package
    {
        /// <summary>
        /// VsTranslatorPackage GUID string.
        /// </summary>
        //public const string PackageGuidString = "c4de4f02-b3d9-405f-94dc-6303c1d9dc14";

        /// <summary>
        /// Initializes a new instance of the <see cref="VsTranslator"/> class.
        /// </summary>
        public VsTranslatorPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }


        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            MenuCmd.Initialize(this);
            StatusBarCmd.Initialize(this);
            if (Application.Current.MainWindow != null) Application.Current.MainWindow.Loaded += MainWindow_Loaded;
        }

        // Load事件的方法
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MenuCmd.Instance.ChangeMenuCommandEnableStatus(false);
        }
    }
}
