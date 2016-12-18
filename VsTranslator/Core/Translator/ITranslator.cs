using System.Collections.Generic;
using VsTranslator.Core.Translator.Entities;

namespace VsTranslator.Core.Translator
{
    public interface ITranslator
    {
        string GetName();

        string GetDescription();

        string GetWebsite();

        //string Translate(string text, string from, string to);

        List<TranslationLanguage> GetTargetLanguages();

        List<TranslationLanguage> GetSourceLanguages();

        TranslationResult Translate(string text, string from, string to);
    }
}