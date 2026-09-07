using M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Models;

namespace M133_M151_Jubilaeum_der_Musikkapelle_Seetalhorn.Services
{
    // Reine, seiteneffektfreie Auswahllogik fuer die Gewinner-Ziehung.
    // Kein Datenbankzugriff, damit sie mit festem Random-Seed deterministisch
    // getestet werden kann.
    public static class WinnerDrawing
    {
        // Waehlt zufaellig einen Gewinner aus den uebergebenen Kandidaten.
        // Gibt null zurueck, wenn keine Kandidaten vorhanden sind.
        public static Seetalhorn? DrawWinner(IReadOnlyList<Seetalhorn> eligible, Random rng)
        {
            if (eligible == null)
            {
                throw new ArgumentNullException(nameof(eligible));
            }
            if (rng == null)
            {
                throw new ArgumentNullException(nameof(rng));
            }
            if (eligible.Count == 0)
            {
                return null;
            }

            int index = rng.Next(eligible.Count);
            return eligible[index];
        }
    }
}
