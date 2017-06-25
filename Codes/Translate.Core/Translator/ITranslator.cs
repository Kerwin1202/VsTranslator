using System.Collections.Generic;
using Translate.Core.Translator.Entities;

namespace Translate.Core.Translator
{
    public interface ITranslator
    {
        //string Translate(string text, string from, string to);

        /// <summary>
        /// To get translate service's name
        /// </summary>
        /// <returns></returns>
        string GetIdentity();
        /// <summary>
        /// Translate text from source language to target language
        /// </summary>
        /// <param name="text">The text what Will be translated</param>
        /// <param name="from">Source language</param>
        /// <param name="to">Target language</param>
        /// <returns></returns>
        TranslationResult Translate(string text, string from, string to);
        /// <summary>
        /// To get all support target languages
        /// </summary>
        /// <returns></returns>
        List<TranslationLanguage> GetAllTargetLanguages();
        /// <summary>
        /// To get all support source languages
        /// </summary>
        /// <returns></returns>
        List<TranslationLanguage> GetAllSourceLanguages();

    }
}