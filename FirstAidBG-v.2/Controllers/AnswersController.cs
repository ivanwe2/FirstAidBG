#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FirstAidBG_v._2.Data;
using FirstAidBG_v._2.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using FirstAidBG_v._2.Business.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace FirstAidBG_v._2.Controllers
{
    public class AnswersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserService _userService;
        private IHttpContextAccessor _httpContextAccessor;

        public AnswersController(ApplicationDbContext context, UserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Answers
        public async Task<IActionResult> Index()
        {
            return View("Index", "Forum");
        }

        // GET: Answers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers
                .Include(a => a.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // GET: Answers/Create
        // GET: Forum/Create
        [Authorize]
        public IActionResult CreateAnswer()
        {
            return View();
        }

        // POST: Forum/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAnswer([Bind("Text")] Answer answer, int? id)
        {
            answer.DatePublished = DateTime.Now;
            answer.AppUser = _userService.GetUserByNameIdentifier(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (answer.AppUser == null)
            {
                answer.AppUser = new AppUser { NameIdentifier = User.Claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value };
            }
            answer.IdForQuestion = (int)id;
            //if (ModelState.IsValid)
            //{
            _context.Answers.Add(answer);
            Question question = await _context.Questions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question.Answers == null)
            {
                question.Answers = new List<Answer>();
                question.Answers.Add(answer);
            }
            else
            {
                
                question.Answers.Add(answer);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Forum");

            //ne bachka dava greshka return View(Details(id));
        }
        //delete issues fixed
        //answers count need a backdoor (they still null)

        //

    

    // GET: Answers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }
            
            return View(answer);
        }

        // POST: Answers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Text,DatePublished,AppUserId,IdForQuestion,Id")] Answer answer)
        {
            Answer answerOld = await _context.Answers.FindAsync(id);

            answerOld.Text = answer.Text;
            try
            {
                _context.Update(answerOld);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(answer.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            //return View("Index", "Forum");
            return RedirectToAction("Index", "Forum");
        }

        // GET: Answers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers
                .Include(a => a.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Forum");
        }

        private bool AnswerExists(int id)
        {
            return _context.Answers.Any(e => e.Id == id);
        }
    }

}

