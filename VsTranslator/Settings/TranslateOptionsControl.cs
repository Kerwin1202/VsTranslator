using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace VsTranslator.Settings
{
    /// <summary>
    /// Translating setting 
    /// </summary>
    public partial class TranslateOptionsControl : UserControl
    {
        internal TranslateOptions TranslateOptions;

        public TranslateOptionsControl()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Swaps the source and target languages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSwap_Click(object sender, EventArgs e)
        {

        }

        public void Initialize()
        {
            
        }

        private void lbBaidu_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://api.fanyi.baidu.com/api/trans/product/index");
        }

        private void lbBing_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://datamarket.azure.com/developer/applications/");
        }
    }
}
