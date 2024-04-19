using Microsoft.AspNetCore.Mvc;
using Turismo.Infrastructure.Data;


namespace Turismo.web.Controllers
{
    public class TuristaController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TuristaController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var turistas = _db.Turistas.ToList();

            return View(turistas);
        }
    }
}
