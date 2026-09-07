using System.Diagnostics;
using M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Data;
using M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Models;
using Microsoft.AspNetCore.Mvc;

namespace M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Controllers
{
    public class SeetalController : Controller
    {
        // Korrekte Antworten in der Reihenfolge Antwort1..Antwort5.
        // Die Werte muessen mit den value-Attributen der Radio-Buttons
        // in Views/Seetal/Competition.cshtml uebereinstimmen.
        private static readonly string[] CorrectAnswers =
        {
            "74", "ca. 4300 kg", "Trompete", "50", "19.088 km"
        };

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
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            var emailExists = _db.Seetalhorn.Any(x => x.Email == obj.Email);
            if (emailExists)
            {
                ModelState.AddModelError(nameof(obj.Email),
                    "Mit dieser E-Mail Adresse wurde bereits teilgenommen.");
                return View(obj);
            }

            var answers = new[]
            {
                obj.Antwort1, obj.Antwort2, obj.Antwort3, obj.Antwort4, obj.Antwort5
            };
            int points = answers.Where((answer, i) => answer == CorrectAnswers[i]).Count();
            obj.Punkte = points;

            _db.Seetalhorn.Add(obj);
            _db.SaveChanges();

            TempData["Punkte"] = points;
            return RedirectToAction(nameof(Result));
        }

        public IActionResult Result()
        {
            if (TempData["Punkte"] is not int points)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Punkte = points;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View(new ErrorViewModel { RequestId = requestId });
        }
    }
}
