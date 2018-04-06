using System.Web.Mvc;

namespace Mmmxa.So.Controllers
{
    public class ConfigManagerController : Controller
    {
        // GET: ConfigManager
        public ActionResult Index()
        {
            return View();
        }

        // GET: ConfigManager/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ConfigManager/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ConfigManager/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ConfigManager/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ConfigManager/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ConfigManager/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ConfigManager/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
