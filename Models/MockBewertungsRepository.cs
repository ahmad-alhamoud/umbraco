using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UmbracoReviewSite.Models
{
    public class MockBewertungsRepository : IBewertungsRepository
    {
        private static List<Bewertung> _bewertungen = new List<Bewertung>();
        private static int _naechsteId = 1;

        static MockBewertungsRepository()
        {
            _bewertungen = new List<Bewertung>();
            _naechsteId = 1;
        }

        public Task Hinzufuegen(Bewertung bewertung)
        {
            bewertung.Id = _naechsteId++;
            bewertung.ErstellungsDatum = DateTime.Now;
            bewertung.Likes = 0;
            bewertung.IstGenehmigt = false;
            _bewertungen.Add(bewertung);
            return Task.CompletedTask;
        }

        public Task Loeschen(int id)
        {
            var bewertung = _bewertungen.FirstOrDefault(b => b.Id == id);
            if (bewertung != null) _bewertungen.Remove(bewertung);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Bewertung>> HoleAusstehendeBewertungen()
        {
            var ausstehende = _bewertungen.Where(b => !b.IstGenehmigt).OrderByDescending(b => b.ErstellungsDatum).ToList();
            return Task.FromResult(ausstehende.AsEnumerable());
        }

        public Task<Bewertung?> HoleBewertungNachId(int id)
        {
            var bewertung = _bewertungen.FirstOrDefault(b => b.Id == id);
            return Task.FromResult(bewertung);
        }

        public Task<IEnumerable<Bewertung>> HoleBewertungenFuerSeite(int seitenId, bool nurGenehmigte = true)
        {
            var bewertungen = _bewertungen.Where(b => b.SeitenId == seitenId);
            
            if (nurGenehmigte)
            {
                bewertungen = bewertungen.Where(b => b.IstGenehmigt);
            }
            
            return Task.FromResult(bewertungen.OrderByDescending(b => b.ErstellungsDatum).AsEnumerable());
        }

        public Task<int> LikeBewertung(int bewertungsId)
        {
            var bewertung = _bewertungen.FirstOrDefault(b => b.Id == bewertungsId);
            if (bewertung != null) bewertung.Likes++;
            return Task.FromResult(bewertung?.Likes ?? 0);
        }

        public Task Aktualisieren(Bewertung bewertung)
        {
            var vorhandene = _bewertungen.FirstOrDefault(b => b.Id == bewertung.Id);
            if (vorhandene != null)
            {
                vorhandene.AutorenName = bewertung.AutorenName;
                vorhandene.AutorenEmail = bewertung.AutorenEmail;
                vorhandene.SterneBewertung = bewertung.SterneBewertung;
                vorhandene.Kommentar = bewertung.Kommentar;
                vorhandene.IstGenehmigt = bewertung.IstGenehmigt;
            }
            return Task.CompletedTask;
        }
    }
}