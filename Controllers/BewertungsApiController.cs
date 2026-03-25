using Microsoft.AspNetCore.Mvc;
using UmbracoReviewSite.Models;
using System;
using System.Linq;
using Umbraco.Cms.Web.Common.Controllers;

namespace UmbracoReviewSite.Controllers
{
    public class BewertungsApiController : UmbracoApiController
    {
        private static readonly MockBewertungsRepository _bewertungsRepository = new MockBewertungsRepository();

        [HttpPost]
        public IActionResult Genehmigen(int bewertungsId)
        {
            try
            {
                var bewertung = _bewertungsRepository.HoleBewertungNachId(bewertungsId).Result;
                if (bewertung == null)
                    return NotFound("Bewertung nicht gefunden.");

                bewertung.IstGenehmigt = true;
                _bewertungsRepository.Aktualisieren(bewertung).Wait();

                return Redirect("/admin-dashboard?key=admin123");
            }
            catch (Exception ex)
            {
                return BadRequest($"Fehler: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Ablehnen(int bewertungsId)
        {
            try
            {
                var bewertung = _bewertungsRepository.HoleBewertungNachId(bewertungsId).Result;
                if (bewertung == null)
                    return NotFound("Bewertung nicht gefunden.");

                _bewertungsRepository.Loeschen(bewertungsId).Wait();
                return Redirect("/admin-dashboard?key=admin123");
            }
            catch (Exception ex)
            {
                return BadRequest($"Fehler: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult HoleGenehmigteBewertungen(int seitenId)
        {
            try
            {
                var bewertungen = _bewertungsRepository.HoleBewertungenFuerSeite(seitenId, nurGenehmigte: true).Result.ToList();
                return Ok(bewertungen);
            }
            catch (Exception ex)
            {
                return BadRequest(new { fehler = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult HoleAusstehendeBewertungen()
        {
            try
            {
                var bewertungen = _bewertungsRepository.HoleAusstehendeBewertungen().Result.ToList();
                return Ok(bewertungen);
            }
            catch (Exception ex)
            {
                return BadRequest(new { fehler = ex.Message });
            }
        }
    }
}