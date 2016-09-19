//------------------------------------------------------------------------------
// <copyright file="CodeViewIDE.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using VsBackground.Enums;

namespace VsBackground.Services
{
    /// <summary>
    /// CodeViewIDE places red boxes behind all the "a"s in the editor window
    /// </summary>
    internal sealed class CodeViewIde
    {
        /// <summary>
        /// The layer of the adornment.
        /// </summary>
        private readonly IAdornmentLayer _layer;

        /// <summary>
        /// Text view where the adornment is created.
        /// </summary>
        private readonly IWpfTextView _view;

        /// <summary>
        /// Adornment brush.
        /// </summary>
        private readonly Brush _brush;

        /// <summary>
        /// Adornment pen.
        /// </summary>
        private readonly Pen _pen;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeViewIde"/> class.
        /// </summary>
        /// <param name="view">Text view to create the adornment for</param>
        public CodeViewIde(IWpfTextView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            this._layer = view.GetAdornmentLayer("CodeViewIDE");

            this._view = view;
            this._view.LayoutChanged += this.OnLayoutChanged;

            // Create the pen and brush to color the box behind the a's
            this._brush = new SolidColorBrush(Color.FromArgb(0x20, 0x00, 0x00, 0xff));
            this._brush.Freeze();

            var penBrush = new SolidColorBrush(Colors.Red);
            penBrush.Freeze();
            this._pen = new Pen(penBrush, 0.5);
            this._pen.Freeze();
        }
        private readonly Canvas _imageCanvas = new Canvas() { IsHitTestVisible = true };
        /// <summary>
        /// Handles whenever the text displayed in the view changes by adding the adornment to any reformatted lines
        /// </summary>
        /// <remarks><para>This event is raised whenever the rendered text displayed in the <see cref="ITextView"/> changes.</para>
        /// <para>It is raised whenever the view does a layout (which happens when DisplayTextLineContainingBufferPosition is called or in response to text or classification changes).</para>
        /// <para>It is also raised whenever the view scrolls horizontally or when its size changes.</para>
        /// </remarks>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        internal void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            if (Settings.Settings.SettingPage.BackgroundType == BackgroundType.CodeView || Settings.Settings.SettingPage.BackgroundType == BackgroundType.Both)
            {
                var rImageSource = BitmapFrame.Create(new Uri(Settings.Settings.SettingPage.Images.Current, UriKind.RelativeOrAbsolute), BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                rImageSource.Freeze();

                _imageCanvas.Background = new ImageBrush(rImageSource)
                {
                    Opacity = Settings.Settings.SettingPage.Opacity,
                    Stretch = Stretch.UniformToFill,
                    AlignmentX = AlignmentX.Right,
                    AlignmentY = AlignmentY.Bottom
                };
                Canvas.SetLeft(_imageCanvas, _view.ViewportWidth - rImageSource.Width);
                Canvas.SetTop(_imageCanvas, _view.ViewportTop);
                // Canvas.SetRight(_editorCanvas, _view.ViewportRight - _view.ViewportWidth);

                _imageCanvas.Width = rImageSource.Width;
                _imageCanvas.Height = rImageSource.Height;
                _layer.RemoveAdornmentsByTag("CodeViewIDE");//不加这句代码下一行会报异常
                this._layer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, "CodeViewIDE", _imageCanvas, null);

            }
            //foreach (ITextViewLine line in e.NewOrReformattedLines)
            //{
            //    this.CreateVisuals(line);
            //}
        }

        /// <summary>
        /// Adds the scarlet box behind the 'a' characters within the given line
        /// </summary>
        /// <param name="line">Line to add the adornments</param>
        private void CreateVisuals(ITextViewLine line)
        {
            IWpfTextViewLineCollection textViewLines = this._view.TextViewLines;

            // Loop through each character, and place a box around any 'a'
            for (int charIndex = line.Start; charIndex < line.End; charIndex++)
            {
                if (this._view.TextSnapshot[charIndex] == 'a')
                {
                    SnapshotSpan span = new SnapshotSpan(this._view.TextSnapshot, Span.FromBounds(charIndex, charIndex + 1));
                    Geometry geometry = textViewLines.GetMarkerGeometry(span);
                    if (geometry != null)
                    {
                        var drawing = new GeometryDrawing(this._brush, this._pen, geometry);
                        drawing.Freeze();

                        var drawingImage = new DrawingImage(drawing);
                        drawingImage.Freeze();

                        var image = new Image
                        {
                            Source = drawingImage,
                        };

                        // Align the image with the top of the bounds of the text geometry
                        Canvas.SetLeft(image, geometry.Bounds.Left);
                        Canvas.SetTop(image, geometry.Bounds.Top);

                        this._layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, span, null, image, null);
                    }
                }
            }
        }
    }
}
