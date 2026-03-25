using Microsoft.AspNetCore.Mvc;
using UmbracoReviewSite.Models;

namespace UmbracoReviewSite.Controllers
{
    public class StartseiteController : Controller
    {
        private static readonly MockBewertungsRepository _bewertungsRepository = new MockBewertungsRepository();

        [HttpPost]
        public IActionResult BewertungGenehmigen(int bewertungsId)
        {
            try
            {
                var bewertung = _bewertungsRepository.HoleBewertungNachId(bewertungsId).Result;
                if (bewertung != null)
                {
                    bewertung.IstGenehmigt = true;
                    _bewertungsRepository.Aktualisieren(bewertung).Wait();
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            
            return Redirect("/admin-dashboard?key=admin123");
        }

        [HttpPost]
        public IActionResult BewertungAblehnen(int bewertungsId)
        {
            try
            {
                _bewertungsRepository.Loeschen(bewertungsId).Wait();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            
            return Redirect("/admin-dashboard?key=admin123");
        }
    }
}