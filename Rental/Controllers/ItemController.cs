using Rental.Data;
using Rental.Models;
using Microsoft.AspNetCore.Mvc;

namespace Rental.Controllers
{
    public class ItemController : Controller
    {
        private readonly RentalContext _db;

        public ItemController(RentalContext db)
        {
            _db = db;
        }

        public IActionResult PriceList()
        {
            IEnumerable<ItemModel> items = _db.Items;
            return View(items);
        }

        public IActionResult Index(string searchValue)
        {
            IEnumerable<ItemModel> items = _db.Items;

            if(string.IsNullOrEmpty(searchValue))
            {
                return View(items);
            }
            else
            {
                IEnumerable<ItemModel> search = items.Where(x => x.Name.ToLower().Contains(searchValue.ToLower()));
                return View(search);
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ItemModel obj)
        {
            if (obj.CostPerHour == null && obj.CostPerDay == null)
            {
                ModelState.AddModelError("OtherError", "Należy ustalić koszt wypożyczenia przedmiotu w co najmniej jednej lubryce!");
            }
            if (ModelState.IsValid)
            {
                _db.Items.Add(obj);
                _db.SaveChanges();
                TempData["success"] = $"Przedmiot: {obj.Name} został dodany do bazy przedmiotów.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            ItemModel? item = _db.Items.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ItemModel obj)
        {
            if (obj.CostPerHour == null && obj.CostPerDay == null)
            {
                ModelState.AddModelError("OtherError", "Należy ustalić koszt wypożyczenia przedmiotu w co najmniej jednej lubryce!");
            }
            if (ModelState.IsValid)
            {
                _db.Items.Update(obj);
                _db.SaveChanges();
                TempData["success"] = $"Przedmiot: {obj.Name} został zaktualizowany.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            ItemModel? item = _db.Items.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(ItemModel obj)
        {
            _db.Items.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = $"Usuięto przedmiot: {obj.Name} z listy przedmiotów.";
            return RedirectToAction("Index");
        }
    }
}
