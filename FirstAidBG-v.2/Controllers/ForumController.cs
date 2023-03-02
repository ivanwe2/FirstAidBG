#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FirstAidBG_v._2.Data.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

using System.Security.Claims;
using FirstAidBG_v._2.Data;
using FirstAidBG_v._2.Business.Services;
using FirstAidBG_v._2.Data.Seeder;

namespace FirstAidBG_v._2.Controllers
{
    public class ForumController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserService _userService;
        IHttpContextAccessor _httpContextAccessor;
        
        
        public ForumController(ApplicationDbContext context, UserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            
        }

        // GET: Forum
        [Authorize]
        public async Task<IActionResult> Index()
        {
            
            var questions = await _context.Questions.ToListAsync();
            foreach (var question in questions)
            {
                if (question.User == null)
                {
                    question.User = await _context.AppUsers.FirstOrDefaultAsync(i => i.Id == question.UserId);
                }                
            }
            return View(questions);
        }

        // GET: Forum/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Question question = await _context.Questions
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (question.User == null)
            {
                question.User = await _context.AppUsers.FirstOrDefaultAsync(i => i.Id == _context.Questions
                    .FirstOrDefault(m => m.Id == id).User.Id);
            }
            if (question.Answers == null)
            {

                question.Answers = _context.Answers.Where(c => c.IdForQuestion == id).ToList();
                foreach (var answer in question.Answers)
                {
                    if (answer.AppUser == null)
                    {
                        answer.AppUser = _context.AppUsers.FirstOrDefault(i => i.Id == answer.AppUserId);
                    }
                }

            }
            _context.AppUsers.FirstOrDefault(i => i.Id == 1).Username = "Admin";
            _context.SaveChanges();


            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Forum/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Forum/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestionId,Text")] Question question)
        {
            question.DatePublished = DateTime.Now;
            question.User = _userService.GetUserByNameIdentifier(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            question.Answers = new List<Answer>();
            //if (ModelState.IsValid)
            //{
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            //}
            //return View(question);
        }

        // GET: Forum/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        // POST: Forum/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Text")] Question question)
        {
            Question questionOld = await _context.Questions.FindAsync(id);

            questionOld.Text = question.Text;

            //if (ModelState.IsValid)
            //{
            try
            {
                _context.Update(questionOld);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(question.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
            //}
            //return View(question);
        }

        // GET: Forum/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Forum/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            foreach (var item in _context.Answers)
            {
                if (item.IdForQuestion == id)
                {
                    _context.Answers.Remove(item);
                }
            }
            var question = await _context.Questions.FindAsync(id);
            _context.Questions.Remove(question);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }


    }
}
