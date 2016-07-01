using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Aspnet.Mvc.Extension
{
    public class PreApplicationStartCode
    {
        // Fields
        private static bool _startWasCalled;

        // Methods
        public static void Start()
        {
            if (!_startWasCalled)
            {
                _startWasCalled = true;

                RegisterRoutes(RouteTable.Routes);

                //ModelBinders.Binders.Add(typeof(FileUploadComponentModel), new FileUploadComponentModelBinder());

                ModelBinderProviders.BinderProviders.Add(new FileUploadModelBinderProvider());
            }
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "fileuploader",
                "__fileuploader/{action}",
                new { controller = "FileUpload" },
                new[] { "Aspnet.Mvc.Extension.Controllers" }
                );
        }
    }
}
