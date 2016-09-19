using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Markup;
using Microsoft.VisualStudio.Shell;
using VsBackground.Editors;
using VsBackground.Enums;
using VsBackground.Interfaces;
using VsBackground.Utilities;

namespace VsBackground.Grids
{
    public class SettingPageGrid : DialogPage, ISettings
    {
        private const string DefaultImage = "Images\\defaultImage.jpg";

        private string _bgImagePath = DefaultImage;

        /// <summary>
        /// 背景图路径
        /// </summary>
        [Category("Images")]
        [DisplayName("Image path")]
        [Description("the directory of background image")]
        [Editor(typeof(BrowseFile), typeof(UITypeEditor))]
        public string BgImagePath
        {
            get { return _bgImagePath; }
            set
            {
                if (!value.Contains("|"))
                {
                    if (System.IO.File.Exists(value))
                    {
                        Images.Clear();
                        _bgImagePath = value;
                        Images.Add(_bgImagePath);
                    }
                }
                else
                {
                    _bgImagePath = "";
                    Images.Clear();
                    foreach (var filePath in value.Split('|'))
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            _bgImagePath += filePath + "|";
                            Images.Add(filePath);
                        }
                    }
                    if (_bgImagePath.Length > 0)
                    {
                        _bgImagePath = _bgImagePath.Substring(0, _bgImagePath.Length - 1);
                    }
                }
            }
        }


        private double _opacity = 0.5;

        /// <summary>
        /// 透明度
        /// </summary>
        [Category("Settings")]
        [DisplayName("Opacity")]
        [Description("the opacity of background image, the value must between 0 and 1(double)")]
        public double Opacity
        {
            get { return _opacity; }
            set
            {
                if (value <= 1 && value >= 0)
                {
                    _opacity = value;
                }
            }
        }

        private int _loopInterval = 300;

        /// <summary>
        /// 循环时间
        /// </summary>
        [Category("Settings")]
        [DisplayName("Loop interval")]
        [Description("the interval of background images loop (second)")]
        public int LoopInterval
        {
            get { return _loopInterval; }
            set
            {
                if (value > 0)
                {
                    _loopInterval = value;
                }
            }
        }

        private BackgroundType _backgroundType = BackgroundType.CodeView;

        [Category("Settings")]
        [DisplayName("Background Type")]
        [Description("the type of background images")]
        //[PropertyPageTypeConverter(typeof(BackgroundTypeConvert))]
        //[TypeConverter(typeof(BackgroundTypeConvert))]
        public BackgroundType BackgroundType
        {
            get { return _backgroundType; }
            set { _backgroundType = value; }
        }



        protected override void OnApply(PageApplyEventArgs e)
        {
            //MessageBox.Show("Apply");
            Debug.WriteLine("Apply");
            base.OnApply(e);
            OnSettingsChanged?.Invoke(null, null);
        }



        public readonly ListHelper Images = new ListHelper { DefaultImage };


        public event EventHandler OnSettingsChanged;



    }
}