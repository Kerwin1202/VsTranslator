using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Text;
using Translate.Core.Translator.Enums;
using Translate.Settings;

namespace Visual_Studio_2017_Translator.Adornment.TransResult
{
    /// <summary>
    /// Interaction logic for TranslatorControl.xaml
    /// </summary>
    public partial class TranslatorControl : UserControl
    {

        private TranslatorControl()
        {
            InitializeComponent();

        }
        private static SnapshotSpan _selectedSpans;


        public TranslatorControl(SnapshotSpan selectedSpans, TranslationRequest transRequest)
        {
            InitializeComponent();

            //https://msdn.microsoft.com/zh-cn/library/mt162312.aspx?f=255&MSPPError=-2147217396#%E4%BD%BF%E7%94%A8 Visual Studio %E4%B8%AD%E7%9A%84%E9%A2%9C%E8%89%B2
            VSColorTheme.ThemeChanged += VSColorTheme_ThemeChanged; ;

            RefreshColors();

            _selectedSpans = selectedSpans;
            transRequest.OnTranslationComplete += TransRequest_OnTranslationComplete;

            transRequest.OnAllTranslationComplete += TransRequest_OnAllTranslationComplete;
        }
        private void VSColorTheme_ThemeChanged(ThemeChangedEventArgs e)
        {
            RefreshColors();

            // Also post a message to all the children so they can apply the current theme appropriately  
            //foreach (System.Windows.Forms.Control child in this.Controls)
            //{
            //    NativeMethods.SendMessage(child.Handle, e.Message, IntPtr.Zero, IntPtr.Zero);
            //}
        }

        
        private System.Drawing.Color _bgColor => VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundColorKey);

        private System.Drawing.Color _foreColor => VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowTextColorKey);

        private void RefreshColors()
        {
            //foreach (var rowDefinition in this.grid.RowDefinitions)
            //{
            //    rowDefinition.
            //}

            var bgColor = new SolidColorBrush(System.Windows.Media.Color.FromArgb(_bgColor.A, _bgColor.R, _bgColor.G, _bgColor.B));
            var foreColor = new SolidColorBrush(System.Windows.Media.Color.FromArgb(_foreColor.A, _foreColor.R, _foreColor.G, _foreColor.B));
            btnSettings.Background = btnClose.Background = this.grid.Background = bgColor;
            btnSettings.Foreground = btnClose.Foreground = this.lbltitle.Foreground = foreColor;
        }

        private void TransRequest_OnAllTranslationComplete()
        {
            //Dispatcher.BeginInvoke(new Action(() =>
            //{
            //    lbltitle.Text = "Translation successed..";
            //}));
        }

        /// <summary>
        /// Append translate result text to Translate Control 
        /// </summary>
        /// <param name="translationResult"></param>
        private void TransRequest_OnTranslationComplete(TranslateResult translationResult)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                lbltitle.Text = $"[{translationResult.Identity}]({translationResult.SourceLanguage} - {translationResult.TargetLanguage})";
                if (translationResult.TranslationResultTypes == TranslationResultTypes.Successed)
                {
                    AppendTargetText(translationResult.Identity, translationResult.SourceLanguage, translationResult.TargetLanguage, translationResult.TargetText);
                }
                else
                {
                    lbltitle.Foreground = new SolidColorBrush(Colors.Red);
                    lbltitle.Text = translationResult.FailedReason;
                }
            }));
        }

        private void AppendTargetText(string identity, string sourceLanguage, string targetLanguage, string targetText)
        {
            var wrapPanel = new WrapPanel();

            //System.Windows.Controls.Image image = new System.Windows.Controls.Image
            //{
            //    Source = new BitmapImage(new Uri("pack://application:,,,/Visual Studio 2017 Translator;component/Resources/google_16.ico")),
            //    Width = 14,
            //    Height = 14
            //};
            //wrapPanel.Children.Add(image);


            var label = new TextBlock()
            {
                Text = targetText,
                TextWrapping = TextWrapping.Wrap,
                ToolTip = "Click to replace the selected text with this translation",
                FontWeight = FontWeights.Bold,
                Padding = new Thickness(6, 3, 6, 3),
                MinWidth = 180,
                Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(_bgColor.A, _bgColor.R, _bgColor.G, _bgColor.B)),
                Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(_foreColor.A, _foreColor.R, _foreColor.G, _foreColor.B)),
            };

            label.MouseDown += Label_MouseDown;

            wrapPanel.SetResourceReference(StyleProperty, "MouseOver");
            wrapPanel.Children.Add(label);

            transResult.Children.Add(wrapPanel);
        }

        private void Label_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TextBlock label = sender as TextBlock;
            if (label == null)
            {
                return;
            }
            ReplaceSelectedText(label.Text);
            e.Handled = true;
            RemoveEvent?.Invoke();
        }

        /// <summary>
        /// Replace selected text in vs code editor when click the translate result text
        /// </summary>
        /// <param name="targetText"></param>
        public static void ReplaceSelectedText(string targetText)
        {
            var span = _selectedSpans.Snapshot.CreateTrackingSpan(_selectedSpans, SpanTrackingMode.EdgeExclusive);
            ITextBuffer buffer = span.TextBuffer;
            var sp = span.GetSpan(buffer.CurrentSnapshot);
            buffer.Replace(sp, targetText);
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            //Global.Dte.ExecuteCommand("Tools.Options", GuidList.TranslateOptions);
            OptionsSettings.ShowOptions();
        }

        public Action RemoveEvent;

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            VSColorTheme.ThemeChanged -= this.VSColorTheme_ThemeChanged;
            RemoveEvent?.Invoke();
        }
    }
}
