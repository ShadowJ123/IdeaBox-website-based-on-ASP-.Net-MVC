using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CaseStudy_Team2_Final.Data;
using CaseStudy_Team2_Final.Models;
using Microsoft.AspNetCore.Authorization;

namespace CaseStudy_Team2_Final.Controllers
{
    //[Authorize]
    [Authorize(Roles ="ADMIN")]
    public class AdminController : Controller
    {
        private readonly IdeaContext _context;

        public AdminController(IdeaContext context)
        {
            _context = context;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            var IdeaList1 = from s in _context.Idea
                            select s;
            IdeaList1 = IdeaList1.Where(s => s.Status.Contains("Pending"));
            return View(await IdeaList1.ToListAsync());
        }
        [ActionName("Index")]
        [HttpPost]
        public IActionResult Index(string SubmitButton, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string buttonClicked = SubmitButton;
            if (buttonClicked == "Approve")
            {
                var idea = _context.Idea
                .FirstOrDefault(m => m.IdeaID == id);
                if (idea != null)
                {
                    idea.Status = "Approved";
                    _context.Idea.Update(idea);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction(nameof(Index));
        }
        // GET: Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var idea = await _context.Idea
                .FirstOrDefaultAsync(m => m.IdeaID == id);
            if (idea == null)
            {
                return NotFound();
            }

            return View(idea);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdeaID,ContributedBy,Date,Title,Description,Tag,Rating,Status")] Idea idea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(idea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(idea);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var idea = await _context.Idea.FindAsync(id);
            if (idea == null)
            {
                return NotFound();
            }
            return View(idea);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdeaID,ContributedBy,Date,Title,Description,Tag,Rating,Status")] Idea idea)
        {
            if (id != idea.IdeaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(idea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IdeaExists(idea.IdeaID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(idea);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var idea = await _context.Idea
                .FirstOrDefaultAsync(m => m.IdeaID == id);
            if (idea == null)
            {
                return NotFound();
            }

            return View(idea);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var idea = await _context.Idea.FindAsync(id);
            _context.Idea.Remove(idea);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IdeaExists(int id)
        {
            return _context.Idea.Any(e => e.IdeaID == id);
        }
    }
}
