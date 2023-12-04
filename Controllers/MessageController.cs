using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projet.Data;
using Projet.Models;

namespace Projet.Controllers
{
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Messages/Index
        public IActionResult Index()
        {
            var messages = _context.Messages.ToList();
            return View(messages);
        }

        // GET: Messages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Messages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Message message)
        {
            
                _context.Messages.Add(message);
                _context.SaveChanges();
                return RedirectToAction(nameof(Confirmation));
            
           
        }

        // GET: Messages/Confirmation
        public IActionResult Confirmation()
        {
            return View();
        }

        // GET: Messages/Delete/5]
        public async Task<IActionResult> Delete(int id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }

}
