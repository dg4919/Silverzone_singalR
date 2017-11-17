using System.Web.Mvc;

namespace Silverzone.Web.Areas.TeacherApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: TeacherApp/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}