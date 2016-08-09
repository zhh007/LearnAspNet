using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspnet.Mvc.Extension
{
    internal class Helper
    {
        internal static T GetAttribute<T>(Type modelType, string propertyName)
            where T : Attribute
        {
            var att = TypeDescriptor.GetProperties(modelType)[propertyName].Attributes[typeof(T)] as T;
            if (att != null)
            {
                return att;
            }

            return null;
        }
    }
}
