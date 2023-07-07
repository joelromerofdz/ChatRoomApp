using ChatRoomApp.Controllers.Base;
using ChatRoomApp.Helpers.Dtos;
using ChatRoomApp.Helpers.Interfaces;
using ChatRoomApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoomApp.Controllers
{
    public class LoginController : BaseChatRoomController
    {
        private readonly IChatRoomAppRepository _repository;

        public LoginController(IChatRoomAppRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (_repository.UserHelper.IsUserAuthenticated())
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
                var singInResult = await _repository.UserHelper.LogInUser(login, false);
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
            if (_repository.UserHelper.IsUserAuthenticated())
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

                var r = await _repository.UserHelper.AddUserSync(register);
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
            await _repository.UserHelper.LogOut();
            return RedirectToAction("Login", "Login");
        }

    }
}
