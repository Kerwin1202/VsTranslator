using System;
using System.Collections.Generic;
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
using MessageBox = System.Windows.Forms.MessageBox;

namespace VsTranslator.Adornment
{
    /// <summary>
    /// Interaction logic for TranslatorControl.xaml
    /// </summary>
    public partial class TranslatorControl : UserControl
    {
        public TranslatorControl(string targetText)
        {
            InitializeComponent();
            _targetText = targetText;
            var label = new TextBlock()
            {
                Text = _targetText
            };
            label.SetValue(Grid.RowProperty, 1);
            grid.Children.Add(label);

        }

        public TranslatorControl()
        {
            InitializeComponent();

        }

        private string _targetText;

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
