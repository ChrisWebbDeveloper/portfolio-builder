using PortfolioBuilder.Data;
using PortfolioBuilder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioBuilder.Controllers
{
    [Authorize(Roles = "Superuser, User Manager")]
    [Route("Users")]
    public class UsersController : ApplicationController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : base(context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users;

            var viewModel = new List<UserViewModel>();

            foreach (var user in users)
            {
                var userModel = new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email
                };

                viewModel.Add(userModel);
            }

            return View(viewModel);
        }

        [Route("Add")]
        public IActionResult Create()
        {
            ViewData["AllRoles"] = _roleManager.Roles;
            CreateUserViewModel viewModel = new CreateUserViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Create(CreateUserViewModel viewModel, string[] roleNames)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    // Add User
                    IdentityUser user = new IdentityUser
                    {
                        UserName = viewModel.Email,
                        Email = viewModel.Email,
                        EmailConfirmed = true,
                    };

                    var userResult = await _userManager.CreateAsync(user, viewModel.Password);

                    if (userResult.Succeeded)
                    {
                        // Roles, only to do when user added
                        if (roleNames.Length > 0)
                        {
                            var roleNamesList = roleNames.ToList();

                            for (int i = roleNamesList.Count - 1; i >= 0; i--)
                            {
                                if (!User.IsInRole(roleNamesList[i]) && !User.IsInRole("Superuser"))
                                {
                                    roleNamesList.RemoveAt(i);
                                }
                            }

                            var roles = roleNamesList.ToArray();

                            var rolesResult = await _userManager.AddToRolesAsync(user, roles);

                            if (rolesResult.Succeeded)
                            {
                                await transaction.CommitAsync();
                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                await transaction.RollbackAsync();
                            }

                            foreach (IdentityError error in rolesResult.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                        else
                        {
                            await transaction.CommitAsync();
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                    }

                    foreach (IdentityError error in userResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            ViewData["AllRoles"] = _roleManager.Roles;
            return View(viewModel);
        }

        [Route("{Id}/Edit")]
        public async Task<IActionResult> Edit(string Id)
        {
            ViewData["AllRoles"] = _roleManager.Roles;

            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var viewModel = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                RoleNames = roles
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("{Id}/Edit")]
        public async Task<IActionResult> Edit(string Id, [Bind("Id, Email, RoleNames")] UserViewModel viewModel, string[] roleNames)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    // Update email
                    var userToUpdate = await _userManager.FindByIdAsync(Id);
                    userToUpdate.UserName = viewModel.Email;
                    userToUpdate.Email = viewModel.Email;

                    IdentityResult userResult = await _userManager.UpdateAsync(userToUpdate);

                    // Add and remove roles
                    var currentUserRoles = await _userManager.GetRolesAsync(userToUpdate);

                    var rolesToAdd = new List<string>();
                    var rolesToRemove = new List<string>();

                    foreach (var role in _roleManager.Roles)
                    {
                        if (User.IsInRole(role.Name) || User.IsInRole("Superuser"))
                        {
                            if (roleNames.ToList().Any(x => x == role.Name) && !currentUserRoles.Any(x => x == role.Name))
                            {
                                rolesToAdd.Add(role.Name);
                            }
                            else if (currentUserRoles.Any(x => x == role.Name))
                            {
                                rolesToRemove.Add(role.Name);
                            }
                        }
                    }

                    IdentityResult rolesAddResult = await _userManager.AddToRolesAsync(userToUpdate, rolesToAdd);
                    IdentityResult rolesRemoveResult = await _userManager.RemoveFromRolesAsync(userToUpdate, rolesToRemove);

                    if (userResult.Succeeded && rolesAddResult.Succeeded && rolesRemoveResult.Succeeded)
                    {
                        await transaction.CommitAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                    }

                    foreach (IdentityError error in userResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    foreach (IdentityError error in rolesAddResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    foreach (IdentityError error in rolesRemoveResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    viewModel.RoleNames = await _userManager.GetRolesAsync(userToUpdate);
                }
            }

            ViewData["AllRoles"] = _roleManager.Roles;
            return View(viewModel);
        }

        [Route("{Id}/Delete")]
        public async Task<IActionResult> Delete(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            var viewModel = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email
            };

            return View(viewModel);
        }

        // POST: Portfolios/5/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("{Id}/Delete")]
        public async Task<IActionResult> DeleteConfirmed(string Id)
        {
            var userToDelete = await _userManager.FindByIdAsync(Id);
            await _userManager.DeleteAsync(userToDelete);

            return RedirectToAction(nameof(Index));
        }
    }
}
