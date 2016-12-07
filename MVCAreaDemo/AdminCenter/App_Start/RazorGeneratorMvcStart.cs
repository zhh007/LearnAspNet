using AdminCenter;
using RazorGenerator.Mvc;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using WebActivatorEx;

[assembly: PostApplicationStartMethod(typeof(RazorGeneratorMvcStart), "Start")]
namespace AdminCenter
{
    public static class RazorGeneratorMvcStart
    {
        public static void Start()
        {
            PrecompiledMvcEngine engine = new PrecompiledMvcEngine(typeof(RazorGeneratorMvcStart).Assembly, "~/Areas/AdminCenter")
            {
                UsePhysicalViewsIfNewer = HttpContext.Current.Request.IsLocal
            };
            ViewEngines.Engines.Insert(0, engine);
            VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);
        }
    }
}