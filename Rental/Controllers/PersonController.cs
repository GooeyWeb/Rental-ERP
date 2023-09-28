using Rental.Data;
using Rental.Models;
using Microsoft.AspNetCore.Mvc;

namespace Rental.Controllers
{
    public class PersonController : Controller
    {
        private readonly RentalContext _db;

        public PersonController(RentalContext db)
        {
            _db = db;
        }

        public IActionResult Index(string searchValue)
        {
            IEnumerable<PersonModel> person = _db.Persons;

            if (string.IsNullOrEmpty(searchValue))
            {
                return View(person);
            }
            else
            {
                IEnumerable<PersonModel> search = person.Where(x => (x.FirstName.ToLower()+" "+x.LastName.ToLower()).Contains(searchValue.ToLower()));
                return View(search);
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PersonModel obj)
        {
            if (obj.PESEL.Length < 11)
            {
                ModelState.AddModelError("PESEL", "Numer PESEL musi zawierać 11 znaków!");
            }
            if (ModelState.IsValid)
            {
                _db.Persons.Add(obj);
                _db.SaveChanges();
                TempData["success"] = $"Użytkownik {obj.FirstName} {obj.LastName} zostały dodany do bazy klientów.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PersonModel? person = _db.Persons.Find(id);

            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PersonModel obj)
        {
            if (obj.PESEL.Length < 11)
            {
                ModelState.AddModelError("PESEL", "Numer PESEL musi zawierać 11 znaków!");
            }
            if (ModelState.IsValid)
            {
                _db.Persons.Update(obj);
                _db.SaveChanges();
                TempData["success"] = $"Dane użytkownika {obj.FirstName} {obj.LastName} zostały zaktualizowane.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PersonModel? person = _db.Persons.Find(id);

            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(PersonModel obj)
        {
            _db.Persons.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = $"Usunięto użytkownika {obj.FirstName} {obj.LastName} z listy klientów.";
            return RedirectToAction("Index");
        }

        public IActionResult Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PersonModel? person = _db.Persons.Find(id);

            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }
    }
}
