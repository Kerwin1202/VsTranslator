//------------------------------------------------------------------------------
// <copyright file="VsBackgroundPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using VsBackground.Grids;
using MessageBox = System.Windows.Forms.MessageBox;

namespace VsBackground
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(VsBackgroundPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideOptionPage(typeof(SettingPageGrid), "Vs IDE", "General", 0, 0, true)]
    [ProvideAutoLoad(UIContextGuids.NoSolution)]//
    [ProvideAutoLoad(UIContextGuids.SolutionExists)]//
    public sealed class VsBackgroundPackage : Package
    {
        /// <summary>
        /// VsBackgroundPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "adab68c6-008e-4283-be85-dd5af15f1c1a";

        /// <summary>
        /// Initializes a new instance of the <see cref="VsBackgroundPackage"/> class.
        /// </summary>
        public VsBackgroundPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.

            _dispatcher = Dispatcher.CurrentDispatcher;
            _timer = new Timer(new TimerCallback(RefreshImage));
        }

        private void RefreshImage(object state)
        {
            SettingPage_SettingsChangedEvent(null, null);
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            Application.Current.MainWindow.Loaded += MainWindow_Loaded;
        }

        #endregion


        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _mainWindow = (Window)sender;
            SettingPage.SettingsChangedEvent += SettingPage_SettingsChangedEvent;
            //RefreshImage();
            Loading();
        }

        private void Loading()
        {
            _timer.Change(0, SettingPage.LoopInterval * 1000);
        }

        private void SettingPage_SettingsChangedEvent(object sender, EventArgs e)
        {
            try
            {
                //_dispatcher.Invoke(RefreshImage);
                Loading();
                GC.Collect();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// 设置页面的实例
        /// </summary>
        public SettingPageGrid SettingPage
        {
            get
            {
                SettingPageGrid page = (SettingPageGrid)GetDialogPage(typeof(SettingPageGrid));
                return page;
            }
        }

        private Window _mainWindow;


        private readonly Timer _timer;
        private readonly Dispatcher _dispatcher;

        public void RefreshImage()
        {
            try
            {
                var curImage = SettingPage.Images.Current;
                if (curImage != null)
                {
                    var rImageSource = BitmapFrame.Create(new Uri(curImage, UriKind.RelativeOrAbsolute), BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    rImageSource.Freeze();
                    var rImageControl = new Image()
                    {
                        Source = rImageSource,
                        Stretch = Stretch.UniformToFill, //按比例填充
                        HorizontalAlignment = HorizontalAlignment.Center, //水平方向中心对齐
                        VerticalAlignment = VerticalAlignment.Center, //垂直方向中心对齐
                        Opacity = SettingPage.Opacity
                    };
                    Grid.SetRowSpan(rImageControl, 4);
                    var rRootGrid = (Grid)_mainWindow.Template.FindName("RootGrid", _mainWindow);
                    rRootGrid.Dispatcher.Invoke(new Action(() =>
                    {
                        rRootGrid.Children.Insert(0, rImageControl);
                    }));
                }
            }
            catch (Exception)
            {
                // ignored
            }
            MessageBox.Show("RefreshImage" + SettingPage.Images.Count);
            SettingPage.Images.MoveNext();
        }

    }
}
