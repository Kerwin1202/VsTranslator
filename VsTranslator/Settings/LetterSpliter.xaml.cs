using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;

namespace VsTranslator.Settings
{
    /// <summary>
    /// Interaction logic for LetterSpliter.xaml
    /// </summary>
    public partial class LetterSpliter : Window
    {
        private static readonly ObservableCollection<Spliter> Spliters = new ObservableCollection<Spliter>();

        public LetterSpliter(IList<Spliter> spliters)
        {
            InitializeComponent();
            Spliters.Clear();
            foreach (var spliter in spliters)
            {
                Spliters.Add(spliter);
            }

            dgLetterSpliters.ItemsSource = Spliters;
        }

        private void btnTest_OnClick(object sender, RoutedEventArgs e)
        {
            var matchRegex = txtMatchRegex.Text;
            var replaceRegex = txtReplaceRegex.Text;
            var example = txtExample.Text;

            try
            {
                var replaceResult = new Regex(matchRegex).Replace(example, replaceRegex);
                ShowOKMsgBox($"Test result:\"{replaceResult}\"");
            }
            catch (Exception)
            {
                ShowErrorMsgBox("Test failed");
            }
        }

        private void ShowWarningMsgBox(string message)
        {
            MessageBox.Show(message, "Tip", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void ShowErrorMsgBox(string message)
        {
            MessageBox.Show(message, "Tip", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void ShowOKMsgBox(string message)
        {
            MessageBox.Show(message, "Tip", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            var matchRegex = txtMatchRegex.Text;
            var replaceRegex = txtReplaceRegex.Text;
            var example = txtExample.Text;
            Spliters.Add(new Spliter()
            {
                MatchRegex = matchRegex,
                Example = example,
                ReplaceRegex = replaceRegex
            });

        }

        private void btnSave_OnClick(object sender, RoutedEventArgs e)
        {
            OptionsSettings.SaveSpliters(Spliters);
            this.Close();
        }

        private void btnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dgLetterSpliters_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedIndex = dgLetterSpliters.SelectedIndex;
            if (selectedIndex <= -1 || selectedIndex >= Spliters.Count) return;
            txtMatchRegex.Text = Spliters[selectedIndex].MatchRegex;
            txtReplaceRegex.Text = Spliters[selectedIndex].ReplaceRegex;
            txtExample.Text = Spliters[selectedIndex].Example;
        }
    }
}
