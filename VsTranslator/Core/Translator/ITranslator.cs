using System.Collections.Generic;
using VsTranslator.Core.Translator.Entities;

namespace VsTranslator.Core.Translator
{
    public interface ITranslator
    {
        //string Translate(string text, string from, string to);
        string GetIdentity();


        TranslationResult Translate(string text, string from, string to);
    }
}