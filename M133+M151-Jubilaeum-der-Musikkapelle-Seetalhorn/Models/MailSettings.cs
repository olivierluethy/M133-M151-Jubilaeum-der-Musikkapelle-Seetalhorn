namespace M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Models
{
    // Wird aus dem Konfigurationsabschnitt "MailSettings" gebunden.
    // Das Passwort kommt aus User Secrets (MailSettings:Password), nicht aus
    // einer committeten Datei.
    public class MailSettings
    {
        public string Mail { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
    }
}
