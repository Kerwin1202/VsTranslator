using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Translate.Settings;

namespace Visual_Studio_2017_Translator.Adornment.TransResult
{
    public class TransAdornmentManager
    {

        private static IWpfTextView _view;
        private static IAdornmentLayer _layer;

        private TransAdornmentManager(IWpfTextView view)
        {
            _view = view;
            _layer = view.GetAdornmentLayer("TranslatorAdornmentLayer");

            _view.LayoutChanged += _view_LayoutChanged;

           // _view.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));

            var curBgColor = (_view.Background as SolidColorBrush)?.Color;

            _view.BackgroundBrushChanged += _view_BackgroundBrushChanged;
        }

        private void _view_BackgroundBrushChanged(object sender, BackgroundBrushChangedEventArgs e)
        {
            // triggered when change color theme,, 
            //e.NewBackgroundBrush.Color

            var curBgColor = (_view.Background as SolidColorBrush)?.Color;
        }

        private static void _view_LayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            _layer.RemoveAllAdornments();
        }

        public static TransAdornmentManager Create(IWpfTextView view)
        {
            return view.Properties.GetOrCreateSingletonProperty(() => new TransAdornmentManager(view));
        }

        public static void Add(IWpfTextView view, TranslationRequest transRequest)
        {
            #region 解决有时候 new了之后layer还是为空 以及有时候调用了_layer.add之后却依旧不显示翻译控件的问题
            _view = view;
            _layer = view.GetAdornmentLayer("TranslatorAdornmentLayer");
            _view.LayoutChanged -= _view_LayoutChanged;
            _view.LayoutChanged += _view_LayoutChanged; 
            #endregion

            RemoveAllAdornments();

            var span = view.Selection.SelectedSpans[0].Snapshot.CreateTrackingSpan(view.Selection.SelectedSpans[0], SpanTrackingMode.EdgeExclusive);

            ITextBuffer buffer = view.Selection.TextView.TextBuffer;
            var sp = span.GetSpan(buffer.CurrentSnapshot);
            Geometry g = view.TextViewLines.GetMarkerGeometry(sp);
            if (g != null)
            {
                var tc = new TranslatorControl(view.Selection.SelectedSpans[0], transRequest) { RemoveEvent = RemoveAllAdornments };
                Canvas.SetLeft(tc, g.Bounds.BottomLeft.X);
                Canvas.SetTop(tc, g.Bounds.BottomLeft.Y);

                //
                //_layer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, tc, null);
                Panel.SetZIndex(tc, 1314520);
                _layer.AddAdornment(sp, null, tc);
            }
        }

        public static void RemoveAllAdornments()
        {
            _layer.RemoveAllAdornments();
        }
    }
}