using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VsTranslator.Core;
using VsTranslator.Core.Entities;
using VsTranslator.Core.Google;
using VsTranslator.Core.Utils;

namespace VsTranslatorTest
{
    [TestClass]
    public class GoogleTranslatorTest
    {
        private readonly ITranslator _googleTranslator = new GoogleTranslator();
        [TestMethod]
        public void Translate()
        {
            string sourceText = "TDD completely turns traditional development around.";
            TranslationResult transResult = _googleTranslator.Translate(sourceText, "en", "zh-CN");
            Assert.AreEqual("TDD完全转变了传统发展。", transResult.TargetText);

            sourceText = "你今天过得好不好";
            transResult = _googleTranslator.Translate(sourceText, "zh-CN", "en");
            Assert.AreEqual("You have a good day today", transResult.TargetText);

            sourceText = "hello\"";
            transResult = _googleTranslator.Translate(sourceText, "en", "zh-CN");
            Assert.AreEqual("你好”", transResult.TargetText);

            sourceText = "hello";
            transResult = _googleTranslator.Translate(sourceText, "en", "zh-CN");
            Assert.AreEqual("你好", transResult.TargetText);
        }
    }
}