# Onboarding – Jubiläums-Quiz Musikkapelle Seetalhorn

Willkommen im Projekt! Diese Datei hilft dir, dich schnell zurechtzufinden.

## Worum geht es
Eine Web-App zum Jubiläum (50 Jahre) der Musikkapelle Seetalhorn. Besucher füllen
einen **Quiz-Wettbewerb** mit 5 Fragen aus und hinterlassen ihre E-Mail-Adresse.
Die Teilnahme wird in einer Datenbank gespeichert, die Teilnehmenden erhalten eine
Bestätigungsmail. Über einen geschützten **Admin-Bereich** kann die Kapelle die
Teilnehmerliste einsehen und unter den Teilnehmenden mit voller Punktzahl einen
Gewinner auslosen.

Das Projekt entstand im Rahmen der Module **M133** (Web-App) und **M151**
(Datenbanken in Web-Applikationen).

## Technologie-Stack
- **ASP.NET Core MVC**, .NET **6.0** (C#)
- **Entity Framework Core 6** mit **SQL Server** (lokal: LocalDB)
- **MailKit** für den SMTP-Mailversand
- Cookie-basierte Authentifizierung für den Admin-Bereich (ohne ASP.NET Core Identity)
- Bootstrap für das Layout

## Voraussetzungen
- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- SQL Server LocalDB (Teil von Visual Studio / SQL Server Express, nur Windows)
- Optional: `dotnet tool install --global dotnet-ef` für Migrationen

## Erste Schritte
```bash
# 1. Projekt bauen (aus dem Repository-Wurzelverzeichnis)
dotnet build "M133+M151-Jubilaeum-der-Musikkapelle-Seetalhorn.sln"

# 2. Ins Projektverzeichnis wechseln
cd "M133+M151-Jubilaeum-der-Musikkapelle-Seetalhorn"

# 3. Datenbank erstellen/aktualisieren
dotnet ef database update

# 4. Geheimnisse setzen (siehe Abschnitt "Konfiguration & Geheimnisse")
dotnet user-secrets set "AdminSettings:Password" "<admin-passwort>"
dotnet user-secrets set "MailSettings:Mail" "<absender@example.com>"
dotnet user-secrets set "MailSettings:Password" "<smtp-passwort>"

# 5. App starten
dotnet run
```
Die App läuft danach standardmässig auf `https://localhost:7012` bzw.
`http://localhost:5012`. Alternativ die `.sln` in Visual Studio öffnen und mit F5
starten.

## Projektstruktur
Der gesamte Anwendungscode liegt im Unterordner
`M133+M151-Jubilaeum-der-Musikkapelle-Seetalhorn/`:

| Ordner / Datei | Zweck |
|---|---|
| `Program.cs` | Startup: DI, DbContext, Auth, Routing (Default-Route `Seetal/Index`) |
| `Controllers/SeetalController.cs` | Öffentlicher Teil: Quiz, Ergebnisseite, Fehlerseite |
| `Controllers/AdminController.cs` | Admin: Login/Logout, Teilnehmerliste, Gewinner-Ziehung |
| `Models/Seetalhorn.cs` | Datenmodell einer Teilnahme (E-Mail, 5 Antworten, Punkte, IsWinner) |
| `Models/MailSettings.cs` | Konfigurationsobjekt für den Mailversand |
| `Models/LoginViewModel.cs` | ViewModel des Admin-Logins |
| `Services/IEmailSender.cs` / `SmtpEmailSender.cs` | Mailversand-Abstraktion + MailKit-Implementierung |
| `Services/WinnerDrawing.cs` | Reine, testbare Zufallsauswahl für die Gewinner-Ziehung |
| `Data/ApplicationDbContext.cs` | EF-Core-Kontext (`DbSet<Seetalhorn>`) |
| `Migrations/` | EF-Core-Migrationen |
| `Views/Seetal/` | Startseite, Quiz, Ergebnisseite |
| `Views/Admin/` | Login, Teilnehmerliste |
| `Views/Shared/` | Layout, Fehlerseite, Validierungs-Partial |

## Wichtige Abläufe

### Quiz-Teilnahme
`Seetal/Index` → «Jetzt teilnehmen» → `Seetal/Competition` (Formular). Beim Absenden
prüft der Controller die serverseitige Validierung und ob die E-Mail bereits
teilgenommen hat. Danach werden die Punkte berechnet (1 pro richtiger Antwort,
korrekte Werte zentral im Array `CorrectAnswers`), gespeichert, eine
Bestätigungsmail versendet und per Post/Redirect/Get auf `Seetal/Result`
weitergeleitet.

### Admin-Bereich
Zugang über `/Admin/Login` (oder den «Admin»-Link im Footer). Nach der Anmeldung
mit dem gemeinsamen Admin-Passwort zeigt `Admin/Participants` alle Teilnahmen. Der
Button «Gewinner ziehen» wählt zufällig eine Person mit 5/5 Punkten. Eine erneute
Ziehung ersetzt den bisherigen Gewinner (mit Rückfrage).

### Bestätigungsmail
Nach erfolgreicher Teilnahme sendet `SmtpEmailSender` eine Dank-Mail mit der
Punktzahl. Schlägt der Versand fehl (z. B. SMTP nicht erreichbar), wird der Fehler
nur protokolliert – die Teilnahme bleibt gespeichert.

## Konfiguration & Geheimnisse
**Keine Passwörter im Repository.** Geheimnisse werden lokal über User Secrets
gesetzt (siehe «Erste Schritte»), im Deployment über Umgebungsvariablen:
- `AdminSettings:Password` – Passwort für den Admin-Bereich
- `MailSettings:Mail` / `MailSettings:Password` – SMTP-Absender und -Passwort
  (bei Gmail ein [App-Passwort](https://support.google.com/accounts/answer/185833))

Nicht-geheime Mail-Einstellungen (`DisplayName`, `Host`, `Port`) sowie der lokale
Connection-String stehen in `appsettings.json` bzw. `appsettings.Development.json`.

## Datenbank-Änderungen
Modelländerungen erfolgen über EF-Core-Migrationen:
```bash
cd "M133+M151-Jubilaeum-der-Musikkapelle-Seetalhorn"
dotnet ef migrations add <Name>
dotnet ef database update
```

## Konventionen
- **Sprache:** Benutzeroberfläche und Commit-Nachrichten auf Deutsch.
- **Schreibweise:** Schweizer Schreibweise – «ss» statt «ß».
- Bestehende Muster im Code übernehmen (Controller/View/Model-Struktur, Namensgebung).

## Weiterführende Dokumente
- `README.md` – Kurzanleitung zu Build, Datenbank und Konfiguration
- `docs/superpowers/specs/` – Design-Specs zu grösseren Features (z. B. Admin-Bereich)
