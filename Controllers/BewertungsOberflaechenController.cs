using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UmbracoReviewSite.Models;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace UmbracoReviewSite.Controllers
{
    public class BewertungsOberflaechenController : SurfaceController
    {
        private readonly IBewertungsRepository _bewertungsRepository;

        public BewertungsOberflaechenController(
            IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider,
            IConfiguration configuration)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _bewertungsRepository = new MockBewertungsRepository();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BewertungEinreichen(Bewertung bewertung)
        {
            if (!ModelState.IsValid)
            {
                TempData["BewertungsFehler"] = "Bitte überprüfen Sie Ihre Eingaben";
                return Redirect(Request.Headers["Referer"].ToString());
            }

            try
            {
                await _bewertungsRepository.Hinzufuegen(bewertung);
                TempData["BewertungsErfolg"] = "Ihre Bewertung wurde erfolgreich hinzugefügt!";
            }
            catch (Exception ex)
            {
                TempData["BewertungsFehler"] = "Fehler: " + ex.Message;
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public async Task<IActionResult> LikeBewertung(int bewertungsId)
        {
            try
            {
                var neueLikeAnzahl = await _bewertungsRepository.LikeBewertung(bewertungsId);
                return Json(new { erfolg = true, likes = neueLikeAnzahl });
            }
            catch (Exception ex)
            {
                return Json(new { erfolg = false, fehler = ex.Message });
            }
        }
    }
}