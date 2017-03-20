using Translate.Core.Translator.Entities;

namespace Translate.Core.Translator
{
    public interface ITranslator
    {
        //string Translate(string text, string from, string to);
        string GetIdentity();


        TranslationResult Translate(string text, string from, string to);



    }
}