using FirstAidBG_v._2.Business.Services;
using FirstAidBG_v._2.Data;
using FirstAidBG_v._2.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstAidBG_v._2.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserService _userService;
        IHttpContextAccessor _httpContextAccessor;
        
        public RegisterController(ApplicationDbContext context, UserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create([Bind("Username,Password,Email")] AppUser user)
        {
            foreach(var item in _context.AppUsers)
            {
                if(item.Username == user.Username)
                {
                    TempData["Error"] = "Грешка. Потребителското име е заето!";
                    return View("Index");
                }
            }
            user.Roles = "Everyone";
            user.NameIdentifier = user.Username;
            user.Provider = "none";
            user.Questions = new List<Question>();
            user.Answers = new List<Answer>();
            //if (ModelState.IsValid)
            //{
            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();
            return View("Details",user);
            //}
            //return View(question);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser user = await _context.AppUsers.FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            
            _context.SaveChanges();

            return View(user);
        }
    }
}
