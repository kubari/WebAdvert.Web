using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAdvert.Web.Models.Accounts;

namespace WebAdvert.Web.Controllers
{
	public class AccountsController : Controller
	{
		private readonly SignInManager<CognitoUser> _signInManager;
		private readonly UserManager<CognitoUser> _userManager;
		private readonly CognitoUserPool _cognitoUserPool;

		public AccountsController(SignInManager<CognitoUser> signInManager, UserManager<CognitoUser> userManager, CognitoUserPool cognitoUserPool)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_cognitoUserPool = cognitoUserPool;
		}

		public async Task<IActionResult> Signup()
		{
			var model = new SignupModel();

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Signup(SignupModel model)
		{
			await _signInManager.SignOutAsync();

			if (ModelState.IsValid)
			{
				var user = _cognitoUserPool.GetUser(model.EMail);
				if (user.Status != null)
				{
					ModelState.AddModelError("UserExists", "User with this email already exists");
					return View(model);
				}

				user.Attributes.Add(CognitoAttribute.Name.AttributeName,
					model.EMail.Substring(0, model.EMail.IndexOf("@")));
				var createdUser = await _userManager.CreateAsync(user, model.Password)
					.ConfigureAwait(false);



				if (createdUser.Succeeded)
				{
					await _signInManager.SignInAsync(user, isPersistent: false);

					return RedirectToAction("Confirm");
				}
			}
			return View();
		}

		public async Task<IActionResult> Confirm()
		{

			return View(new ConfirmModel());
		}

		[HttpPost]
		public async Task<IActionResult> Confirm(ConfirmModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.EMail);

				if (user == null)
				{
					ModelState.AddModelError("NotFound", "A user with the given email is not found");
					return View(model);
				}

				var cognitoUserManager = (CognitoUserManager<CognitoUser>)_userManager;

				var result = await cognitoUserManager.ConfirmSignUpAsync(user, model.Code, true)
					.ConfigureAwait(false);

				if (result.Succeeded)

				{
					return RedirectToAction("Index", "Home");
				}

				foreach (var e in result.Errors)
				{
					ModelState.AddModelError(e.Code, e.Description);
				}
			}
			return View(model);
		}

		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return Redirect("/");
		}
		
		public async Task<IActionResult> Login()
		{
			return View(new LoginModel());
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}


			var result = await _signInManager.PasswordSignInAsync(model.EMail, model.Password, model.RememberMe, false);

			if (result.Succeeded)
			{
				return Redirect("/");
			}

			ModelState.AddModelError("LoggingError", "Email and password do not match");

			return View();
		}
	}
}
