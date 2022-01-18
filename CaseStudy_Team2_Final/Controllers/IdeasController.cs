using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CaseStudy_Team2_Final.Data;
using CaseStudy_Team2_Final.Models;
using PagedList;
using Microsoft.AspNetCore.Authorization;

namespace CaseStudy_Team2_Final.Controllers
{
    [Authorize]
    public class IdeasController : Controller
    {
        private readonly IdeaContext _context;
        private readonly EmpContext _ctx;
        public IdeasController(IdeaContext context, EmpContext ct)
        {
            _context = context;
            _ctx = ct;
        }
        // GET: Ideas
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["RateSortParam"] = sortOrder == "Rate" ? "rate_desc" : "Rate";
            ViewData["CurrentFilter"] = searchString;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            var idealist = from s in _context.Idea
                           select s;
            idealist = idealist.Where(s => s.ContributedBy.Contains(User.Identity.Name));
            if (!String.IsNullOrEmpty(searchString))
            {
                idealist = idealist.Where(s => s.ContributedBy.Contains(searchString)
                                       || s.Title.Contains(searchString) || s.Status.Contains(searchString) || s.Tag.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    idealist = idealist.OrderByDescending(s => s.ContributedBy);
                    break;
                case "Date":
                    idealist = idealist.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    idealist = idealist.OrderByDescending(s => s.Date);
                    break;
                case "Rate":
                    idealist = idealist.OrderBy(s => s.Rating);
                    break;
                case "rate_desc":
                    idealist = idealist.OrderByDescending(s => s.Rating);
                    break;
                default:
                    idealist = idealist.OrderBy(s => s.ContributedBy);
                    break;
            }
            int pageSize = 4;
            return View(await PaginatedList<Idea>.CreateAsync(idealist.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        public async Task<IActionResult> Search(string sortOrder, string currentFilter, string searchString,int? pageNumber)
        {
            //decimal ratetemp;
            //if (star == "star-5")
            //{
            //    ratetemp = 5;
            //}
            //else if (star == "star-4")
            //{
            //    ratetemp = 4;
            //}
            //else if (star == "star-3")
            //{
            //    ratetemp = 3;
            //}
            //else if (star == "star-2")
            //{
            //    ratetemp = 2;
            //}
            //else if (star == "star-1")
            //{
            //    ratetemp = 1;
            //}
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["RateSortParam"] = sortOrder == "Rate" ? "rate_desc" : "Rate";
            ViewData["CurrentFilter"] = searchString;
           // ViewData["ratingbtn"] = searchString;
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            var idealist = from s in _context.Idea
                           select s;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                idealist = idealist.Where(s => s.ContributedBy.Contains(searchString)
                                       || s.Title.Contains(searchString) || s.Status.Contains(searchString) || s.Tag.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    idealist = idealist.OrderByDescending(s => s.ContributedBy);
                    break;
                case "Date":
                    idealist = idealist.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    idealist = idealist.OrderByDescending(s => s.Date);
                    break;
                case "Rate":
                    idealist = idealist.OrderBy(s => s.Rating);
                    break;
                case "rate_desc":
                    idealist = idealist.OrderByDescending(s => s.Rating);
                    break;
                default:
                    idealist = idealist.OrderBy(s => s.ContributedBy);
                    break;
            }
            int pageSize = 3;
            
            return View(await PaginatedList<Idea>.CreateAsync(idealist.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [ActionName("Search")]
        [HttpPost]
        public IActionResult Search(string myrating, string SubmitButton, int? id)
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
                    if (myrating == "5")
                    {
                        idea.Rating = idea.Rating + (5 - idea.Rating) / 2 + (5 - idea.Rating) / 4 ;
                    }
                    else if (myrating == "4")
                    {
                        idea.Rating = idea.Rating + (4 - idea.Rating) / 2 + (4 - idea.Rating) / 4 ;
                    }
                    else if (myrating == "3")
                    {
                        idea.Rating = idea.Rating + (3 - idea.Rating) / 2 + (3 - idea.Rating) / 4;
                    }
                    else if (myrating == "2")
                    {
                        idea.Rating = idea.Rating + (2 - idea.Rating) / 2 + (2 - idea.Rating) / 4 ;
                    }
                    else if (myrating == "1")
                    {
                        idea.Rating = idea.Rating + (1 - idea.Rating) / 2 + (1 - idea.Rating) / 4 ;
                    }
                    //int temp = idea.Rating + (ratetemp - idea.Rating) / 2 + (ratetemp - idea.Rating) / 4 + (ratetemp - idea.Rating) / 8;
                    //idea.Status = "Approved";
                    _context.Idea.Update(idea);
                    _context.SaveChanges();
                }
            }
           
            return RedirectToAction("Search", "Ideas");
        }
        public IActionResult Dashboard()
        {
            var IdeaList1 = from s in _context.Idea
                           select s;
            IdeaList1 = IdeaList1.Where(s => s.Status.Contains("Approved"));
            IdeaList1 = IdeaList1.OrderByDescending(s => s.Rating).Take(10);
            var model = new collection();
            model.emplist = _ctx.Emp.ToList();
            model.idealist = IdeaList1.ToList();
            return View(model);
        }

        //[Authorize]
        public IActionResult Approve()
        {
            var IdeaList1 = from s in _context.Idea
                            select s;
            IdeaList1 = IdeaList1.Where(s => s.Status.Contains("Pending"));
            return View( _context.Idea.ToList());
        }

        //[ActionName("Approve")]
        //[HttpPost]
        //public IActionResult Approve(string SubmitButton, int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    string buttonClicked = SubmitButton;
        //    if (buttonClicked == "Approve")
        //    {
        //        var idea = _context.Idea
        //        .FirstOrDefault(m => m.IdeaID == id);
        //        if (idea != null)
        //        {
        //            idea.Status = "Approved";
        //            _context.Idea.Update(idea);
        //            _context.SaveChanges();
        //        }
        //    }
        //    return RedirectToAction(nameof(Index));
        //}

        // GET: Ideas/Details/5
        //[Authorize(Roles = "ADMIN")]
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

        // GET: Ideas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ideas/Create
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
       
        // GET: Ideas/Edit/5
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

        // POST: Ideas/Edit/5
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
                    var DbIdea = _context.Idea.Where(s => s.IdeaID == idea.IdeaID).FirstOrDefault();
                    decimal newRating = DbIdea.Rating + (idea.Rating- DbIdea.Rating) / 2+ (idea.Rating - DbIdea.Rating) / 4+ (idea.Rating - DbIdea.Rating) / 8;
                    DbIdea.Rating = newRating;
                    _context.Update(DbIdea);
                   // _context.Update(idea);
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
                if (User.Identity.Name == "chhayaprakashj@gmail.com")
                {
                    return RedirectToAction("Search", "ideas");
                }
                else
                    return RedirectToAction(nameof(Index));
            }
            return View(idea);
        }

        // GET: Ideas/Delete/5
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

        // POST: Ideas/Delete/5
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
