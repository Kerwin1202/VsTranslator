using System.Windows;
using System.Windows.Controls;
using MessageBox = System.Windows.Forms.MessageBox;

namespace VsTranslator.Ouput
{
    /// <summary>
    /// Interaction logic for OutputControl.xaml
    /// </summary>
    public partial class OutputControl : UserControl
    {
        public OutputControl()
        {
            InitializeComponent();
            this.Width = base.Width;
            
        }
    }
}
