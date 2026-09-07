# Admin-Bereich mit Teilnehmerliste und Gewinner-Ziehung

## Ziel
Die Musikkapelle soll die eingegangenen Wettbewerbs-Teilnahmen einsehen und
unter den Teilnehmenden mit voller Punktzahl einen Gewinner auslosen können.
Der Zugang ist durch ein gemeinsames Admin-Passwort geschützt.

## Umfang (Scope)
Enthalten:
- Cookie-basierte Anmeldung mit einem gemeinsamen Admin-Passwort.
- Geschützte Teilnehmerliste (Email, Punkte), mit hervorgehobenem Gewinner.
- Zufällige Gewinner-Ziehung unter allen Teilnehmenden mit 5 von 5 Punkten.
- Speichern des Gewinners in der Datenbank; Neu-Ziehung ersetzt den bisherigen.

Nicht enthalten (bewusst ausgeklammert, YAGNI):
- Benutzerverwaltung / mehrere Konten / Rollen (kein ASP.NET Core Identity).
- Teilnahme-Zeitstempel, Paginierung, CSV-Export.
- E-Mail-Benachrichtigung des Gewinners.
- Automatisierte Tests / Testprojekt (Logik wird testbar gekapselt, manuelle Prüfung).

## Authentifizierung
- Cookie-Authentifizierung über `Microsoft.AspNetCore.Authentication.Cookies`,
  **ohne** ASP.NET Core Identity.
- Registrierung in `Program.cs`:
  - `builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => { options.LoginPath = "/Admin/Login"; options.AccessDeniedPath = "/Admin/Login"; });`
  - `app.UseAuthentication();` **vor** `app.UseAuthorization();`.
- Das Admin-Passwort wird aus der Konfiguration unter `AdminSettings:Password`
  gelesen. Es wird **nicht** in einer committeten Datei abgelegt, sondern über
  User Secrets gesetzt:
  `dotnet user-secrets set "AdminSettings:Password" "<passwort>"`.
- Nach erfolgreichem Login wird per `HttpContext.SignInAsync` ein Auth-Cookie
  mit einer einzelnen Claim (z. B. Name = "Admin") gesetzt.

## Datenmodell
- Neue Eigenschaft auf `Models/Seetalhorn.cs`:
  `public bool IsWinner { get; set; }` (default `false`).
- Neue EF-Core-Migration, welche die Spalte `IsWinner` (NOT NULL, Default 0)
  zur Tabelle hinzufügt.
- Invariante: Höchstens ein Datensatz hat `IsWinner == true`. Diese Invariante
  wird in der Ziehungslogik durchgesetzt (nicht per DB-Constraint).

## Ziehungslogik (testbar gekapselt)
- Reine, seiteneffektfreie Auswahlmethode, die aus einer Liste von Kandidaten
  zufällig einen auswählt, z. B.:
  `static Seetalhorn? DrawWinner(IReadOnlyList<Seetalhorn> eligible, Random rng)`.
  - Kandidaten sind Teilnehmende mit `Punkte == 5` (bzw. == Anzahl Fragen).
  - Rückgabe `null`, wenn keine Kandidaten vorhanden sind.
  - Der `Random` wird als Parameter übergeben, damit die Auswahl in einem Test
    mit festem Seed deterministisch überprüfbar ist.
- Diese Methode enthält keinen Datenbankzugriff. Der Controller lädt die
  Kandidaten, ruft die Methode auf, setzt bei allen bisherigen Gewinnern
  `IsWinner = false`, setzt beim gezogenen `IsWinner = true` und speichert.

## Controller: AdminController
- `GET Login` – zeigt das Login-Formular.
- `POST Login` `[ValidateAntiForgeryToken]` – vergleicht das eingegebene
  Passwort mit `AdminSettings:Password`; bei Erfolg Cookie setzen und zu
  `Participants` weiterleiten, sonst Formular mit Fehlermeldung erneut anzeigen.
- `POST Logout` `[ValidateAntiForgeryToken]` – meldet ab, leitet zu `Login`.
- `GET Participants` `[Authorize]` – lädt alle Teilnahmen, sortiert (nach Punkten
  absteigend, dann Id), reicht sie an die View; ermittelt den aktuellen Gewinner.
- `POST Draw` `[Authorize][ValidateAntiForgeryToken]` – führt die Ziehung aus
  (siehe oben) und leitet zurück zu `Participants`. Gibt es keinen 5/5-Kandidaten,
  wird kein Gewinner gesetzt und eine Hinweismeldung (TempData) angezeigt.

## Views
- `Views/Admin/Login.cshtml` – Passwortfeld, Absenden-Button, Fehleranzeige.
- `Views/Admin/Participants.cshtml` – Tabelle (Email, Punkte, Gewinner-Markierung),
  Banner mit aktuellem Gewinner, „Gewinner ziehen"-Button (POST-Formular mit
  Anti-Forgery-Token und JS-`confirm`-Rückfrage bei bereits vorhandenem Gewinner),
  Abmelden-Link.
- Zugang zum Admin-Bereich über einen dezenten „Admin"-Link im Footer von
  `Views/Shared/_Layout.cshtml`.

## Datenfluss
1. Admin ruft `/Admin/Participants` auf → nicht angemeldet → Weiterleitung zu
   `/Admin/Login`.
2. Passwort eingeben → Cookie gesetzt → Teilnehmerliste sichtbar.
3. „Gewinner ziehen" klicken → `POST Draw` → zufällige Auswahl aus 5/5 →
   bisheriger Gewinner zurückgesetzt, neuer markiert, gespeichert → zurück zur
   Liste mit hervorgehobenem Gewinner.

## Fehlerbehandlung / Randfälle
- Falsches Passwort: Login-View mit Fehlermeldung, kein Cookie.
- Nicht angemeldeter Zugriff auf `[Authorize]`-Aktionen: automatische
  Weiterleitung zu `LoginPath`.
- Ziehung ohne 5/5-Teilnehmer: kein Gewinner, TempData-Hinweis in der Liste.
- Neu-Ziehung: JS-`confirm` warnt, dass der bisherige Gewinner ersetzt wird.

## Konfiguration
- `AdminSettings:Password` über User Secrets (lokal) bzw. Umgebungsvariable
  (Deployment). Kein Passwort im Repository.

## Verifikation (manuell, durch den Entwickler auf einer Maschine mit .NET-6-SDK)
Hinweis: In der Entwicklungsumgebung, in der dieser Code erstellt wurde, ist kein
.NET-SDK verfügbar; Build und Tests konnten dort nicht ausgeführt werden.
1. `dotnet build` läuft ohne Fehler.
2. `dotnet ef database update` legt die Spalte `IsWinner` an.
3. Admin-Passwort per User Secrets setzen, App starten.
4. `/Admin/Participants` ohne Login → Weiterleitung zum Login.
5. Falsches Passwort → Fehlermeldung; richtiges Passwort → Liste sichtbar.
6. Mehrere Teilnahmen anlegen (inkl. mind. einer mit 5/5) → „Gewinner ziehen"
   markiert eine 5/5-Person; erneutes Ziehen ersetzt den Gewinner.
7. Ziehung ohne 5/5-Teilnehmer → Hinweismeldung, kein Gewinner.
8. Abmelden → erneuter Zugriff auf die Liste verlangt wieder Login.
