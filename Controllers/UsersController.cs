using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Controllers
{
	[Authorize]
	public class UsersController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ApplicationDbContext _context;

        public UsersController(
			ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
		    SignInManager<ApplicationUser> signInManager)
        {
			this._context = context;
            this._userManager = userManager;
			this._roleManager = roleManager;
			this._signInManager = signInManager;
        }
        public async Task<ActionResult> Index()
		{
			var users = await _context.Users.Include(x=>x.Role).ToListAsync();
			return View(users);
		}

		[HttpGet]
		public async Task<ActionResult> Create()
		{
			ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> Create(UserViewModel model)
		{
            ApplicationUser user = new ApplicationUser();
			user.UserName = model.UserName;
			user.FirstName = model.FirstName;
			user.MiddleName = model.MiddleName;
			user.LastName = model.LastName;
			user.NationalId = model.NationalId;
			user.NormalizedUserName = model.UserName;
			user.Email = model.Email;
			user.EmailConfirmed = true;
			user.PhoneNumber = model.PhoneNumber;
			user.PhoneNumberConfirmed = true;
			user.CreatedOn = DateTime.Now;
			user.CreatedById = "Pranay";
			user.RoleId = model.RoleId;
			var result = await _userManager.CreateAsync(user,model.Password);

			if (result.Succeeded)
			{
				return RedirectToAction("Index");
			}
			else
			{
				return View(model);
			}
			ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name",model.RoleId);
		}

		

	}
}
