# M133-M151-Jubilaeum-der-Musikkapelle-Seetalhorn
Eine Webseite entwickelt mit ASP.NET Core MVC, in welcher Besucher einen Wettbewerb ausfüllen können.

## Voraussetzungen
- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- SQL Server – standardmässig **SQL Server LocalDB** (Teil von Visual Studio bzw. der SQL Server Express-Installation, nur unter Windows verfügbar)
- Optional: das EF-Core-Tool für Migrationen (`dotnet tool install --global dotnet-ef`)

## Projekt bauen
Aus dem Repository-Wurzelverzeichnis:

```bash
dotnet build "M133+M151-Jubilaeum-der-Musikkapelle-Seetalhorn.sln"
```

## Datenbank einrichten
Die Verbindungszeichenkette steht in `appsettings.Development.json` unter `ConnectionStrings:DefaultConnection` und zeigt auf eine lokale LocalDB-Instanz. Die Datenbank wird über die vorhandenen EF-Core-Migrationen erstellt:

```bash
cd "M133+M151-Jubilaeum-der-Musikkapelle-Seetalhorn"
dotnet ef database update
```

Nutzt du eine andere SQL-Server-Instanz, passe den Wert von `DefaultConnection` entsprechend an.

## Anwendung starten
```bash
cd "M133+M151-Jubilaeum-der-Musikkapelle-Seetalhorn"
dotnet run
```

Die App ist danach unter den in `Properties/launchSettings.json` konfigurierten Adressen erreichbar (standardmässig `https://localhost:7012` bzw. `http://localhost:5012`). Alternativ lässt sich die Projektmappe direkt in Visual Studio öffnen und mit F5 starten.

## Admin-Bereich
Unter `/Admin/Login` (bzw. über den «Admin»-Link im Footer) gelangt man zum geschützten Admin-Bereich. Dort lässt sich die Teilnehmerliste einsehen und ein Gewinner unter allen Teilnehmenden mit voller Punktzahl (5/5) auslosen.

Der Zugang ist durch ein gemeinsames Passwort geschützt. Damit kein Passwort im Repository landet, wird es über User Secrets gesetzt:

```bash
cd "M133+M151-Jubilaeum-der-Musikkapelle-Seetalhorn"
dotnet user-secrets init
dotnet user-secrets set "AdminSettings:Password" "<dein-passwort>"
```

Ohne gesetztes Passwort ist keine Anmeldung möglich. Im Deployment kann der Wert stattdessen als Umgebungsvariable `AdminSettings__Password` bereitgestellt werden.

## Bestätigungsmail
Nach einer erfolgreichen Teilnahme wird eine Bestätigungsmail mit der erreichten Punktzahl an die angegebene Adresse versendet (per MailKit über SMTP). Schlägt der Versand fehl, wird der Fehler nur protokolliert – die Teilnahme bleibt gespeichert.

Absender, Host und Port stehen unter `MailSettings` in `appsettings.json`. Das SMTP-Passwort wird – wie das Admin-Passwort – über User Secrets gesetzt, damit es nicht im Repository landet:

```bash
cd "M133+M151-Jubilaeum-der-Musikkapelle-Seetalhorn"
dotnet user-secrets set "MailSettings:Mail" "<absender@example.com>"
dotnet user-secrets set "MailSettings:Password" "<smtp-passwort>"
```

Bei Gmail als Host ist dafür ein [App-Passwort](https://support.google.com/accounts/answer/185833) nötig. Im Deployment können die Werte alternativ als Umgebungsvariablen `MailSettings__Mail` und `MailSettings__Password` gesetzt werden.

## Webseiten die mir geholfen haben
Issue 1: Check if email already exists in the database<br>
Link: https://stackoverflow.com/questions/54258869/check-if-the-user-already-exists-in-asp-net-mvc
