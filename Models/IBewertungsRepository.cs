using System.Collections.Generic;
using System.Threading.Tasks;

namespace UmbracoReviewSite.Models
{
    public interface IBewertungsRepository
    {
        Task<IEnumerable<Bewertung>> HoleBewertungenFuerSeite(int seitenId, bool nurGenehmigte = true);
        Task<Bewertung?> HoleBewertungNachId(int id);
        Task Hinzufuegen(Bewertung bewertung);
        Task Aktualisieren(Bewertung bewertung);
        Task Loeschen(int id);
        Task<int> LikeBewertung(int bewertungsId);
        Task<IEnumerable<Bewertung>> HoleAusstehendeBewertungen();
    }
}