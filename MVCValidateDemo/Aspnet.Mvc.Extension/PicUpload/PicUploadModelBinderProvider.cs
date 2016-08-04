using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Aspnet.Mvc.Extension
{
    public class PicUploadModelBinderProvider : IModelBinderProvider
    {
        private static ReaderWriterLockSlim _lock;
        static PicUploadModelBinderProvider()
        {
            _lock = new ReaderWriterLockSlim();
        }

        private Dictionary<Type, IModelBinder> dict = new Dictionary<Type, IModelBinder>();

        private PicUploadModelBinder cmb = new PicUploadModelBinder();
        public PicUploadModelBinderProvider()
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
                        if (property.PropertyType == typeof(PicUploadModel))
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
