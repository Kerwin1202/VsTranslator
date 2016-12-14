using Microsoft.VisualStudio.TestTools.UnitTesting;
using VsTranslator.Core.Baidu;
using VsTranslator.Core.Baidu.Entities;
using VsTranslator.Core.Youdao;
using VsTranslator.Core.Youdao.Entities;
using VsTranslator.Core.Youdao.Enums;

namespace VsTranslatorTest
{
    [TestClass]
    public class YoudaoTranslatorTest
    {
        readonly YoudaoTranslator _youdaoTranslator = new YoudaoTranslator("zhiyue", "702916626");

        [TestMethod]
        public void Translate()
        {
            string sourceText = "TDD completely turns traditional development around.";

            YoudaoTransResult youdaoTransResult = _youdaoTranslator.Translate(sourceText);

            Assert.IsNotNull(youdaoTransResult);

            Assert.AreEqual(youdaoTransResult.ErrorCode, ErrorCodes.Normal);

            Assert.AreEqual(youdaoTransResult.Translation[0], "TDD完全变成传统的开发。");
        }
    }
}