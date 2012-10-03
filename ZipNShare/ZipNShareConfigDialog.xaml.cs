using A2ZKnowledgeVisualsPvtLtd.ZipNShare.ViewModel;
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
using System.Windows.Shapes;

namespace A2ZKnowledgeVisualsPvtLtd.ZipNShare
{
    /// <summary>
    /// Interaction logic for ZipNShareConfigDialog.xaml
    /// </summary>
    public partial class ZipNShareConfigDialog : Window
    {
        private ZipNShareTools toolsPage;


        public ZipNShareConfigDialog()
        {
            InitializeComponent();
        }

        public ZipNShareConfigDialog(ZipNShareTools toolsPage):this()
        {
            this.toolsPage = toolsPage;
            ZipNShareConfigViewModel model = (ZipNShareConfigViewModel)this.DataContext;
            model.PropertyChanged += model_PropertyChanged;
            model.Initialize(toolsPage);
        }

        void model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ZipNShareConfigViewModel model = (ZipNShareConfigViewModel)this.DataContext;
            if (e.PropertyName == "OutputFileName")
            {
                toolsPage.OutputFileName = model.OutputFileName;
            }
            else if (e.PropertyName == "OutputFolder")
            {
                toolsPage.OutputFolder = model.OutputFolder;
            }
            else if (e.PropertyName == "OverwriteZipFileIfExists")
            {
                toolsPage.OverwriteZipFileIfExists = model.OverwriteZipFileIfExists;
            }
            else if (e.PropertyName == "ZipExclusions")
            {
                toolsPage.Exceptions = new List<ZipExclusion>(model.ZipExclusions);
            }
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            toolsPage.SaveSettingsToStorage();
            this.DialogResult = true;
        }

        private void SaveAndRunButtonClick(object sender, RoutedEventArgs e)
        {
            toolsPage.SaveSettingsToStorage();
            this.TriggerRun = true;
            this.DialogResult = true;
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            ZipNShareConfigViewModel model = (ZipNShareConfigViewModel)this.DataContext;

            if (ExclusionTypeComboBox.SelectedIndex < 0 ||
                ExclusionExpressionTextBox.Text == string.Empty)
            {
                MessageBox.Show(@"Please Select and ExclusionType and provide and ExclusionExpression 
                    (e.g. ExclusionType=File, ExclusionExpression=.suo)", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                model.ZipExclusions.Add(new ZipExclusion 
                {
                    ExclusionType = (ExclusionType)ExclusionTypeComboBox.SelectedIndex, 
                    Expression = ExclusionExpressionTextBox.Text 
                });
                toolsPage.Exceptions = new List<ZipExclusion>(model.ZipExclusions);
            }
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            ZipNShareConfigViewModel model = (ZipNShareConfigViewModel)this.DataContext;
            if (ExclusionsListBox.SelectedItem != null)
            {
                model.ZipExclusions.Remove((ZipExclusion)ExclusionsListBox.SelectedItem);
                toolsPage.Exceptions = new List<ZipExclusion>(model.ZipExclusions);
            }
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void SelectOutputFolderButtonClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
            dlg.ShowNewFolderButton = true;
            System.Windows.Forms.DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                ZipNShareConfigViewModel model = (ZipNShareConfigViewModel)this.DataContext;
                model.OutputFolder = dlg.SelectedPath;                
            }
        }

        public bool TriggerRun { get; set; }
    }
}
