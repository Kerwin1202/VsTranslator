using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using MessageBox = System.Windows.Forms.MessageBox;

namespace VsTranslator.Adornment
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
        }

        private static void _view_LayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            // _layer.RemoveAllAdornments();
        }

        public static TransAdornmentManager Create(IWpfTextView view)
        {
            return view.Properties.GetOrCreateSingletonProperty(() => new TransAdornmentManager(view));
        }

        public static void Add(IWpfTextView view, string targetText)
        {
            if (_layer == null)
            {
                _view = view;
                _layer = view.GetAdornmentLayer("TranslatorAdornmentLayer");

                _view.LayoutChanged += _view_LayoutChanged;
            }
            _layer.RemoveAllAdornments();
            var span = view.Selection.SelectedSpans[0].Snapshot.CreateTrackingSpan(view.Selection.SelectedSpans[0], SpanTrackingMode.EdgeExclusive);
            ITextBuffer buffer = view.Selection.TextView.TextBuffer;
            var sp = span.GetSpan(buffer.CurrentSnapshot);
            Geometry g = view.TextViewLines.GetMarkerGeometry(sp);
            if (g != null)
            {
                var tc = new TranslatorControl(targetText);
                Canvas.SetLeft(tc, g.Bounds.BottomLeft.X);
                Canvas.SetTop(tc, g.Bounds.BottomLeft.Y);
                _layer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, tc, null);

            }
        }
    }
}