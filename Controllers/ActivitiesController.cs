using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace time_tracker.Controllers;

[Authorize]
public class ActivitiesController : Controller
{
    public IActionResult Track()
    {
        return View();
    }
}
