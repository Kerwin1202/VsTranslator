using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VsTranslator.Core.Bing;

namespace VsTranslatorTest
{
    [TestClass]
    public class BingTranslatorTest
    {
        readonly BingTranslator _bingTranslator = new BingTranslator("VsTranslatorByKerwin", "LOAgOYu99LyNzVoa+LL53zIk93RFhAaZQxwtSW+an5E=");
        [TestMethod]
        public void Translate()
        {
            string sourceText = "TDD completely turns traditional development around.";
            var targetText = _bingTranslator.Translate(sourceText);
            Assert.AreEqual(targetText, "TDD 完全扭转传统的发展。");


            sourceText = "你今天过得好不好";
            targetText = _bingTranslator.Translate(sourceText, "zh-CHS", "en");
            Assert.AreEqual("You have good", targetText);


            sourceText = "hello\"";
            targetText = _bingTranslator.Translate(sourceText);
            Assert.AreEqual("hello\"", targetText);

            sourceText = "hello";
            targetText = _bingTranslator.Translate(sourceText);
            Assert.AreEqual("你好", targetText);
        }

    }
}
