using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Translate.Core.Translator;
using Translate.Core.Translator.Bing;
using Translate.Core.Translator.Entities;

namespace VsTranslatorTest
{
    [TestClass]
    public class BingTranslatorTest
    {
        
        //VsTranslator  
        //SVJTxigXb3ezDDm6ZG5hn/FC20YUbV37clW3zw8hLLE=
        readonly ITranslator _bingTranslator = new BingTranslator();
        [TestMethod]
        public void Translate()
        {
            string sourceText = "TDD completely turns traditional development around.";
            TranslationResult transResult = _bingTranslator.Translate(sourceText, "en", "zh-CHS");
            Assert.AreEqual("TDD 完全扭转了传统的发展。", transResult.TargetText);


            sourceText = "你今天过得好不好";
            transResult = _bingTranslator.Translate(sourceText, "zh-CHS", "en");
            Assert.AreEqual("Did you have a good day?", transResult.TargetText);


            sourceText = "hello\"";
            transResult = _bingTranslator.Translate(sourceText, "en", "zh-CHS");
            Assert.AreEqual("你好\"", transResult.TargetText);

            sourceText = "hello";
            transResult = _bingTranslator.Translate(sourceText, "en", "zh-CHS");
            Assert.AreEqual("你好", transResult.TargetText);

            sourceText = "<result>";
            transResult = _bingTranslator.Translate(sourceText, "en", "zh-CHS");
            Assert.AreEqual("<result>", transResult.TargetText);

            sourceText = "你今天过得好不好\r\n我很好";
            transResult = _bingTranslator.Translate(sourceText, "zh-CHS", "en");
            Assert.AreEqual("Did you have a good day?\r\nI am fine", transResult.TargetText);
        }

    }
}
