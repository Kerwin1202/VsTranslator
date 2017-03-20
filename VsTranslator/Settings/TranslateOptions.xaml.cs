using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Translate.Core.Translator.Baidu;
using Translate.Core.Translator.Bing;
using Translate.Core.Translator.Entities;
using Translate.Core.Translator.Google;
using Translate.Core.Translator.Youdao;

namespace VsTranslator.Settings
{
    /// <summary>
    /// Interaction logic for TranslateOptionsControl2.xaml
    /// </summary>
    public partial class TranslateOptions : Window
    {

        public Action<Settings> OnSave;

        public readonly Settings Settings;


        public TranslateOptions(Settings settings)
        {
            Settings = settings ?? new Settings();

            InitializeComponent();

            //Bind settings to views
            grid.DataContext = Settings;


            cbService.Items.Add(GoogleTranslator.GetName());
            cbService.Items.Add(BingTranslator.GetName());
            cbService.Items.Add(BaiduTranslator.GetName());
            cbService.Items.Add(YoudaoTranslator.GetName());
        }

        private void lblBaidu_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("http://api.fanyi.baidu.com/api/trans/product/index");
        }

        private void lblBing_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://datamarket.azure.com/developer/applications/");
        }

        private void lblYoudao_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("http://fanyi.youdao.com/openapi");
        }

        private void btnSave_OnClick(object sender, RoutedEventArgs e)
        {
            OnSave?.Invoke(Settings);
            this.Close();
        }

        private void btnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Open Letter spliter's dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSpliter_OnClick(object sender, RoutedEventArgs e)
        {
            new LetterSpliter(OptionsSettings.Settings.LetterSpliters).ShowDialog();
        }

        /// <summary>
        /// Open a folder browser dialog to select directory for translate cache
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChange_OnClick(object sender, RoutedEventArgs e)
        {
            var fbd = new FolderBrowserDialog
            {
                Description = "Translate cache path",
                SelectedPath = OptionsSettings.Settings.TranslateCachePath,
                ShowNewFolderButton = true
            };
            var showDialogResult = fbd.ShowDialog();
            if (showDialogResult != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            var selectedPath = fbd.SelectedPath;
            if (!System.IO.Directory.Exists(selectedPath))
            {
                return;
            }
            //Do not know why only set one of the other will not change
            Settings.TranslateCachePath = txtTranslateCachePath.Text = selectedPath;
        }

        #region translate service changed
        /// <summary>
        /// When the translate service was changed, to set source language, target language and last language 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbService_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbSourceLanguage.Items.Clear();
            cbTargetLanguage.Items.Clear();
            cbLastLanguage.Items.Clear();
            switch (cbService.SelectedIndex)
            {
                case 0:
                    AppendLang2Control(GoogleTranslator.GetSourceLanguages(), GoogleTranslator.GetTargetLanguages());
                    SetLanguageSelectedIndex(Settings.GoogleSettings);
                    break;
                case 1:
                    AppendLang2Control(BingTranslator.GetSourceLanguages(), BingTranslator.GetTargetLanguages());
                    SetLanguageSelectedIndex(Settings.BingSettings);
                    break;
                case 2:
                    AppendLang2Control(BaiduTranslator.GetSourceLanguages(), BaiduTranslator.GetTargetLanguages());
                    SetLanguageSelectedIndex(Settings.BaiduSettings);
                    break;
                case 3:
                    AppendLang2Control(YoudaoTranslator.GetSourceLanguages(), YoudaoTranslator.GetTargetLanguages());
                    SetLanguageSelectedIndex(Settings.YoudaoSettings);
                    break;
            }
        }


        private void SetLanguageSelectedIndex(TransSettings transSettings)
        {
            cbSourceLanguage.SelectedIndex = transSettings.SourceLanguageIndex;
            cbTargetLanguage.SelectedIndex = transSettings.TargetLanguageIndex;
            cbLastLanguage.SelectedIndex = transSettings.LastLanguageIndex;
        }

        private void AppendLang2Control(List<TranslationLanguage> sourceLanguages, List<TranslationLanguage> targetLanguages)
        {
            AppendLang2SourceLanguage(sourceLanguages);
            AppendLang2TargetLanguage(targetLanguages);
            AppendLang2LastLanguage(targetLanguages);
        }

        private void AppendLang2SourceLanguage(List<TranslationLanguage> langs)
        {
            foreach (TranslationLanguage translationLanguage in langs)
            {
                cbSourceLanguage.Items.Add(translationLanguage);
            }
        }

        private void AppendLang2TargetLanguage(List<TranslationLanguage> langs)
        {
            foreach (TranslationLanguage translationLanguage in langs)
            {
                cbTargetLanguage.Items.Add(translationLanguage);
            }
        }
        private void AppendLang2LastLanguage(List<TranslationLanguage> langs)
        {
            foreach (TranslationLanguage translationLanguage in langs)
            {
                cbLastLanguage.Items.Add(translationLanguage);
            }
        }
        #endregion

        #region target language changed
        private void cbTargetLanguage_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //TranslationLanguage lang = cbTargetLanguage.SelectedValue as TranslationLanguage;
            //lblLastLanguage.Text = lang?.Name;
            SetTargetLanguageIndex(GetTransSettings());
        }

        private void SetTargetLanguageIndex(TransSettings transSettings)
        {
            if (transSettings != null && cbTargetLanguage.SelectedIndex != transSettings.TargetLanguageIndex && cbTargetLanguage.SelectedIndex != -1)
            {
                transSettings.TargetLanguageIndex = cbTargetLanguage.SelectedIndex;
            }
        } 
        #endregion

        private TransSettings GetTransSettings()
        {
            TransSettings transSettings = null;
            switch (cbService.SelectedIndex)
            {
                case 0:
                    transSettings = Settings.GoogleSettings;
                    break;
                case 1:
                    transSettings = Settings.BingSettings;
                    break;
                case 2:
                    transSettings = Settings.BaiduSettings;
                    break;
                case 3:
                    transSettings = Settings.YoudaoSettings;
                    break;
            }
            return transSettings;
        }

        private void cbSourceLanguage_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetSourceLanguageIndex(GetTransSettings());
        }


        private void SetSourceLanguageIndex(TransSettings transSettings)
        {
            if (transSettings != null && cbSourceLanguage.SelectedIndex != transSettings.SourceLanguageIndex && cbSourceLanguage.SelectedIndex != -1)
            {
                transSettings.SourceLanguageIndex = cbSourceLanguage.SelectedIndex;
            }
        }

        private void cbLastLanguage_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetLastLanguageIndex(GetTransSettings());
        }

        private void SetLastLanguageIndex(TransSettings transSettings)
        {
            if (cbLastLanguage.SelectedIndex != transSettings.LastLanguageIndex && cbLastLanguage.SelectedIndex != -1)
            {
                transSettings.LastLanguageIndex = cbLastLanguage.SelectedIndex;
            }
        }
    }
}
