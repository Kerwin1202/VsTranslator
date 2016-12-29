using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using VsTranslator.Adornment;
using VsTranslator.Core.Translator;
using VsTranslator.Core.Translator.Entities;
using VsTranslator.Core.Translator.Enums;

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


            AddCommand2OleMenu(GuidList.CommandSet, (int)PkgCmdIdList.GoogleTranslate, TranslateMenu_Clicked);
            AddCommand2OleMenu(GuidList.CommandSet, (int)PkgCmdIdList.BingTranslate, TranslateMenu_Clicked);
            AddCommand2OleMenu(GuidList.CommandSet, (int)PkgCmdIdList.BaiduTranslate, TranslateMenu_Clicked);
            AddCommand2OleMenu(GuidList.CommandSet, (int)PkgCmdIdList.YoudaoTranslate, TranslateMenu_Clicked);

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
                        var targetText = Translate(menuCommand.CommandID.ID, selectedText);
                        buffer.Replace(sp, selectedText + "->" + targetText);
                    }
                    catch (Exception)
                    {
                        //
                    }
                }
            }
        }

        private static string Translate(int commandId, string selectedText)
        {
            StatusBarCmd.SetStatusTextWithFreeze("Translating...");
            string fromWay = string.Empty;
            string targetText = string.Empty;
            TranslationResult result = new TranslationResult() { FailedReason = "Can't translate by this way", TranslationResultTypes = TranslationResultTypes.Failed, TargetText = string.Empty };
            try
            {
                switch (commandId)
                {
                    case (int)PkgCmdIdList.GoogleTranslate:
                        fromWay = "Google";
                        result = TranslatorFactory.TranslateByGoogle(selectedText);
                        break;
                    case (int)PkgCmdIdList.BingTranslate:
                        fromWay = "Bing";
                        result = TranslatorFactory.TranslateByBing(selectedText);
                        break;
                    case (int)PkgCmdIdList.BaiduTranslate:
                        fromWay = "Baidu";
                        result = TranslatorFactory.TranslateByBaidu(selectedText);
                        break;
                    case (int)PkgCmdIdList.YoudaoTranslate:
                        fromWay = "Youdao";
                        result = TranslatorFactory.TranslateByYoudao(selectedText);
                        break;
                }
                if (result.TranslationResultTypes == TranslationResultTypes.Successed)
                {
                    targetText = result.TargetText;
                    Connector.Execute(GetCurrentViewHost(), targetText);
                    StatusBarCmd.SetStatusTextWithoutFreeze(fromWay + " translate successed, from " + selectedText + " to " + targetText);
                }
                else
                {
                    throw new Exception(result.FailedReason);
                }
            }
            catch (Exception exception)
            {
                StatusBarCmd.SetStatusTextWithoutFreeze(fromWay + " translate failed, " + exception.Message);
                throw;
            }
            return targetText;
        }


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

        public static void AddCommand2OleMenu(Guid commandSet, int commandId, EventHandler eventHandler)
        {
            OleMenuCommandService mcs = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                CommandID translateCommandId = new CommandID(commandSet, commandId);
                OleMenuCommand menuItemTranslate = new OleMenuCommand(eventHandler, translateCommandId)
                {
                    Enabled = false
                };
                mcs.AddCommand(menuItemTranslate);
            }
        }
    }
}