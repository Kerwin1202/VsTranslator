using Microsoft.VisualStudio.TestTools.UnitTesting;
using VsTranslator.Core.Google;

namespace VsTranslatorTest
{
    [TestClass]
    public class GoogleTranslatorTest
    {
        [TestMethod]
        public void Translate()
        {
            string sourceText = "TDD completely turns traditional development around.";
            var targetText = new GoogleTranslator().Translate(sourceText);

        }
    }
}