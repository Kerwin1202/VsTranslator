using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VsTranslator.Core;
using VsTranslator.Core.Translator;
using VsTranslator.Core.Translator.Entities;
using VsTranslator.Core.Translator.Google;

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

            sourceText = "It's a very small project and may be fairly self explanatory if you are familiar with Visual Studio editor extensions. There are two components to the extension:";
            transResult = _googleTranslator.Translate(sourceText, "en", "zh-CN");
            Assert.AreEqual("这是一个非常小的项目，如果你熟悉Visual Studio编辑器扩展可能是相当自我解释。 扩展有两个组件：", transResult.TargetText);
        }
    }
}