using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project_I.Web.Models.Accounts;

namespace Project_I.Web.Controllers
{
    public class AccountsController : Controller
    {
        private SignInManager<CognitoUser> _signInManager;
        private UserManager<CognitoUser> _userManager;
        private CognitoUserPool _cognitoUserPool;
        public AccountsController(SignInManager<CognitoUser> signInManager,
            UserManager<CognitoUser> userManager,
            CognitoUserPool cognitoUserPool)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _cognitoUserPool = cognitoUserPool;
        }
        public async Task<IActionResult> SignUp()
        {
            var model = new SignUpModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpModel signUpModel)
        {
            if (ModelState.IsValid)
            {
                var user = _cognitoUserPool.GetUser(signUpModel.Email);
                if (user.Status != null)
                {
                    ModelState.AddModelError("UserExists","User Already Exists");
                    return View(signUpModel);
                }
                user.Attributes.Add(CognitoAttribute.Name.AttributeName, signUpModel.Email);
                var createdUser=await _userManager.CreateAsync(user, signUpModel.Password).ConfigureAwait(false);
                if (createdUser.Succeeded)
                {
                  return  RedirectToAction("Confirm");
                }
            }
            return View();
        }
        public async Task<IActionResult> Confirm()
        {
            var model = new ConfirmModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Confirm(ConfirmModel confirmModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(confirmModel.Email).ConfigureAwait(false);
                if (user == null)
                {
                    ModelState.AddModelError("NotFound", "Email not found");
                    return View(confirmModel);
                }
                //var result = await _userManager.ConfirmEmailAsync(user, confirmModel.Code).ConfigureAwait(false);
                var result = await (_userManager as CognitoUserManager<CognitoUser>).ConfirmSignUpAsync(user, confirmModel.Code, true).ConfigureAwait(false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(item.Code, item.Description);
                    }
                }
            }
            return View();
        }
        public async Task<IActionResult> SignIn()
        {
            var model = new SignInModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInModel signIn)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(signIn.Email, signIn.Password, signIn.RememberMe, false).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("*", "*");
                }
            }

            return View();
        }
    }
}
