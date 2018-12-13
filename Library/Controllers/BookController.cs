using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using System;
using System.Linq;


namespace Library.Controllers
{
    public class BookController : Controller
    {
        [HttpGet("/books")]
        public ActionResult Index()
        {   
            List<Book> allBooks = Book.GetAll();
            return View(allBooks);
        }

        [HttpGet("/books/new")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost("/books")]
        public ActionResult Create(string name, int copies, string author)
        {
            Book foundBook = Book.FindTitle(name);
            if(foundBook.GetName() == "")
            {
                Book newBook = new Book(name, copies, author);
                newBook.Save();
            }
            List<Book> allBooks = Book.GetAll();
            return View("Index", allBooks);
        }
        
        [HttpGet("/books/{id}")]
        public ActionResult Show(int id)
        {   
            Book foundBook = Book.Find(id);
            List<Patron> foundPatrons = foundBook.GetPatrons();
            List<Patron> allPatrons = Patron.GetAll();
            Dictionary<string, object> myDic = new Dictionary<string,object> ();
            myDic.Add("book", foundBook);
            myDic.Add("patrons", foundPatrons);
            myDic.Add("allPatrons", allPatrons);
            return View(myDic);
        }

        [HttpGet("/books/search")]
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost("/books/search")]
        public ActionResult Author(string author)
        {
            List<Book> foundBooks = Book.FindAuthor(author);
            return View(foundBooks);
        }

        [HttpGet("/books/searchTitle")]
        public ActionResult SearchTitle()
        {
            return View();
        }

        [HttpPost("/books/searchTitle")]
        public ActionResult SearchTitle(string name)
        {
            Book foundBook = Book.FindTitle(name);
            return View("Title", foundBook);
        }

        [HttpPost("/books/{id}")]
        public ActionResult AddPatron(int id, int patronId)
        {
            Patron foundPatron = Patron.Find(patronId);
            Book foundBook = Book.Find(id);
            List<Patron> foundPatrons = foundBook.GetPatrons();

            bool isUniquePatron = true; 
            for (int i = 0; i < foundPatrons.Count; i++)
            {
                if (patronId == foundPatrons[i].GetId())
                {
                    isUniquePatron = false;
                }
            }

            if (isUniquePatron)
            {
                foundBook.AddPatron(foundPatron);
                int copies = foundBook.GetCopies();
                foundBook.EditCopies(copies-1);
            }
            
            return RedirectToAction("Show");
        }

    }
}
