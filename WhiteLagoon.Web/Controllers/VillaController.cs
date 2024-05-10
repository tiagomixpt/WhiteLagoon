﻿using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
   
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index() // /villa
        {
            var villas = _db.Villas.ToList();

            return View(villas);
        }

        public IActionResult Create() 
        {
            return View();      
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Villa obj)
        {
            if(obj.Name == obj.Description)
            {
                ModelState.AddModelError("", "The Description cannot exactly match the Name");
            }

            if (ModelState.IsValid)
            {
            _db.Villas.Add(obj);
            _db.SaveChanges();
                TempData["success"] = "The Villa has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Villa could not be created.";
            return View(obj);
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj = _db.Villas.FirstOrDefault(u => u.Id == villaId);

            //Villa? obj2 = _db.Villas.Find(villaId);
            //  var VillaList = _db.Villas.Where(u => u.Price > 50 && u.Occupancy > 0).ToList();
            
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Update(Villa obj)
		{
		
			if (ModelState.IsValid && obj.Id>0)
			{
				_db.Villas.Update(obj);
				_db.SaveChanges();
                TempData["success"] = "The Villa has been updated successfully.";
                return RedirectToAction(nameof(Index));
			}
            TempData["error"] = "The Villa could not be updated.";
            return View(obj);
		}

		public IActionResult Delete(int villaId)
		{
			Villa? obj = _db.Villas.FirstOrDefault(u => u.Id == villaId);


			if (obj == null)
			{
				return RedirectToAction("Error", "Home");
			}
			return View(obj);
		}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _db.Villas.FirstOrDefault(_ => _.Id == obj.Id);
            if (objFromDb is not null)
            {
				_db.Villas.Remove(objFromDb);
				_db.SaveChanges();

                TempData["success"] = "The Villa has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Villa could not be deleted.";
            return View(obj);
        }
    }
}
