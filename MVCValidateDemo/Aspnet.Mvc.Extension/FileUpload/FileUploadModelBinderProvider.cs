using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Threading;

namespace Aspnet.Mvc.Extension
{
    public class FileUploadModelBinderProvider : IModelBinderProvider
    {
        private static ReaderWriterLockSlim _lock;
        static FileUploadModelBinderProvider()
        {
            _lock = new ReaderWriterLockSlim();
        }

        private Dictionary<Type, IModelBinder> dict = new Dictionary<Type, IModelBinder>();

        private FileUploadModelBinder cmb = new FileUploadModelBinder();
        public FileUploadModelBinderProvider()
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
                try
                {
                    _lock.EnterWriteLock();
                    foreach (var property in modelType.GetProperties())
                    {
                        if (property.PropertyType == typeof(FileUploadModel))
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
