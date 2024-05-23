using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EmployeeManagementSystem.Controllers
{
	[Authorize]
	public class RolesController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ApplicationDbContext _context;

		public RolesController(
			ApplicationDbContext context,
			RoleManager<IdentityRole> roleManager,
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager)
		{
			this._context = context;
			this._roleManager = roleManager;
			this._userManager = userManager;
			this._signInManager = signInManager;
		}
		public async Task<IActionResult> Index()
		{
			var roles = await _context.Roles.ToListAsync();
			return View(roles);
		}

		[HttpGet]
		public async Task<ActionResult> Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> Create(RolesViewModel model)
		{
			IdentityRole role = new IdentityRole();
			role.Name = model.RollName;

			var result = await _roleManager.CreateAsync(role);

			if (result.Succeeded)
			{
				return RedirectToAction("Index");
			}
			else
			{
				return View(model);
			}

		}

		[HttpGet]
		public async Task<ActionResult> Edit(string id)
		{
			var role = new RolesViewModel();

			var result = await _roleManager.FindByIdAsync(id);

			role.RollName = result.Name;
			role.Id = result.Id;
			return View(role);
		}

		[HttpPost]
		public async Task<ActionResult> Edit(string id, RolesViewModel model)
		{

			var checkIfExist = await _roleManager.RoleExistsAsync(model.RollName);

			if (!checkIfExist)
			{

				var result = await _roleManager.FindByIdAsync(id);

				result.Name = model.RollName;

				var finlresult = await _roleManager.UpdateAsync(result);

				if (finlresult.Succeeded)
				{
					return RedirectToAction("Index");
				}
				else
				{
					return View(model);
				}
			}
			else
			{
				
				return View(model);
				
			}
		}
	}
}
