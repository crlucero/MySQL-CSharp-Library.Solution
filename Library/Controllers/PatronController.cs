using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Library.Models;

namespace Library.Controllers
{
    public class PatronController : Controller
    {
        [HttpGet("/patrons")]
        public ActionResult Index()
        {   
            List<Patron> allPatrons = Patron.GetAll();
            return View(allPatrons);
        }

        [HttpGet("/patrons/{id}")]
        public ActionResult Show(int id)
        {
            Patron foundPatron = Patron.Find(id);
            List<Book> foundBooks = foundPatron.GetBooks();
            Dictionary<string, object> myDic = new Dictionary<string, object> ();
            myDic.Add("patron", foundPatron);
            myDic.Add("books", foundBooks);
            return View(myDic);

        }

        [HttpGet("/patrons/new")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost("/patrons")]
        public ActionResult Create(string name)
        {
            Patron foundPatron = Patron.FindByName(name);
            if(foundPatron.GetName() == "")
            {
                Patron newPatron = new Patron(name);
                newPatron.Save();
            }
            List<Patron> allPatrons = Patron.GetAll();
            return View("Index", allPatrons);
        }

    }
}