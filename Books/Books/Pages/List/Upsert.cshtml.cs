using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Books.Pages.List
{
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public UpsertModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Book Book { get; set; } = new Book();

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
                return Page();

            Book = await _db.Books.FindAsync(id);
            if (Book == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if(Book.Id == 0)
                {
                    await _db.Books.AddAsync(Book);
                }
                else
                {
                    var bookFromDb = await _db.Books.FindAsync(Book.Id);
                    bookFromDb.Name = Book.Name;
                    bookFromDb.Author = Book.Author;
                    bookFromDb.ISBN = Book.ISBN;
                }
                await _db.SaveChangesAsync();
                return RedirectToPage("Index");
            }
            else
                return Page();
        }
    }
}