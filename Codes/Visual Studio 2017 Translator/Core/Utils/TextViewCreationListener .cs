using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Visual_Studio_2017_Translator.Core.Utils
{
    //[ContentType("code")]
    [ContentType("text")]
    [Export(typeof(IWpfTextViewCreationListener))]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    public sealed class TextViewCreationListener : IWpfTextViewCreationListener
    {
        [Export(typeof(AdornmentLayerDefinition))]
        [Name("TranslatorAdornmentLayer")]
        [Order(After = PredefinedAdornmentLayers.Selection, Before = PredefinedAdornmentLayers.Text)]
        [Order(After = PredefinedAdornmentLayers.Caret)]
        [Order(After = PredefinedAdornmentLayers.Outlining)]
        [Order(After = PredefinedAdornmentLayers.Selection)]
        [Order(After = PredefinedAdornmentLayers.Squiggle)]
        [Order(After = PredefinedAdornmentLayers.Text)]
        [Order(After = PredefinedAdornmentLayers.TextMarker)]
        public AdornmentLayerDefinition TranslatorLayerDefinition;

        private IWpfTextView _view;

        public void TextViewCreated(IWpfTextView textView)
        {
            _view = textView;
            _view.Selection.SelectionChanged += Selection_SelectionChanged;
            _view.Closed += _view_Closed;


            //IEditorFormatMap formatMap = FormatMapService.GetEditorFormatMap(textView);

            //ResourceDictionary regularCaretProperties = formatMap.GetProperties("Caret");
            //ResourceDictionary overwriteCaretProperties = formatMap.GetProperties("Overwrite Caret");
            //ResourceDictionary indicatorMargin = formatMap.GetProperties("Indicator Margin");
            //ResourceDictionary visibleWhitespace = formatMap.GetProperties("Visible Whitespace");
            //ResourceDictionary selectedText = formatMap.GetProperties("Selected Text");
            //ResourceDictionary inactiveSelectedText = formatMap.GetProperties("Inactive Selected Text");

            //formatMap.BeginBatchUpdate();

            //regularCaretProperties[EditorFormatDefinition.ForegroundBrushId] = Brushes.Magenta;
            //formatMap.SetProperties("Caret", regularCaretProperties);

            //overwriteCaretProperties[EditorFormatDefinition.ForegroundBrushId] = Brushes.Turquoise;
            //formatMap.SetProperties("Overwrite Caret", overwriteCaretProperties);

            //indicatorMargin[EditorFormatDefinition.BackgroundColorId] = Colors.LightGreen;
            //formatMap.SetProperties("Indicator Margin", indicatorMargin);

            //visibleWhitespace[EditorFormatDefinition.ForegroundColorId] = Colors.Red;
            //formatMap.SetProperties("Visible Whitespace", visibleWhitespace);

            //selectedText[EditorFormatDefinition.BackgroundBrushId] = Brushes.LightPink;
            //formatMap.SetProperties("Selected Text", selectedText);

            //inactiveSelectedText[EditorFormatDefinition.BackgroundBrushId] = Brushes.DeepPink;
            //formatMap.SetProperties("Inactive Selected Text", inactiveSelectedText);

            //formatMap.EndBatchUpdate();
        }

        private void _view_Closed(object sender, EventArgs e)
        {
            MenuCmd.Instance.ChangeMenuCommandEnableStatus(false);
        }

        private void Selection_SelectionChanged(object sender, EventArgs e)
        {
            //TransAdornmentManager.RemoveAllAdornments();
            ITextSelection selection = sender as ITextSelection;
            if (selection != null)
            {
                SnapshotSpan span = selection.SelectedSpans[0];
                string selectedText = span.GetText();
                var hasSelectedText = !string.IsNullOrWhiteSpace(selectedText);
                MenuCmd.Instance.ChangeMenuCommandEnableStatus(hasSelectedText);
            }
            else
            {
                MenuCmd.Instance.ChangeMenuCommandEnableStatus(false);
            }
        }


        [Import]
        internal IEditorFormatMapService FormatMapService = null;
    }
}