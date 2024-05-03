using Microsoft.AspNetCore.Mvc;
using Turismo.Domain.Entities;
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Turista obj)
        {
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("Name", "The description cannot exactly match the Name");
            }

            if (ModelState.IsValid)
            {
                _db.Turistas.Add(obj);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(obj);
        }

        public IActionResult Update(int turistaId)
        {
            Turista? obj = _db.Turistas.FirstOrDefault(u => u.Id == turistaId);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Turista obj)
        {
            if (ModelState.IsValid && obj.Id > 0)
            {
                _db.Turistas.Update(obj);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(obj);
        }

        public IActionResult Delete(int turistaId)
        {
            Turista? obj = _db.Turistas.FirstOrDefault(u => u.Id == turistaId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Turista obj)
        {
            Turista? objFromDb = _db.Turistas.FirstOrDefault(v => v.Id == obj.Id);

            if (objFromDb is not null)
            {
                _db.Turistas.Remove(objFromDb);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(obj);
        }
    }
}
