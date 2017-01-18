using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using VsTranslator.Core.Translator;
using VsTranslator.Core.Translator.Entities;
using VsTranslator.Core.Translator.Enums;

namespace VsTranslator.Adornment
{
    public class TranslationRequest
    {
        public event Action<TranslateResult> OnTranslationComplete;

        private readonly string _selectedText;

        private readonly List<Trans> _translators;

        public TranslationRequest(string selectedText, List<Trans> translators)
        {
            _selectedText = selectedText;
            _translators = translators ?? new List<Trans>();

            foreach (Trans translator in _translators)
            {
                new Thread(TranslationThread).Start(translator);
            }
        }

        private void TranslationThread(object obj)
        {
            Trans trans = obj as Trans;
            if (trans != null)
            {
                TranslationResult result = trans.Translator.Translate(_selectedText, trans.SourceLanguage, trans.TargetLanguage);

                TranslateResult translateResult = JsonConvert.DeserializeObject<TranslateResult>(JsonConvert.SerializeObject(result));

                translateResult.Translator = trans.Translator;
                OnTranslationComplete?.Invoke(translateResult);
            }
        }
    }

    public class Trans
    {
        public ITranslator Translator { get; set; }

        public string SourceLanguage { get; set; }

        public string TargetLanguage { get; set; }
    }

    public class TranslateResult : TranslationResult
    {
        public ITranslator Translator { get; set; }
    }
}