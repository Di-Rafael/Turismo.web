using Microsoft.AspNetCore.Mvc;
using Turismo.Application.Common.Interfaces;
using Turismo.Domain.Entities;



public class TuristaController : Controller
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IWebHostEnvironment _webHostEnvironment;

	public TuristaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
	{
		_unitOfWork = unitOfWork;
		_webHostEnvironment = webHostEnvironment;
	}

	public IActionResult Index()
	{
		var turista = _unitOfWork.Turista.GetAll();

		return View(turista);
	}
	public IActionResult Create()
	{
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult Create(Turista obj)
	{

		if (ModelState.IsValid)
		{
			_unitOfWork.Turista.Add(obj);
			_unitOfWork.Save();
			TempData["success"] = "The Turista has been created successfully.";
			return RedirectToAction(nameof(Index));
		}

		TempData["error"] = "The Turista could not be created.";
		return View(obj);
	}

	public IActionResult Update(int TuristaId)
	{
		Turista? obj = (Turista?)_unitOfWork.Turista.Get(_ => _.Id == TuristaId);

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
			_unitOfWork.Turista.Update(obj);
			_unitOfWork.Save();
			TempData["success"] = "The Turista has been updated successfully.";
			return RedirectToAction(nameof(Index));
		}

		TempData["error"] = "The turista could not be updated.";
		return View(obj);
	}

	public IActionResult Delete(int TuristaId)
	{
		Turista? obj = (Turista?)_unitOfWork.Turista.Get(_ => _.Id == TuristaId);

		if (obj is null)
		{
			return RedirectToAction("Error", "Home");
		}

		return View(obj);
	}

	[HttpPost]
	public IActionResult Delete(Turista obj)
	{
		Turista? objFromDb = (Turista?)_unitOfWork.Turista.Get(_ => _.Id == obj.Id);

		if (objFromDb is not null)
		{
			_unitOfWork.Turista.Remove(obj);
			_unitOfWork.Save();

			TempData["success"] = "The Turista has been deleted successfully.";

			return RedirectToAction(nameof(Index));
		}

		TempData["error"] = "The Turista could not be deleted.";
		return View(obj);
	}
}

