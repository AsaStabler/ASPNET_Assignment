using Business.Services;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using PresentationMVCWebApp.Models;

namespace PresentationMVCWebApp.Controllers;

public class AuthController(IAuthService authService) : Controller
{
    private readonly IAuthService _authService = authService;

    //SignUp is the Registration page
    public IActionResult SignUp()
    {
        return View();
    }

    //SignUp is the Registration page
    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpViewModel model)
    {
        ViewBag.ErrorMessage = null;

        if (!ModelState.IsValid)
            return View(model);

        //Mapping from SignUpViewModel to SignUpFormData
        var signUpFormData = model.MapTo<SignUpFormData>();

        //Create User and add role
        var result = await _authService.SignUpAsync(signUpFormData);
        if (result.Succeeded)
        {
            return RedirectToAction("Login", "Auth");
        }

        //Error msg to be displayed on View
        ViewBag.ErrorMessage = result.Error;
        return View(model);
    }

    public IActionResult Login()
    {
        return LocalRedirect("/projects");
        //return View();
    }

    //SignIn is the Login page
    public IActionResult SignIn(string returnUrl = "~/")
    {
        ViewBag.ReturnUrl = returnUrl;

        return View();
    }

    //SignIn is the Login page
    [HttpPost]
    public async Task<IActionResult> SignIn(SignInViewModel model, string returnUrl = "~/")
    {
        ViewBag.ErrorMessage = null;
        ViewBag.ReturnUrl = returnUrl;

        if (!ModelState.IsValid)
            return View(model);

        //Mapping from SignInViewModel to SignInFormData
        var signInFormData = model.MapTo<SignInFormData>();

        //Signing in with Email and Password
        var result = await _authService.SignInAsync(signInFormData);
        if (result.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }

        //Error msg to be displayed on View
        ViewBag.ErrorMessage = result.Error;
        return View(model);
    }
}
