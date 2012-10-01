using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2ZKnowledgeVisualsPvtLtd.ZipNShare.ViewModel
{
    public class ZipExclusionListTypeConverter : TypeConverter
    {
        private List<ZipExclusion> values = null;
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            bool returnValue = base.CanConvertFrom(context, sourceType);
            return returnValue;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }
            bool returnValue = base.CanConvertTo(context, destinationType);
            return returnValue;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                values = new List<ZipExclusion>();
                string[] vals = ((string)value).Split(new char[] { ',' });
                for (int i = 0; i < vals.Length; i++)
                {
                    values.Add((ZipExclusion)(new ZipExclusionTypeConverter()).ConvertFromString(vals[i]));
                }

                return values;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            List<ZipExclusion> values = value as List<ZipExclusion>;
            if (values != null)
            {
                if (context == null)
                {
                    string content = string.Empty;
                    foreach (var item in values)
                    {
                        content += string.Concat(item.ToString(), ",");
                    }
                    content = content.TrimEnd(new char[] { ',' });
                    return content;
                }
                else
                {
                    return string.Format("({0} Items in collection)", ((List<ZipExclusion>)value).Count);
                }
            }
            return "0 Items in collection";
        }
    }
}
