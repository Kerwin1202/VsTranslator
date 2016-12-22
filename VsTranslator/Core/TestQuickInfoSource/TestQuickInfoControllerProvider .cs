using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace VsTranslator.Core.TestQuickInfoSource
{
    [Export(typeof(IIntellisenseControllerProvider))]
    [Name("ToolTip QuickInfo Controller")]
    [ContentType("text")]
    internal class TestQuickInfoControllerProvider : IIntellisenseControllerProvider
    {
        [Import]
        internal IQuickInfoBroker QuickInfoBroker { get; set; }

        public IIntellisenseController TryCreateIntellisenseController(ITextView textView, IList<ITextBuffer> subjectBuffers)
        {
            return new TestQuickInfoController(textView, subjectBuffers, this);
        }
    }
}