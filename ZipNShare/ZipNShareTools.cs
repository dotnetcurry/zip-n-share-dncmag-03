using A2ZKnowledgeVisualsPvtLtd.ZipNShare.ViewModel;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace A2ZKnowledgeVisualsPvtLtd.ZipNShare
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("C15BE071-55B9-40B8-B2E3-D49E00C75575")]
    public class ZipNShareTools : DialogPage
    {
        private string _outputFolder;
        private List<ZipExclusion> _exclusions;

        public ZipNShareTools()
        {
            _exclusions = new List<ZipExclusion>();
        }

        [TypeConverter(typeof(ZipExclusionListTypeConverter))]
        public List<ZipExclusion> Exceptions
        {
            get { return _exclusions; }
            set { _exclusions = value; }
        }

        [EditorAttribute(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string OutputFolder
        {
            get { return _outputFolder; }
            set { _outputFolder = value; }
        }

        private string _outputFileName;

        [Description("Use %SOLUTION_NAME% to use the respective solution name")]
        public string OutputFileName
        {
            get { return _outputFileName; }
            set { _outputFileName = value; }
        }

        private bool _overwriteIfExists;

        public bool OverwriteZipFileIfExists
        {
            get { return _overwriteIfExists; }
            set { _overwriteIfExists = value; }
        }
    }
}
