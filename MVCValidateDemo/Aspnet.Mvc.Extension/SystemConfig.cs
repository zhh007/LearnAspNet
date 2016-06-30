using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspnet.Mvc.Extension
{
    public class SystemConfig
    {
        public static readonly string FileStoreRootPath = ConfigurationManager.AppSettings["System:FileStoreRootPath"];
    }
}
