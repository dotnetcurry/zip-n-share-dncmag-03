using System;
using System.ComponentModel;

namespace A2ZKnowledgeVisualsPvtLtd.ZipNShare.ViewModel
{
    public class ZipExclusionTypeConverter : TypeConverter
    {
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
                ZipExclusion exclusion = new ZipExclusion();
                string[] vals = ((string)value).Split(new char[] { ':' });
                if (vals.Length == 2)
                {
                    exclusion.ExclusionType = vals[0] == "File" ? ExclusionType.File : ExclusionType.Folder;
                    exclusion.Expression = vals[1];
                }
                return exclusion;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

}
