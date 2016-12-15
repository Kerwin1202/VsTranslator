using System;
using System.Globalization;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VsTranslator.Core.Baidu;
using VsTranslator.Core.Baidu.Entities;

namespace VsTranslatorTest
{
    [TestClass]
    public class BaiduTranslatorTest
    {
        readonly BaiduTranslator _baiduTranslator = new BaiduTranslator("20161214000033991", "HMlukU9THx2Twx1I14Hz");
        [TestMethod]
        public void Translate()
        {
            string sourceText = "TDD completely turns traditional development around.";
            BaiduTransResult baiduTransResult = _baiduTranslator.Translate(sourceText);
            Assert.IsNotNull(baiduTransResult);
            Assert.AreEqual("TDD完全变传统发展。", baiduTransResult.TransResult[0].Dst);


            sourceText = "你今天过得好不好";
            baiduTransResult = _baiduTranslator.Translate(sourceText, "zh", "en");
            Assert.IsNotNull(baiduTransResult);
            Assert.AreEqual("TDD完全变传统发展。", baiduTransResult.TransResult[0].Dst);

            sourceText = "hello\"";
            baiduTransResult = _baiduTranslator.Translate(sourceText);
            Assert.IsNotNull(baiduTransResult);
            Assert.AreEqual("TDD完全变传统发展。", baiduTransResult.TransResult[0].Dst);

            sourceText = "hello";
            baiduTransResult = _baiduTranslator.Translate(sourceText);
            Assert.IsNotNull(baiduTransResult);
            Assert.AreEqual("TDD完全变传统发展。", baiduTransResult.TransResult[0].Dst);

        }
    }
}