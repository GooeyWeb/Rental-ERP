using Rental.Data;
using Rental.Models;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System;

namespace Rental.Controllers
{
    public class OrderController : Controller
    {
        private readonly RentalContext _db;

        public OrderController(RentalContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            dynamic thisModel = new ExpandoObject();
            thisModel.Orders = _db.Orders;
            thisModel.Items = _db.Items;
            thisModel.Persons = _db.Persons;

            return View(thisModel);
        }

        public IActionResult Create(string searchPerson, string searchItem)
        {
            dynamic thisModel = new ExpandoObject();
            
            if (searchPerson != null)
                thisModel.Persons = _db.Persons.Where(x => (x.FirstName.ToLower() + " " + x.LastName.ToLower()).Contains(searchPerson.ToLower()));
            else
                thisModel.Persons = _db.Persons;

            if (searchItem != null)
                thisModel.Items = _db.Items.Where(x => x.Name.ToLower().Contains(searchItem.ToLower()));
            else
                thisModel.Items = _db.Items;

            return View(thisModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string pesel, int itemID, int advance, int amount)
        {
            ItemModel item = new ItemModel();

            if (!string.IsNullOrEmpty(itemID.ToString()))
            {
                item = _db.Items.Find(itemID);
            }

            if (pesel == null || string.IsNullOrEmpty(itemID.ToString()))
            {
                ModelState.AddModelError("OtherError", "Zamówienie musi mieć przypisanego użytkownika i sprzęt!");
            }

            if (amount > item.Availability)
            {
                ModelState.AddModelError("OtherError", "Nie ma wystarczająco sprzętu na stanie!");
            }

            if (ModelState.IsValid)
            {
                OrderModel order = new OrderModel();
                order.Advance = advance;
                order.PersonPESEL = pesel;
                order.Amount = amount;
                order.ItemID = item.ItemID;
                order.CostPerHour = item.CostPerHour;
                order.CostPerDay = item.CostPerDay;

                item.Availability -= amount;

                _db.Orders.Add(order);
                _db.Items.Update(item);
                _db.SaveChanges();
                TempData["success"] = ("Zamówienie zostało zatwierdzone");
                return RedirectToAction("Index","Home");
            }

            PersonModel person = _db.Persons.Find(pesel);
            dynamic thisModel = new ExpandoObject();

            if (person != null)
                thisModel.Persons = _db.Persons;
            else
                thisModel.Persons = person;
            if (item != null)
                thisModel.Items = _db.Items;
            else
                thisModel.Items = item;

            return View(thisModel);
        }

        public IActionResult CreateRepair(string searchPerson)
        {
            IEnumerable<PersonModel> person = _db.Persons;

            if (searchPerson != null)
                person = _db.Persons.Where(x => (x.FirstName.ToLower() + " " + x.LastName.ToLower()).Contains(searchPerson.ToLower()));

            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateRepair(string pesel, string item, int advance, int amount, int cost)
        {
            if (pesel == null)
            {
                ModelState.AddModelError("OtherError", "Zlecenie naprawy musi mieć przypisanego użytkownika!");
            }

            if (ModelState.IsValid)
            {
                OrderModel order = new OrderModel();
                order.Advance = advance;
                order.PersonPESEL = pesel;
                order.Amount = amount;
                order.ItemNameForRepair = item;
                order.Cost = cost;

                _db.Orders.Add(order);
                _db.SaveChanges();
                TempData["success"] = ("Zlecenie naprawy zostało zatwierdzone");
                return RedirectToAction("Index", "Home");
            }

            IEnumerable<PersonModel> person = _db.Persons;

            return View(person);
        }

        public IActionResult Details(int? id)
        {
            dynamic thisModel = new ExpandoObject();
            OrderModel? order = _db.Orders.Find(id);
            thisModel.Order = order;
            thisModel.Item = _db.Items.Find(order.ItemID);
            thisModel.Person = _db.Persons.Find(order.PersonPESEL);
            thisModel.Today = DateTime.Now;
            TimeSpan dateDiff = thisModel.Today - order.LoanDate;
            int minutes = dateDiff.Minutes;
            int hours = dateDiff.Hours;
            int days = dateDiff.Days;
            decimal? cost = 0;

            //10 minute forgiveness and minimal pay
            if (minutes % 60 > 10 || hours < 1)
                hours++;
            //daily wage privilege or if only cost per day is available
            if (order.CostPerDay != null && order.CostPerDay <= hours * order.CostPerHour || order.CostPerHour == null)
                days++;
            //cost per hour
            else
                cost = hours * order.CostPerHour * order.Amount;
            //cost per day
            if (order.CostPerDay > 0)
                cost += days * order.CostPerDay * order.Amount;
            //overall cost after deducting the advance payment
            thisModel.Cost = cost - order.Advance;
            
            return View(thisModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(OrderModel order, decimal? cost, ItemModel item)
        {
            order.ReturnDate = DateTime.Now;
            order.Cost = cost;
            item.Availability += (int)order.Amount;
            order.Paid = true;
            _db.Items.Update(item);
            _db.Orders.Update(order);
            _db.SaveChanges();

            TempData["success"] = $"Zapisano wpłatę.";
            return RedirectToAction("Details");
        }

        public IActionResult DetailsRepair(int? id)
        {
            dynamic thisModel = new ExpandoObject();
            OrderModel? order = _db.Orders.Find(id);
            thisModel.Order = order;
            thisModel.Person = _db.Persons.Find(order.PersonPESEL);

            return View(thisModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DetailsRepairPaid(OrderModel order, decimal cost)
        {
            order.ReturnDate = DateTime.Now;
            order.Cost = cost;
            order.Paid = true;
            _db.Orders.Update(order);
            _db.SaveChanges();

            TempData["success"] = $"Zapisano wydanie sprzętu.";
            return RedirectToAction("Index");
        }
    }
}
