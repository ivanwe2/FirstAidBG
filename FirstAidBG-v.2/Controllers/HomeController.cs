
using FirstAidBG_v._2.Business.Models;
using FirstAidBG_v._2.Business.Services;
using FirstAidBG_v._2.Data;
using FirstAidBG_v._2.Data.Seeder;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FirstAidBG_v._2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService _userService;
        private readonly ApplicationDbContext _context;
        
        public HomeController(ILogger<HomeController> logger, UserService userService, ApplicationDbContext context)
        {
            _logger = logger;
            _userService = userService;
            _context = context;
            
        }

        //possible exception
        public async Task<IActionResult> Index()
        {
            if (_context.AppUsers.Any() || _context.Answers.Any() || _context.Questions.Any())
            {
                return View();
            }
            else
            {
                await Seed.SeedUsersAsync(_context);
                await Seed.SeedQuestionsAsync(_context);
                await Seed.SeedAnswersAsync(_context);
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SecuredAsync()
        {
            var idToken = await HttpContext.GetTokenAsync("id_token");
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}