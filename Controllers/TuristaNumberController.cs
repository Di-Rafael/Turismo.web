using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Turismo.Application.Common.Interfaces;
using Turismo.Domain.Entities;
using Turismo.web.ViewModels;

namespace Turismo.web.Controllers
{
	public class TuristaNumberController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public TuristaNumberController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			var TuristaNumber = _unitOfWork.TuristaNumber.GetAll(includeProperties: "Turista");
			return base.View((object)TuristaNumber);
		}

		public IActionResult Create()
		{
			TuristaNumberVM TuristaNumberVM = new()
			{
				TuristaList = _unitOfWork.Turista.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				})
			};
			return View(TuristaNumberVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(TuristaNumberVM obj)
		{
			if (ModelState.IsValid)
			{
				bool roomNumberExists = _unitOfWork.TuristaNumber.Any(u => u.Turista_Number == obj.TuristaNumber.Turista_Number);

				if (!roomNumberExists)
				{
					// Remove the explicit setting of the Votos property
					obj.TuristaNumber.Turista_Number = 0; // Or any default value if necessary

					_unitOfWork.TuristaNumber.Add(obj.TuristaNumber);
					_unitOfWork.Save();

					TempData["success"] = "The turista number has been created successfully.";
					return RedirectToAction(nameof(Index));
				}
				else
				{
					TempData["error"] = "O turista with the same number value already exists.";
				}
			}

			// Re-populate the YoutuberList property
			obj.TuristaList = _unitOfWork.Turista.GetAll().Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString()
			}).ToList();

			return View(obj);
		}
		public IActionResult Update(int turistaNumberId)
		{
			TuristaNumberVM TuristaNumberVM = new() 
			{
				TuristaList = _unitOfWork.Turista.GetAll().Select(y => new SelectListItem
				{
					Text = y.Name,
					Value = y.Id.ToString()
				})
					.ToList(),
				TuristaNumber = (Domain.Entities.TuristaNumber)_unitOfWork.TuristaNumber.Get(y => y.Turista_Number == turistaNumberId)
			};

			if (TuristaNumberVM.TuristaNumber == null)
			{
				return RedirectToAction("Error", "Home");
			}

			return View(TuristaNumberVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Update(TuristaNumberVM viewModel)
		{
			if (ModelState.IsValid && viewModel.TuristaNumber != null)
			{
				_unitOfWork.TuristaNumber.Update(viewModel.TuristaNumber);
				_unitOfWork.Save();
				TempData["success"] = "The turista number has been updated successfully.";
				return RedirectToAction(nameof(Index));
			}

			TempData["error"] = "The turista number could not be updated.";
			viewModel.TuristaList = _unitOfWork.Turista.GetAll().Select(y => new SelectListItem
			{
				Text = y.Name,
				Value = y.Id.ToString()
			})
				.ToList();

			return View(viewModel);
		}

		public IActionResult Delete(int turistaNumberId)
		{
			TuristaNumberVM TuristaNumberVM = new()
			{
				TuristaList = _unitOfWork.Turista.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				}),
				TuristaNumber = (TuristaNumber)_unitOfWork.TuristaNumber.Get(_ => _.Turista_Number == turistaNumberId)!
			};

			if (TuristaNumberVM.TuristaNumber is null)
			{
				return RedirectToAction("Error", "Home");
			}

			return View(TuristaNumberVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Delete(TuristaNumberVM TuristaNumberVM)
		{
			TuristaNumber objFromDb = (TuristaNumber)_unitOfWork.TuristaNumber.Get(_ => _.Turista_Number == TuristaNumberVM.TuristaNumber.Turista_Number);

			if (objFromDb is not null)
			{
				_unitOfWork.TuristaNumber.Remove(objFromDb);
				_unitOfWork.Save();

				TempData["success"] = "The turista number has been deleted successfully.";

				return RedirectToAction(nameof(Index));
			}

			TempData["error"] = "The turista number could not be deleted.";
			return View(TuristaNumberVM);
		}
	}
}