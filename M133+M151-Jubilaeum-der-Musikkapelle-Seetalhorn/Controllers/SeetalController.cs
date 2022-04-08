using M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Data;
using M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Models;
using Microsoft.AspNetCore.Mvc;

namespace M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Controllers
{
    public class SeetalController : Controller
    {
        private readonly ApplicationDbContext _db;
        public SeetalController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Competition()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Competition(Seetalhorn obj)
        {
            int points = 0;
            if (obj.Antwort1 != "74")
            {
            }
            else
            {
                points++;
            }
            if(obj.Antwort2 != "ca. 4300 kg")
            {

            }
            else
            {
                points++;
            }
            if(obj.Antwort3 != "Trompete")
            {

            }
            else
            {
                points++;
            }
            if(obj.Antwort4 != "50")
            {

            }
            else
            {
                points++;
            }
            if(obj.Antwort5 != "19.088 km")
            {

            }
            else
            {
                points++;
            }
            obj.Punkte = points;

            _db.Seetalhorn.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
