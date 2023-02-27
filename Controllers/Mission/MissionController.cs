using Microsoft.AspNetCore.Mvc;

namespace CI_Platform.Controllers.Mission
{
    public class MissionController : Controller
    {
        public IActionResult missionLandingPlateform()
        {
            return View();
        }

        public IActionResult volunteering()
        {
            return View();
        }
    }
}
