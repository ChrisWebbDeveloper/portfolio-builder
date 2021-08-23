using PortfolioBuilder.Data;
using PortfolioBuilder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PortfolioBuilder.Controllers
{
    [Authorize(Roles = "Superuser")]
    [Route("Roles")]
    public class RolesController : ApplicationController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RolesController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager) : base(context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [Route("Add")]
        public IActionResult Create()
        {
            RoleViewModel roleVM = new RoleViewModel();
            return View(roleVM);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Create(RoleViewModel vm)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = vm.RoleName
                };

                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(vm);
        }

        [Route("{Name}/Edit")]
        public async Task<IActionResult> Edit(string Name)
        {
            if (!await RoleExists(Name))
            {
                return NotFound();
            };

            RoleViewModel roleVM = new RoleViewModel { RoleName = Name };
            return View(roleVM);
        }

        [HttpPost]
        [Route("{Name}/Edit")]
        public async Task<IActionResult> Edit(string Name, [Bind("RoleName")] RoleViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var roleToUpdate = await _roleManager.FindByNameAsync(Name);
                roleToUpdate.Name = vm.RoleName;

                IdentityResult result = await _roleManager.UpdateAsync(roleToUpdate);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            var originalVM = new RoleViewModel { RoleName = Name };
            return View(originalVM);
        }

        [Route("{Name}/Delete")]
        public async Task<IActionResult> Delete(string Name)
        {
            if (!await RoleExists(Name))
            {
                return NotFound();
            };

            RoleViewModel roleVM = new RoleViewModel { RoleName = Name };
            return View(roleVM);
        }

        // POST: Portfolios/5/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("{Name}/Delete")]
        public async Task<IActionResult> DeleteConfirmed(string Name)
        {
            var roleToDelete = await _roleManager.FindByNameAsync(Name);
            await _roleManager.DeleteAsync(roleToDelete);

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> RoleExists(string name)
        {
            return await _roleManager.RoleExistsAsync(name);
        }
    }
}
