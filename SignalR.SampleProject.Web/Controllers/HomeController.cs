using Microsoft.AspNetCore.Mvc;
using SignalR.SampleProject.Web.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SignalR.SampleProject.Web.Models.ViewModels;
using SignalR.SampleProject.Web.Services;

namespace SignalR.SampleProject.Web.Controllers
{
    public class HomeController
        (
            ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            AppDbContext context,
            FileService fileService
        ) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var userToCreate = new IdentityUser()
            {
                UserName = Guid.NewGuid().ToString(),
                Email = model.Email
            };

            var result = await userManager.CreateAsync(userToCreate, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty,error.Description);
                }
            }

            return RedirectToAction(nameof(SignIn));
        }

        public IActionResult SignIn()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var hasUser = await userManager.FindByEmailAsync(model.Email);
            if (hasUser is null)
            {
                ModelState.AddModelError(string.Empty,"Email or Password is wrong!" );
            }

            var result = await signInManager.PasswordSignInAsync(hasUser!, model.Password, true,false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email or Password is wrong!");
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ProductList()
        {
            var currentUser = await userManager.FindByEmailAsync("ihsanemirkuzucu@gmail.com");

            if (context.Products.Any(x => x.UserId == currentUser.Id.ToString()))
            {
                var products = context.Products.Where(x => x.UserId == currentUser.Id.ToString()).ToList();
                return View(products);
            }

            var productList = new List<Product>()
            {
                new Product() { Name = "Pen 1", Description = "Des 1", Price = 100, UserId = currentUser.Id.ToString() },
                new Product() { Name = "Pen 2", Description = "Des 2", Price = 200, UserId = currentUser.Id.ToString() },
                new Product() { Name = "Pen 3", Description = "Des 3", Price = 300, UserId = currentUser.Id.ToString() },
                new Product() { Name = "Pen 4", Description = "Des 4", Price = 400, UserId = currentUser.Id.ToString() },
                new Product() { Name = "Pen 5", Description = "Des 5", Price = 500, UserId = currentUser.Id.ToString() }
            };
            await context.Products.AddRangeAsync(productList);
            await context.SaveChangesAsync();

            return View(productList);
        }

        [Authorize]
        public async Task<IActionResult> CreateExcell()
        {
            var response = new
            {
                Status = await fileService.AddMessageToQueue()
            };
            return Json(response);
        }

    }
}
