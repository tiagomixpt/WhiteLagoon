using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interface;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.Controllers
{

	public class VillaController : Controller
    {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			_unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
		}
		public IActionResult Index() // /villa
        {
            var villas = _unitOfWork.Villa.GetAll();

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
                if(obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\Villas");
                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    obj.Image.CopyTo(fileStream);

                    obj.ImageUrl = @"\images\VillaImages" + fileName;
                    

                }
                else
                    obj.ImageUrl = "https://placehold.co/600x400";
                

                _unitOfWork.Villa.Add(obj);
				_unitOfWork.Save();
					TempData["success"] = "The Villa has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Villa could not be created.";
            return View(obj);
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(u => u.Id == villaId);

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
				_unitOfWork.Villa.Update(obj);
				_unitOfWork.Save();
                TempData["success"] = "The Villa has been updated successfully.";
                return RedirectToAction(nameof(Index));
			}
            TempData["error"] = "The Villa could not be updated.";
            return View(obj);
		}

		public IActionResult Delete(int villaId)
		{
			Villa? obj = _unitOfWork.Villa.Get(u => u.Id == villaId);


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
            Villa? objFromDb = _unitOfWork.Villa.Get(_ => _.Id == obj.Id);
            if (objFromDb is not null)
            {
				_unitOfWork.Villa.Remove(objFromDb);
				_unitOfWork.Save();

                TempData["success"] = "The Villa has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Villa could not be deleted.";
            return View(obj);
        }
    }
}
