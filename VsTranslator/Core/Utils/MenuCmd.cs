using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Translate.Core.Translator;
using Translate.Settings;
using VsTranslator.Adornment.Translate;
using VsTranslator.Adornment.TransResult;

namespace VsTranslator.Core.Utils
{
    public class MenuCmd
    {
        public static MenuCmd Instance
        {
            get;
            private set;
        }
        private static Package _package;

        private static IServiceProvider ServiceProvider => _package;

        public static void Initialize(Package package)
        {
            Instance = new MenuCmd(package);


            #region four translate menu 
            AddCommand2OleMenu(GuidList.CommandSet, (int)PkgCmdIdList.GoogleTranslate, TranslateMenu_Clicked);
            AddCommand2OleMenu(GuidList.CommandSet, (int)PkgCmdIdList.BingTranslate, TranslateMenu_Clicked);
            AddCommand2OleMenu(GuidList.CommandSet, (int)PkgCmdIdList.BaiduTranslate, TranslateMenu_Clicked);
            AddCommand2OleMenu(GuidList.CommandSet, (int)PkgCmdIdList.YoudaoTranslate, TranslateMenu_Clicked);
            #endregion

            #region translate option menu
            AddCommand2OleMenu(GuidList.CommandSet, (int)PkgCmdIdList.TranslateOptions, TranslateOptions_Clicked, true);
            #endregion

            #region translate client menu
            AddCommand2OleMenu(GuidList.CommandSet, (int)PkgCmdIdList.TranslateClient, TranslateClient_Clicked, true);
            #endregion

            #region translate in website
            AddCommand2OleMenu(GuidList.CommandSet, (int)PkgCmdIdList.TranslateInWebSite, TranslateInWebSite_Clicked, true);
            #endregion
        }


        /// <summary>
        /// Open the translate page in browser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TranslateInWebSite_Clicked(object sender, EventArgs e)
        {
            Process.Start("http://www.zhanghuanglong.com/translate/");
        }

        /// <summary>
        /// Open translate client windows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void TranslateClient_Clicked(object sender, EventArgs e)
        {
            //OptionsSettings.ShowClient();
            ToolWindowPane window = _package.FindToolWindow(typeof(TranslateClient), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException("Cannot create tool window");
            }
            Guid nullGuid = Guid.Empty;
            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
            //set the width and height
            windowFrame.SetFramePos(VSSETFRAMEPOS.SFP_fSize, ref nullGuid, 0, 0, 640, 400);
        }

        private static void TranslateOptions_Clicked(object sender, EventArgs e)
        {
            OptionsSettings.ShowOptions();
        }

        private MenuCmd(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            _package = package;
        }

        static IWpfTextViewHost GetCurrentViewHost()
        {
            // code to get access to the editor's currently selected text cribbed from
            // http://msdn.microsoft.com/en-us/library/dd884850.aspx
            IVsTextManager txtMgr = (IVsTextManager)ServiceProvider.GetService(typeof(SVsTextManager));
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


        private static void TranslateMenu_Clicked(object sender, EventArgs e)
        {
            MenuCommand menuCommand = sender as MenuCommand;
            //MenuCommand.Enabled = false;
            var viewHost = GetCurrentViewHost();
            if (viewHost == null || menuCommand == null)
            {
                return;
            }
            IWpfTextView view = viewHost.TextView;
            ITextSelection selection = view.Selection;
            if (selection != null)
            {
                SnapshotSpan selectionSpan = view.Selection.SelectedSpans[0];
                String selectedText = selectionSpan.GetText();
                //nothing is selected - taking the entire line
                if (string.IsNullOrEmpty(selectedText))
                {
                    #region commented //selected whole line when no selection text
                    //ITextSnapshotLine line = span.Start.GetContainingLine();
                    //selectedText = span.Start.GetContainingLine().GetText();
                    //view.Selection.Select(new SnapshotSpan(line.Start, line.End), false); 
                    #endregion

                    StatusBarCmd.SetStatusTextWithoutFreeze("nothing selected, make sure selected, then try again");
                }
                //still no selection
                if (string.IsNullOrWhiteSpace(selectedText))
                {
                    StatusBarCmd.SetStatusTextWithoutFreeze("nothing selected, make sure selected, then try again");
                }
                else
                {
                    var span = selectionSpan.Snapshot.CreateTrackingSpan(selectionSpan, SpanTrackingMode.EdgeExclusive);
                    ITextBuffer buffer = view.Selection.TextView.TextBuffer;
                    Span sp = span.GetSpan(buffer.CurrentSnapshot);
                    try
                    {
                        Translate(menuCommand.CommandID.ID, selectedText);
                        //buffer.Replace(sp, selectedText);
                    }
                    catch (Exception exception)
                    {
                        //
                    }
                }
            }
        }

        private static void Translate(int commandId, string selectedText)
        {
            TranslateType translateType = (TranslateType) commandId;
            selectedText = OptionsSettings.SpliteLetterByRules(selectedText);
            ITranslator translator = null;
            try
            {
                translator = TranslatorFactory.GetTranslator(translateType);
            }
            catch (Exception exception)
            {
                //When app id and client secret is empty, this exception will triggered
                if (exception.Message == "app id and client secret is necessary")
                {
                    DialogResult result = MessageBox.Show($"{exception.Message}, go to set?", "Tip", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        OptionsSettings.ShowOptions();
                    }
                }
                else
                {
                    MessageBox.Show(exception.Message, "Tip", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            TranslationRequest transRequest = new TranslationRequest(selectedText, new List<Trans>() { new Trans()
            {
                Translator = translator,
                SourceLanguage = TranslatorFactory.GetSourceLanguage(translateType,selectedText),
                TargetLanguage = TranslatorFactory.GetTargetLanguage(translateType,selectedText),
            } });
            Connector.Execute(GetCurrentViewHost(), transRequest);
        }


        /// <summary>
        /// Change the enabled status of the translate menu
        /// </summary>
        /// <param name="cmdId"></param>
        /// <param name="enableCmd"></param>
        private void ChangeTranslatorCommand(int cmdId, bool enableCmd)
        {
            var mcs = ServiceProvider.GetService(typeof(IMenuCommandService))
                as OleMenuCommandService;
            var newCmdId = new CommandID(GuidList.CommandSet, cmdId);
            MenuCommand mc = mcs?.FindCommand(newCmdId);
            if (mc != null)
            {
                mc.Enabled = enableCmd;
            }
        }

        public void ChangeTranslatorCommand(bool enableCmd)
        {
            ChangeTranslatorCommand((int)PkgCmdIdList.GoogleTranslate, enableCmd);
            ChangeTranslatorCommand((int)PkgCmdIdList.BingTranslate, enableCmd);
            ChangeTranslatorCommand((int)PkgCmdIdList.BaiduTranslate, enableCmd);
            ChangeTranslatorCommand((int)PkgCmdIdList.YoudaoTranslate, enableCmd);
        }

        /// <summary>
        /// Bind event to translate menu and set the menu enabled status
        /// </summary>
        /// <param name="commandSet"></param>
        /// <param name="commandId"></param>
        /// <param name="eventHandler"></param>
        /// <param name="isEnabled"></param>
        public static void AddCommand2OleMenu(Guid commandSet, int commandId, EventHandler eventHandler, bool isEnabled = false)
        {
            OleMenuCommandService mcs = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                CommandID translateCommandId = new CommandID(commandSet, commandId);
                OleMenuCommand menuItemTranslate = new OleMenuCommand(eventHandler, translateCommandId)
                {
                    Enabled = isEnabled
                };
                mcs.AddCommand(menuItemTranslate);
            }
        }
    }
}