using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2ZKnowledgeVisualsPvtLtd.ZipNShare.ViewModel
{
    [Serializable]
    [TypeConverter(typeof(ZipExclusionTypeConverter))]
    public class ZipExclusion
    {
        public string Expression { get; set; }
        public ExclusionType ExclusionType { get; set; }

        public override string ToString()
        {
            return ExclusionType.ToString() + ":" + Expression;
        }
    }
}
