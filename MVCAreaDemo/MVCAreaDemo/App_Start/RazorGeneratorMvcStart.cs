//using System.Web;
//using System.Web.Mvc;
//using System.Web.WebPages;
//using RazorGenerator.Mvc;

//[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(WebArea1.RazorGeneratorMvcStart), "Start")]

//namespace MVCAreaDemo
//{
//    public static class RazorGeneratorMvcStart {
//        public static void Start() {
//            var engine = new CompositePrecompiledMvcEngine(typeof(RazorGeneratorMvcStart).Assembly) {
//                UsePhysicalViewsIfNewer = HttpContext.Current.Request.IsLocal
//            };

//            ViewEngines.Engines.Insert(0, engine);
//            //ViewEngines.Engines.Add(engine);

//            // StartPage lookups are done by WebPages. 
//            VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);
//        }
//    }
//}
