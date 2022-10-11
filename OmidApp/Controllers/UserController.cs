using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OmidApp.Models;
namespace OmidApp.Controllers;

public class UserController : Controller
{
    private readonly IUser db;
    public UserController(IUser _db)
    {
        db = _db;
    }
    public IActionResult Profile()
    {
        var id=User.Identity.GetId();
        var user=db.ShowUser(Convert.ToInt32(id));
        return View(user);
    }
    public IActionResult EditProfile(VmUser u)
    {
        db.EditUserProfile(u);
        //edit ClaimTypes.Name  in auttication
        var identity = (ClaimsIdentity)User.Identity;
        var claim = identity.FindFirst(ClaimTypes.Name);
        if (claim != null)
        {
            identity.RemoveClaim(claim);
            identity.AddClaim(new Claim(ClaimTypes.Name, u.FirstAndLastName));
        }

        //update claimprincpal in auttication
        var princpal = new ClaimsPrincipal(identity);
        HttpContext.SignInAsync(princpal);
        

        
           
      
        
        return RedirectToAction("home","home");
    }
    

    

}


