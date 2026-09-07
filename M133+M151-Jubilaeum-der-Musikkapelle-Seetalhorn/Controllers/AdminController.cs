using System.Security.Claims;
using M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Data;
using M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Models;
using M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Controllers
{
    public class AdminController : Controller
    {
        // Volle Punktzahl = Anzahl Quizfragen. Nur wer alle richtig hat,
        // kommt in den Lostopf.
        private const int FullScore = 5;

        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;

        public AdminController(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var adminPassword = _config["AdminSettings:Password"];
            if (string.IsNullOrEmpty(adminPassword) || model.Password != adminPassword)
            {
                ModelState.AddModelError(string.Empty, "Falsches Passwort.");
                return View(model);
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Admin") };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            return RedirectToAction(nameof(Participants));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        public IActionResult Participants()
        {
            var participants = _db.Seetalhorn
                .OrderByDescending(x => x.Punkte)
                .ThenBy(x => x.Id)
                .ToList();
            return View(participants);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Draw()
        {
            var eligible = _db.Seetalhorn
                .Where(x => x.Punkte == FullScore)
                .ToList();

            var winner = WinnerDrawing.DrawWinner(eligible, Random.Shared);
            if (winner == null)
            {
                TempData["DrawMessage"] = "Keine Teilnehmenden mit voller Punktzahl vorhanden.";
                return RedirectToAction(nameof(Participants));
            }

            // Bisherige Gewinner zuruecksetzen, dann den neuen markieren
            // (es gibt hoechstens einen Gewinner).
            foreach (var previous in _db.Seetalhorn.Where(x => x.IsWinner).ToList())
            {
                previous.IsWinner = false;
            }
            winner.IsWinner = true;
            _db.SaveChanges();

            TempData["DrawMessage"] = $"Neuer Gewinner gezogen: {winner.Email}";
            return RedirectToAction(nameof(Participants));
        }
    }
}
