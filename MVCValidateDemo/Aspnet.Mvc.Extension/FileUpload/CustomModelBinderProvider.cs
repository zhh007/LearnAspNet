using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Threading;

namespace Aspnet.Mvc.Extension
{
    public class CustomModelBinderProvider : IModelBinderProvider
    {
        private static ReaderWriterLockSlim _lock;
        static CustomModelBinderProvider()
        {
            _lock = new ReaderWriterLockSlim();
        }

        Dictionary<Type, IModelBinder> dict = new Dictionary<Type, IModelBinder>();

        FileUploadComponentModelBinder cmb = new FileUploadComponentModelBinder();
        public CustomModelBinderProvider()
        {
        }

        public IModelBinder GetBinder(Type modelType)
        {
            IModelBinder result = null;
            if (dict.ContainsKey(modelType))
            {
                result = dict[modelType];
            }
            else
            {
                _lock.EnterWriteLock();
                try
                {
                    foreach (var property in modelType.GetProperties())
                    {
                        if (property.PropertyType == typeof(FileUploadComponentModel))
                        {
                            result = cmb;
                            break;
                        }
                    }
                    dict[modelType] = result;
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
            return result;
        }
    }
}
