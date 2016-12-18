using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VsTranslator.Core;
using VsTranslator.Core.Translator;
using VsTranslator.Core.Translator.Bing;
using VsTranslator.Core.Translator.Entities;

namespace VsTranslatorTest
{
    [TestClass]
    public class BingTranslatorTest
    {
        
        //VsTranslator  
        //SVJTxigXb3ezDDm6ZG5hn/FC20YUbV37clW3zw8hLLE=
        readonly ITranslator _bingTranslator = new BingTranslator("VsTranslatorByKerwin", "LOAgOYu99LyNzVoa+LL53zIk93RFhAaZQxwtSW+an5E=");
        [TestMethod]
        public void Translate()
        {
            string sourceText = "TDD completely turns traditional development around.";
            TranslationResult transResult = _bingTranslator.Translate(sourceText, "en", "zh-CHS");
            Assert.AreEqual( "TDD 完全扭转传统的发展。", transResult.TargetText);


            sourceText = "你今天过得好不好";
            transResult = _bingTranslator.Translate(sourceText, "zh-CHS", "en");
            Assert.AreEqual("You have good", transResult.TargetText);


            sourceText = "hello\"";
            transResult = _bingTranslator.Translate(sourceText, "en", "zh-CHS");
            Assert.AreEqual("你好\"", transResult.TargetText);

            sourceText = "hello";
            transResult = _bingTranslator.Translate(sourceText, "en", "zh-CHS");
            Assert.AreEqual("你好", transResult.TargetText);
        }

    }
}
