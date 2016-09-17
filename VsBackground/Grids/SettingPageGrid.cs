using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using VsBackground.Editors;
using VsBackground.Utilities;

namespace VsBackground.Grids
{
    public class SettingPageGrid : DialogPage
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

        private int _loopInterval = 30;

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



        protected override void OnApply(PageApplyEventArgs e)
        {
            MessageBox.Show("Apply");
            base.OnApply(e);
            SettingsChangedEvent?.Invoke(null, null);
        }


        public event EventHandler SettingsChangedEvent;

        public readonly ListHelper Images = new ListHelper { DefaultImage };



    }
}