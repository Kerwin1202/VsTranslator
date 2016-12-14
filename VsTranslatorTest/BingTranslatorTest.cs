using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VsTranslator.Core.Bing;

namespace VsTranslatorTest
{
    [TestClass]
    public class BingTranslatorTest
    {
       readonly  BingTranslator _bingTranslator = new BingTranslator("VsTranslatorByKerwin", "LOAgOYu99LyNzVoa+LL53zIk93RFhAaZQxwtSW+an5E=");
        [TestMethod]
        public void Translate()
        {
            string sourceText = "TDD completely turns traditional development around.";

            var targetText =  _bingTranslator.Translate(sourceText);

            Assert.AreEqual("TDD 完全扭转传统的发展。", targetText);
        }

    }
}
