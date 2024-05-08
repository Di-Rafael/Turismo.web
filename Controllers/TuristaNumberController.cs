using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Turismo.Domain.Entities;
using Turismo.Infrastructure.Data;

namespace Turismo.web.Controllers
{
	public class TuristaNumberController : Controller
	{
		private readonly ApplicationDbContext _db;

		public TuristaNumberController(ApplicationDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			var TuristaNumbers = _db.TuristaNumbers
				.Include(u => u.Turista)
				.ToList();

			return View(TuristaNumbers);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(TuristaNumbers obj)
		{
			if (ModelState.IsValid)
			{
				_db.TuristaNumbers.Add(obj);
				_db.SaveChanges();
				TempData["success"] = "The Youtuber Records has been created successfully.";
				return RedirectToAction(nameof(Index));
			}

			TempData["error"] = "The Youtuber Records could not be created.";
			return View(obj);
		}
		public IActionResult Update(int villaId)
		{
			Turista? obj = _db.Turistas.FirstOrDefault(_ => _.Id == villaId);

			//Villa? obj2 = _db.Villas.Find(villaId);
			//var VillaList = _db.Villas.Where(_ => _.Price > 50 && _.Occupancy > 0);

			if (obj is null)
			{
				return RedirectToAction("Error", "Home");
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
				TempData["success"] = "The villa has been updated successfully.";
				return RedirectToAction(nameof(Index));
			}

			TempData["error"] = "The villa could not be updated.";
			return View(obj);
		}

		public IActionResult Delete(int villaId)
		{
			Turista? obj = _db.Turistas.FirstOrDefault(_ => _.Id == villaId);

			if (obj is null)
			{
				return RedirectToAction("Error", "Home");
			}

			return View(obj);
		}

		[HttpPost]
		public IActionResult Delete(Turista obj)
		{
			Turista? objFromDb = _db.Turistas.FirstOrDefault(_ => _.Id == obj.Id);

			if (objFromDb is not null)
			{
				_db.Turistas.Remove(objFromDb);
				_db.SaveChanges();

				TempData["success"] = "The villa has been deleted successfully.";

				return RedirectToAction(nameof(Index));
			}

			TempData["error"] = "The villa could not be deleted.";
			return View(obj);
		}
	}
}