using System.Diagnostics;
using M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Data;
using M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Models;
using M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Services;
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
        private readonly IEmailSender _emailSender;
        private readonly ILogger<SeetalController> _logger;

        public SeetalController(
            ApplicationDbContext db,
            IEmailSender emailSender,
            ILogger<SeetalController> logger)
        {
            _db = db;
            _emailSender = emailSender;
            _logger = logger;
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
        public async Task<IActionResult> Competition(Seetalhorn obj)
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

            // Bestaetigungsmail versenden. Ein Fehler darf die Teilnahme nicht
            // verhindern, deshalb wird er nur protokolliert.
            try
            {
                var body =
                    "Vielen Dank für deine Teilnahme am Jubiläums-Quiz der " +
                    "Musikkapelle Seetalhorn!\n\n" +
                    $"Du hast {points} von {CorrectAnswers.Length} Fragen richtig beantwortet.";
                await _emailSender.SendAsync(
                    obj.Email,
                    "Danke für deine Teilnahme am Jubiläums-Quiz",
                    body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Bestätigungsmail an {Email} konnte nicht versendet werden.", obj.Email);
            }

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
