//------------------------------------------------------------------------------
// <copyright file="VsTranslatorPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using VsTranslator.Core.Translator;
using VsTranslator.Core.Translator.Google;
using VsTranslator.Settings;

namespace VsTranslator
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
    [Guid(PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]

    [ProvideOptionPage(typeof(TranslateOptions), "VsExtensions", "VsTranslator", 0, 0, true)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideAutoLoad(UIContextGuids.NoSolution)]//设置当VS打开的时候就运行本类
    [ProvideAutoLoad(UIContextGuids.SolutionExists)]//同上
    public sealed class VsTranslatorPackage : Package
    {
        /// <summary>
        /// VsTranslatorPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "08a874bc-f0d4-40bc-8bbf-a1f2e30f92e5";

        /// <summary>
        /// Initializes a new instance of the <see cref="VsTranslatorPackage"/> class.
        /// </summary>
        public VsTranslatorPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
            
                CommandID translateCommandId = new CommandID(GuidList.CommandSet, (int)PkgCmdIdList.GoogleTranslate);
                MenuCommand menuItemTranslate = new MenuCommand(TranslateMenu_Clicked, translateCommandId);

                mcs.AddCommand(menuItemTranslate);
            }
        }


        #endregion


        IWpfTextViewHost GetCurrentViewHost()
        {
            // code to get access to the editor's currently selected text cribbed from
            // http://msdn.microsoft.com/en-us/library/dd884850.aspx
            IVsTextManager txtMgr = (IVsTextManager)GetService(typeof(SVsTextManager));
            IVsTextView vTextView = null;
            int mustHaveFocus = 1;
            txtMgr.GetActiveView(mustHaveFocus, null, out vTextView);
            IVsUserData userData = vTextView as IVsUserData;
            if (userData == null)
            {
                return null;
            }
            object holder;
            Guid guidViewHost = DefGuidList.guidIWpfTextViewHost;
            userData.GetData(ref guidViewHost, out holder);
            var viewHost = (IWpfTextViewHost)holder;
            return viewHost;
        }


        /// Given an IWpfTextViewHost representing the currently selected editor pane,
        /// return the ITextDocument for that view. That's useful for learning things 
        /// like the filename of the document, its creation date, and so on.
        ITextDocument GetTextDocumentForView(IWpfTextViewHost viewHost)
        {
            ITextDocument document;
            viewHost.TextView.TextDataModel.DocumentBuffer.Properties.TryGetProperty(typeof(ITextDocument), out document);
            return document;
        }

        /// Get the current editor selection
        ITextSelection GetSelection(IWpfTextViewHost viewHost)
        {
            return viewHost.TextView.Selection;
        }


        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void TranslateMenu_Clicked(object sender, EventArgs e)
        {
            MenuCommand MenuCommand = sender as MenuCommand;
            //MenuCommand.Enabled = false;
            var viewHost = GetCurrentViewHost();
            if (viewHost == null)
            {
                return;
            }
            IWpfTextView view = viewHost.TextView;
            ITextSelection selection = view.Selection;
            if (selection != null)
            {
                SnapshotSpan span = view.Selection.SelectedSpans[0];
                String selectedText = span.GetText();
                //nothing is selected - taking the entire line
                if (string.IsNullOrEmpty(selectedText))
                {
                    ITextSnapshotLine line = span.Start.GetContainingLine();
                    selectedText = span.Start.GetContainingLine().GetText();
                    view.Selection.Select(new SnapshotSpan(line.Start, line.End), false);
                }
                //still no selection
                if (string.IsNullOrWhiteSpace(selectedText))
                {
                    MessageBox.Show("Nothing to translate", "Translator", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(GoogleTranslator.Translate(selectedText, "en", "zh-CN").TargetText, "翻译结果", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //Options opt = Global.Options;
                    ////TranslationResult translation = opt.Translator.GetTranslation(selectedText, opt.SourceLanguage, opt.TargetLanguage);
                    //TranslationRequest request = new TranslationRequest(selectedText, opt.Translator, opt.SourceLanguage, opt.TargetLanguage);
                    //Connector.Execute(view, request);
                }
            }
        }



        internal readonly ITranslator GoogleTranslator = new GoogleTranslator();


        // TranslateOptions page = (TranslateOptions)GetDialogPage(typeof(TranslateOptions));

    }
}
