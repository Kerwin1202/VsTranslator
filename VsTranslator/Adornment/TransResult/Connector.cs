using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace VsTranslator.Adornment.TransResult
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    public sealed class Connector : IWpfTextViewCreationListener
    {

        [Export(typeof(AdornmentLayerDefinition))]
        [Name("ConnectorAdornmentLayer")]
        [Order(After = PredefinedAdornmentLayers.Selection, Before = PredefinedAdornmentLayers.Text)]
        [Order(After = PredefinedAdornmentLayers.Caret)]
        [Order(After = PredefinedAdornmentLayers.Outlining)]
        [Order(After = PredefinedAdornmentLayers.Selection)]
        [Order(After = PredefinedAdornmentLayers.Squiggle)]
        [Order(After = PredefinedAdornmentLayers.TextMarker)]
        public AdornmentLayerDefinition TranslatorAdornmentLayer;



        public void TextViewCreated(IWpfTextView textView)
        {
            TransAdornmentManager.Create(textView);
        }

        static public void Execute(IWpfTextViewHost host,  TranslationRequest transRequest)
        {
            IWpfTextView view = host.TextView;
            //Add a comment on the selected text.   
            if (!view.Selection.IsEmpty)
            {
                TransAdornmentManager.Add(view, transRequest);
            }
        }
    }
}