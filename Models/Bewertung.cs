using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UmbracoReviewSite.Models
{
    public class Bewertung
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie Ihren Namen ein")]
        public string? AutorenName { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie Ihre E-Mail-Adresse ein")]
        [EmailAddress(ErrorMessage = "Ungültige E-Mail-Adresse")]
        public string? AutorenEmail { get; set; }

        [Required(ErrorMessage = "Bitte wählen Sie eine Bewertung")]
        [Range(1, 5, ErrorMessage = "Die Bewertung muss zwischen 1 und 5 liegen")]
        public int SterneBewertung { get; set; }

        [Required(ErrorMessage = "Bitte schreiben Sie einen Kommentar")]
        [StringLength(500, ErrorMessage = "Der Kommentar ist zu lang")]
        public string? Kommentar { get; set; }

        public int Likes { get; set; }
        public DateTime ErstellungsDatum { get; set; }
        public bool IstGenehmigt { get; set; }
        public int SeitenId { get; set; }
    }

    public class BewertungsAnsichtsModell
    {
        public int SeitenId { get; set; }
        public IEnumerable<Bewertung>? Bewertungen { get; set; }
        public Bewertung? NeueBewertung { get; set; }
    }
}