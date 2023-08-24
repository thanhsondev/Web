using BookShoppingCartMvcUI.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nine.Data;
using Nine.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nine.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AdminController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;           
        }
       
            //=====================================================================================================//

            [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ShowUser()
        {
            var user = await (from users in _context.Users
                              join UserRole in _context.UserRoles
                              on users.Id equals UserRole.UserId
                              join role in _context.Roles
                              on UserRole.RoleId equals role.Id
                              where role.Name == "User"
                              select users).ToListAsync();
            return View(user);
        }
        //=====================================================================================================//
       
        public async Task<IActionResult> ChangeUser(string id)
        {
			// Retrieve the user by their ID
			var user = await _userManager.FindByIdAsync(id);

			if (user == null)
			{
				return NotFound(); // User not found, return a 404 error
			}

			// You may want to load additional data or perform other operations here
			// For example, you can load user-related data, roles, or any other necessary information.

			return View(user); // Return a view for editing the user
		}
//=====================================================================================================//

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> ChangeUserRoleToOwner(string id)
		{
			// Retrieve the user by their ID
			var user = await _userManager.FindByIdAsync(id);

			if (user == null)
			{
				return NotFound(); // User not found, return a 404 error
			}

			// Remove the "User" role and add the "Owner" role
			var removeUserResult = await _userManager.RemoveFromRoleAsync(user, "User");
			if (!removeUserResult.Succeeded)
			{
				ModelState.AddModelError(string.Empty, "Failed to remove User role.");
				return View("ChangeUser", user);
			}

			var addOwnerResult = await _userManager.AddToRoleAsync(user, "Owner");
			if (!addOwnerResult.Succeeded)
			{
				ModelState.AddModelError(string.Empty, "Failed to add Owner role.");
				return View("ChangeUser", user);
			}

			return RedirectToAction("ShowUser"); // Redirect to the user list after successful role change
		}
//=====================================================================================================//

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> UpdateUser(ApplicationUser model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(model.Id);

				if (user == null)
				{
					return NotFound(); // User not found, return a 404 error
				}

				// Update the user's properties
				user.FullName = model.FullName;
				user.Email = model.Email;
				user.Address = model.Address;

				// Save the changes to the database
				var result = await _userManager.UpdateAsync(user);

				if (result.Succeeded)
				{
					return RedirectToAction("ShowUser"); // Redirect to the user list after successful update
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			// If ModelState is not valid, return to the edit view with validation errors
			return View("ChangeUser", model);
		}
		//=====================================================================================================//


		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> ChangePasswordUser(string id)
		{
			// Retrieve the user by their ID
			var user = await _userManager.FindByIdAsync(id);

			if (user == null)
			{
				return NotFound(); // User not found, return a 404 error
			}

			return View(user); // Return a view for changing the user's password
		}
		//=====================================================================================================//

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> UpdatePasswordUser(string id, string NewPassword, string ConfirmPassword)
		{
			// Retrieve the user by their ID
			var user = await _userManager.FindByIdAsync(id);

			if (user == null)
			{
				return NotFound(); // User not found, return a 404 error
			}

			// Check if NewPassword and ConfirmPassword match
			if (NewPassword != ConfirmPassword)
			{
				ModelState.AddModelError(string.Empty, "The New Password and Confirm Password do not match.");
				return View("ChangePassword", user);
			}

			// Change the user's password
			var token = await _userManager.GeneratePasswordResetTokenAsync(user);
			var result = await _userManager.ResetPasswordAsync(user, token, NewPassword);

			if (result.Succeeded)
			{
				return RedirectToAction("ShowUser"); // Redirect to the user list after successful password change
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}

			return View("ChangePasswordUser", user); // If there are errors, return to the change password form
		}

		//=====================================================================================================//
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> ShowOwner()
		{
			var owner = await (from users in _context.Users
							   join UserRole in _context.UserRoles
							   on users.Id equals UserRole.UserId
							   join role in _context.Roles
							   on UserRole.RoleId equals role.Id
							   where role.Name == "Owner"
							   select users).ToListAsync();
			return View(owner);
		}

		//=====================================================================================================//

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> ChangeOwner(string id)
		{
			// Retrieve the owner by their ID
			var owner = await _userManager.FindByIdAsync(id);

			if (owner == null)
			{
				return NotFound(); // Owner not found, return a 404 error
			}

			return View(owner); // Return a view for editing the owner information
		}
//=====================================================================================================//

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> UpdateOwner(ApplicationUser model)
		{
			if(ModelState.IsValid)
			{
				var owner= await _userManager.FindByIdAsync(model.Id);

				if (owner == null)
				{
					return NotFound(); // User not found, return a 404 error
				}

				// Update the user's properties
				owner.FullName = model.FullName;
				owner.Email = model.Email;
				owner.Address = model.Address;

				// Save the changes to the database
				var result = await _userManager.UpdateAsync(owner);

				if (result.Succeeded)
				{
					return RedirectToAction("ShowOwner"); // Redirect to the user list after successful update
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			// If ModelState is not valid, return to the edit view with validation errors
			return View("ChangeOwner", model);
		}

		

		//=====================================================================================================//

		

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> ChangePasswordOwner(string id)
		{
			// Retrieve the user by their ID
			var owner = await _userManager.FindByIdAsync(id);

			if (owner == null)
			{
				return NotFound(); // User not found, return a 404 error
			}

			return View(owner); // Return a view for changing the user's password
		}
		//=====================================================================================================//

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> UpdatePasswordOwner(string id, string NewPassword, string ConfirmPassword)
		{
			// Retrieve the user by their ID
			var owner = await _userManager.FindByIdAsync(id);

			if (owner == null)
			{
				return NotFound(); // User not found, return a 404 error
			}

			// Check if NewPassword and ConfirmPassword match
			if (NewPassword != ConfirmPassword)
			{
				ModelState.AddModelError(string.Empty, "The New Password and Confirm Password do not match.");
				return View("ChangePassword", owner);
			}

			// Change the user's password
			var token = await _userManager.GeneratePasswordResetTokenAsync(owner);
			var result = await _userManager.ResetPasswordAsync(owner, token, NewPassword);

			if (result.Succeeded)
			{
				return RedirectToAction("ShowUser"); // Redirect to the user list after successful password change
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}

			return View("ChangePassword", owner); // If there are errors, return to the change password form
		}
		//=====================================================================================================//
		// Show toàn bộ Genre
		[Authorize(Roles = "Admin")]
        public IActionResult ManageGenre()
        {
            var category = _context.Genres.ToList();
            return View(category);
        }

//=====================================================================================================//

        // click button đổi status Genre
        [Authorize(Roles = "Admin")]
        public IActionResult Accept(int id)
        {
            Genre genre = _context.Genres.Find(id);
            if (genre == null)
            {
                return RedirectToAction("ManageGenre");
            }
            else
            {
                genre.GenreStatus = "accepted";
                _context.Genres.Update(genre);
                _context.SaveChanges();
                return RedirectToAction("ManageGenre");
            }

        }

//=====================================================================================================//
        //Delete Genre
        public IActionResult Delete(int id)
        {
            Genre genre = _context.Genres.Find(id);
            if (genre == null)
            {
                return RedirectToAction("ManageGenre");
            }
            else
            {
                _context.Genres.Remove(genre);
                _context.SaveChanges();
                return RedirectToAction("ManageGenre");
            }
        }

//=====================================================================================================//
        
		public async Task<IActionResult> DeleteUser(string id)
		{
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                return RedirectToAction("ShowUser");

			}
			else
            {
                var result = await _userManager.DeleteAsync(user);
                if(result.Succeeded)
                {
                    return RedirectToAction("ShowUser");
				}  
                return RedirectToAction("ShowUser");
			}
		}

//=====================================================================================================//

        public async Task<IActionResult> DeleteOwner(string id)
		{
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                return RedirectToAction("ShowOwner");

			}
			else
            {
                var result = await _userManager.DeleteAsync(user);
                if(result.Succeeded)
                {
                    return RedirectToAction("ShowOwner");
				}  
                return RedirectToAction("ShowOwner");
			}
		}


		

		


	}
}
