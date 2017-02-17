using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VsTranslator.Adornment.TransResult;
using VsTranslator.Core.Translator;
using VsTranslator.Core.Translator.Baidu;
using VsTranslator.Core.Translator.Bing;
using VsTranslator.Core.Translator.Entities;
using VsTranslator.Core.Translator.Enums;
using VsTranslator.Core.Translator.Google;
using VsTranslator.Core.Translator.Youdao;
using VsTranslator.Settings;

namespace VsTranslator.Adornment.Translate
{
    /// <summary>
    /// TranslateClient.xaml 的交互逻辑
    /// </summary>
    public partial class TranslateClient : Window
    {
        public TranslateClient(Settings.Settings settings)
        {
            _settings = settings;
            InitializeComponent();

        }

        private void SetSettingText(List<TranslationLanguage> sourLanguages, List<TranslationLanguage> targetLanguages, TransSettings settings)
        {
            if (lblSettingText==null)
            {
                // when .ctor call InitializeComponent method, this method will be call at the same time, because call rbTranslateType_Checked method  before call win_Loaded method, this time lblSettingText is null
                return;
            }

            //when source language is empty , source language is auto in bing
            var sourceLanguage = sourLanguages[settings.SourceLanguageIndex].Code.ToUpper();

            var from = string.IsNullOrWhiteSpace(sourceLanguage) ? "AUTO" : sourceLanguage;

            lblSettingText.Text = from.ToUpper() + " -> "+
                                 targetLanguages[settings.TargetLanguageIndex].Code.ToUpper() + " -> " +
                                 targetLanguages[settings.LastLanguageIndex].Code.ToUpper();
        }

        private void win_Loaded(object sender, RoutedEventArgs e)
        {
            //Topmost = true;

            cbTop.DataContext = this;

            SetSettingText(GoogleTranslator.GetSourceLanguages(), GoogleTranslator.GetTargetLanguages(), _settings.GoogleSettings);

            StartListenerClipboard();
        }

        private void win_Closed(object sender, EventArgs eventArgs)
        {
            CloseClipboardListener();
        }

        private Settings.Settings _settings;

        /// <summary>
        /// The work thread of translate
        /// </summary>
        private Thread _workThread;

        /// <summary>
        /// The thread of to set the status bar text to be ready
        /// </summary>
        private Thread _readyThread;
        /// <summary>
        /// 
        /// </summary>
        private TranslateType _translateType = TranslateType.Google;

        /// <summary>
        /// Next clipboard viewer window 
        /// </summary>
        private IntPtr _hWndNextViewer;
        /// <summary>
        /// The <see cref="HwndSource"/> for this window.
        /// </summary>
        private HwndSource _hWndSource;


        private void StartListenerClipboard()
        {
            WindowInteropHelper wih = new WindowInteropHelper(this);
            _hWndSource = HwndSource.FromHwnd(wih.Handle);
            if (_hWndSource != null)
            {
                _hWndSource.AddHook(this.WinProc);   // start processing window messages
                _hWndNextViewer = Win32.SetClipboardViewer(_hWndSource.Handle);   // set this window as a viewer
            }
        }

        private void CloseClipboardListener()
        {
            Win32.ChangeClipboardChain(_hWndSource.Handle, _hWndNextViewer);
            _hWndNextViewer = IntPtr.Zero;
            _hWndSource.RemoveHook(this.WinProc);
        }

        private IntPtr WinProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case Win32.WM_CHANGECBCHAIN:
                    if (wParam == _hWndNextViewer)
                    {
                        // clipboard viewer chain changed, need to fix it.
                        _hWndNextViewer = lParam;
                    }
                    else if (_hWndNextViewer != IntPtr.Zero)
                    {
                        // pass the message to the next viewer.
                        Win32.SendMessage(_hWndNextViewer, msg, wParam, lParam);
                    }
                    break;

                case Win32.WM_DRAWCLIPBOARD:
                    // clipboard content changed
                    SetPasteEnabled(!string.IsNullOrWhiteSpace(Clipboard.GetText()));
                    // pass the message to the next viewer.
                    Win32.SendMessage(_hWndNextViewer, msg, wParam, lParam);
                    break;
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// To set the enable state of the paste button
        /// </summary>
        /// <param name="isEnabled"></param>
        private void SetPasteEnabled(bool isEnabled)
        {
            btnPaste.IsEnabled = isEnabled;
            btnPaste.Opacity = isEnabled ? 1 : 0.5;
        }

        /// <summary>
        /// Clear the target text and set the enabled status of copy button to be disabled
        /// </summary>
        private void ClearTargetText()
        {
            txtTarget.Text = "";
            btnCopy.IsEnabled = false;
        }

        private void btnNew_OnClick(object sender, RoutedEventArgs e)
        {
            AbortWrokThread();
            txtSource.Text = "";
            ClearTargetText();
        }

        private void btnCopy_OnClick(object sender, RoutedEventArgs e)
        {
            string targetText = txtTarget.Text;
            if (!string.IsNullOrWhiteSpace(targetText))
            {
                Clipboard.SetText(txtTarget.Text);
            }
        }

        private void btnPaste_OnClick(object sender, RoutedEventArgs e)
        {
            AbortWrokThread();
            txtSource.Text = Clipboard.GetText();
            PreparationAndTranslation(txtSource.Text);
        }

        private void btnPrev_OnClick(object sender, RoutedEventArgs e)
        {
            AbortWrokThread();

        }

        private void btnNext_OnClick(object sender, RoutedEventArgs e)
        {
            AbortWrokThread();

        }

        private void btnSettings_OnClick(object sender, RoutedEventArgs e)
        {
            Topmost = false;
            OptionsSettings.ShowOptions();
        }

        private void btnTranslate_OnClick(object sender, RoutedEventArgs e)
        {
            var sourceText = txtSource.Text;
            PreparationAndTranslation(sourceText);
        }


        private void PreparationAndTranslation(string sourceText)
        {
            AbortWrokThread();
            lblStatus.Text = "Translating...";
            ClearTargetText();
            _workThread = new Thread(() =>
            {
                try
                {
                    Translate(sourceText);
                }
                catch (Exception exception)
                {
                    SetTargetText(exception.Message);
                }
            })
            { IsBackground = true };
            _workThread.Start();
        }

        private void Translate(string sourceText)
        {
            int cmdId = (int)_translateType;
            var translator = TranslatorFactory.GetTranslator(cmdId);
            TranslationRequest transRequest = new TranslationRequest(sourceText, new List<Trans>()
            {
                new Trans()
                {
                    Translator = translator,
                    SourceLanguage = TranslatorFactory.GetSourceLanguage(cmdId, sourceText),
                    TargetLanguage = TranslatorFactory.GetTargetLanguage(cmdId, sourceText),
                }
            });
            transRequest.OnTranslationComplete += TransRequest_OnTranslationComplete;
            transRequest.OnAllTranslationComplete += TransRequest_OnAllTranslationComplete;
        }

        /// <summary>
        /// Abort the translate work thread
        /// </summary>
        private void AbortWrokThread()
        {
            _workThread?.Abort();
        }


        private void rbTranslateType_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (string.IsNullOrWhiteSpace(radioButton?.Tag.ToString()))
            {
                return;
            }
            _translateType = (TranslateType)Enum.Parse(typeof(TranslateType), radioButton.Tag.ToString());

            switch (_translateType)
            {
                case TranslateType.Google:
                    SetSettingText(GoogleTranslator.GetSourceLanguages(), GoogleTranslator.GetTargetLanguages(), _settings.GoogleSettings);
                    break;
                case TranslateType.Bing:
                    SetSettingText(BingTranslator.GetSourceLanguages(), BingTranslator.GetTargetLanguages(), _settings.BingSettings);
                    break;
                case TranslateType.Baidu:
                    SetSettingText(BaiduTranslator.GetSourceLanguages(), BaiduTranslator.GetTargetLanguages(), _settings.BaiduSettings);
                    break;
                case TranslateType.Youdao:
                    SetSettingText(YoudaoTranslator.GetSourceLanguages(), YoudaoTranslator.GetTargetLanguages(), _settings.YoudaoSettings);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void TransRequest_OnAllTranslationComplete()
        {
            SetStatusText("Translate successed...");
            SetReady();
        }

        /// <summary>
        /// To set the text of statusbar
        /// </summary>
        /// <param name="text"></param>
        private void SetStatusText(string text)
        {
            Dispatcher.BeginInvoke(new Action(() => { lblStatus.Text = text; }));
        }

        /// <summary>
        /// To set the text to be ready of the statusbar
        /// </summary>
        private void SetReady()
        {
            _readyThread = new Thread(() =>
            {
                Thread.Sleep(3000);
                SetStatusText("ready...");
            })
            { IsBackground = true };
            _readyThread.Start();
        }

        private void TransRequest_OnTranslationComplete(TranslateResult translateResult)
        {
            SetTargetText(translateResult.TargetText);
        }

        /// <summary>
        /// Set target text and set the copy button to be enabled and transparency is 1
        /// </summary>
        /// <param name="targetText"></param>
        private void SetTargetText(string targetText)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                txtTarget.Text = targetText;
                if (!string.IsNullOrWhiteSpace(txtTarget.Text))
                {
                    btnCopy.IsEnabled = true;
                    btnCopy.Opacity = 1;
                }
            }));
        }
    }

}
