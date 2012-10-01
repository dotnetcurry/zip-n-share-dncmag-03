using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2ZKnowledgeVisualsPvtLtd.ZipNShare.ViewModel
{
    public class ZipNShareConfigViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ZipNShareConfigViewModel()
        {

        }

        private void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ObservableCollection<ZipExclusion> _zipExclusions;
        private string _outputFolder;
        private string _outputFileName;
        private bool _overwriteIfExists;

        public string OutputFileName
        {
            get { return _outputFileName; }
            set
            {
                _outputFileName = value;
                RaisePropertyChanged("OutputFileName");
            }
        }

        public bool OverwriteZipFileIfExists
        {
            get { return _overwriteIfExists; }
            set
            {
                _overwriteIfExists = value;
                RaisePropertyChanged("OverwriteZipFileIfExists");
            }
        }

        public string OutputFolder
        {
            get { return _outputFolder; }
            set
            {
                _outputFolder = value;
                RaisePropertyChanged("OutputFolder");
            }
        }

        public ObservableCollection<ZipExclusion> ZipExclusions
        {
            get { return _zipExclusions; }
            set
            {
                if (value != null)
                {
                    _zipExclusions = value;
                    RaisePropertyChanged("ZipExclusions");
                }
            }
        }

        ZipNShareTools tools;

        internal void Initialize(ZipNShareTools toolsPage)
        {
            this.OutputFileName = toolsPage.OutputFileName;
            this.OutputFolder = toolsPage.OutputFolder;
            this.OverwriteZipFileIfExists = toolsPage.OverwriteZipFileIfExists;
            this.ZipExclusions = new ObservableCollection<ZipExclusion>(toolsPage.Exceptions);
        }
    }
}
