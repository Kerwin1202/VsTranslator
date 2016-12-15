using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VsTranslator.Core.Google;
using VsTranslator.Core.Utils;

namespace VsTranslatorTest
{
    [TestClass]
    public class GoogleTranslatorTest
    {
        private readonly GoogleTranslator _googleTranslator = new GoogleTranslator();
        [TestMethod]
        public void Translate()
        {
            string sourceText = "TDD completely turns traditional development around.";
            var targetText = _googleTranslator.Translate(sourceText);
            Assert.AreEqual("TDD完全转变了传统发展。", targetText);

            sourceText = "你今天过得好不好";
            targetText = _googleTranslator.Translate(sourceText, "zh-CN", "en");
            Assert.AreEqual("You have a good day today", targetText);

            sourceText = "hello\"";
            targetText = _googleTranslator.Translate(sourceText);
            Assert.AreEqual("你好”", targetText);

            sourceText = "hello";
            targetText = _googleTranslator.Translate(sourceText);
            Assert.AreEqual("你好", targetText);
        }
    }
}