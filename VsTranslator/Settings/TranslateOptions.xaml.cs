using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VsTranslator.Settings
{
    /// <summary>
    /// Interaction logic for TranslateOptionsControl2.xaml
    /// </summary>
    public partial class TranslateOptions : Window
    {
        public TranslateOptions()
        {
            InitializeComponent();
        }

        private void lblBaidu_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("http://api.fanyi.baidu.com/api/trans/product/index");
        }

        private void lblBing_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://datamarket.azure.com/developer/applications/");
        }
    }
}
