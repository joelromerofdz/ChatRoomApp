using ChatRoomApp.Controllers.Base;
using ChatRoomApp.Helpers.Dtos;
using ChatRoomApp.Helpers.Interfaces;
using ChatRoomApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoomApp.Controllers
{
    public class LoginController : BaseChatRoomController
    {
        public readonly IUserHelper _userHelper;

        public LoginController(IUserHelper userHelper)
        {
            _userHelper = userHelper;
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (_userHelper.IsUserAuthenticated())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginRequest login)
        {
            if (ModelState.IsValid)
            {
                var singInResult = await _userHelper.LogInUser(login, false);
                if (singInResult.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "User or Password incorrect.");
            }

            return View(login);
        }

        public ActionResult Register()
        {
            if (_userHelper.IsUserAuthenticated())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(RegisterViewModel register)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Register", register);
                }

                var r = await _userHelper.AddUserSync(register);
                if (r.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                GetUserModelStateErrors(r.Errors, ModelState);
                return View("Register", register);
            }
            catch (Exception ex)
            {
                return View("Register", register);
            }
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _userHelper.LogOut();
            return RedirectToAction("Login", "Login");
        }

    }
}
