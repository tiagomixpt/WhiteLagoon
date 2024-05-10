﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{

	public class VillaNumberController : Controller
	{
		private readonly ApplicationDbContext _db;

		public VillaNumberController(ApplicationDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
            var villaNumbers = _db.VillaNumber
                .Include(u=>u.Villa)
                .ToList();

			return View(villaNumbers);
		}
        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new()
            {

                VillaList = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()

                })
            };
            
            return View(villaNumberVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VillaNumberVM obj)
        {
            bool roomNumberExists = _db.VillaNumber.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);
           
            if (ModelState.IsValid && !roomNumberExists)
            {
                _db.VillaNumber.Add(obj.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "The Villa has been created successfully.";
                return RedirectToAction(nameof(Index));
            }

            if (roomNumberExists)
                {
				TempData["error"] = "The villa number already exists.";
			}
            obj.VillaList = _db.Villas.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()

            });

			return View(obj);
        }

        public IActionResult Update(int villaNumberId)
        {
			VillaNumberVM villaNumberVM = new()
			{

				VillaList = _db.Villas.ToList().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()

				}),
                VillaNumber = _db.VillaNumber.FirstOrDefault(_=>_.Villa_Number == villaNumberId)!
			};
			if (villaNumberVM.VillaNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {
			

			if (ModelState.IsValid)
			{
				_db.VillaNumber.Update(villaNumberVM.VillaNumber);
				_db.SaveChanges();
				TempData["success"] = "The Villa has been updated successfully.";
				return RedirectToAction(nameof(Index));
			}

			villaNumberVM.VillaList = _db.Villas.ToList().Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString()

			});

			return View(villaNumberVM);
		
        }

		public IActionResult Delete(int villaNumberId)
		{
			VillaNumberVM villaNumberVM = new()
			{

				VillaList = _db.Villas.ToList().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()

				}),
				VillaNumber = _db.VillaNumber.FirstOrDefault(_ => _.Villa_Number == villaNumberId)!
			};
			if (villaNumberVM.VillaNumber is null)
			{
				return RedirectToAction("Error", "Home");
			}
			return View(villaNumberVM);
		}
		[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? objFromDb = _db.VillaNumber
                .FirstOrDefault(_ => _.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
            if (objFromDb is not null)
            {
                _db.VillaNumber.Remove(objFromDb);
                _db.SaveChanges();

                TempData["success"] = "The Villa Number has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Villa could not be deleted.";
            return View(villaNumberVM   );
        }
    }
}