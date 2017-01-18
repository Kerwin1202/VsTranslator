using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Text;
using VsTranslator.Core.Translator;
using VsTranslator.Core.Translator.Enums;

namespace VsTranslator.Adornment
{
    /// <summary>
    /// Interaction logic for TranslatorControl.xaml
    /// </summary>
    public partial class TranslatorControl : UserControl
    {

        private TranslatorControl()
        {
            InitializeComponent();

        }

        public TranslatorControl(SnapshotSpan selectedSpans, TranslationRequest transRequest)
        {
            InitializeComponent();

            transRequest.OnTranslationComplete += TransRequest_OnTranslationComplete;
        }

        private void TransRequest_OnTranslationComplete(TranslateResult translationResult)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                AppendTargetText(translationResult.TranslationResultTypes == TranslationResultTypes.Successed
                    ? translationResult.TargetText
                    : translationResult.FailedReason);
            }));
        }

        private void AppendTargetText(string targetText)
        {
            var label = new TextBlock()
            {
                Text = targetText
            };
            transResult.Children.Add(label);
        }


        public static void ReplaceSelectedText(SnapshotSpan selectedSpans, string targetText)
        {
            var span = selectedSpans.Snapshot.CreateTrackingSpan(selectedSpans, SpanTrackingMode.EdgeExclusive);
            ITextBuffer buffer = span.TextBuffer;
            var sp = span.GetSpan(buffer.CurrentSnapshot);
            buffer.Replace(sp, targetText);
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Global.Dte.ExecuteCommand("Tools.Options", GuidList.TranslateOptions);
        }

        public Action RemoveEvent;

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            RemoveEvent?.Invoke();
        }
    }
}
