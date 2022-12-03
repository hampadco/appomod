using System.Net.Http.Headers;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmidApp.Models;
using Extensions;

[Area("Admin")]
public class HomeController : Controller
{
  private readonly IQuestion db;
   private readonly IUser dbUser;
   
   private readonly IWalet dbWalet;
   private readonly Context _context;

    public HomeController(IQuestion _db,IUser _dbUser,IWalet _dbWalet ,Context _context)
    {
        db = _db;
        dbUser = _dbUser;
        dbWalet = _dbWalet;
        this._context = _context;
    }
    
 

  public IActionResult Index(string txt)
    {
      //search
     
         ViewBag.User=dbUser.ShowAllUser(txt);
      
       
      

        return View();
    }
     public IActionResult login()
    {
        return View();
    }
     public IActionResult log(int Password, string email)
    {
        if (Password == 1234 && email == "admin")
        {
               var claims = new List<Claim>() 
               {
               new Claim (ClaimTypes.NameIdentifier,"admin"),
               new Claim (ClaimTypes.Name, "admin")
               };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                var properties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddYears(1),
                    IsPersistent = true
                };
                HttpContext.SignInAsync(principal, properties);
                return RedirectToAction("index");
        }
        else
        {
            
             TempData["error"] = "Error";
             return RedirectToAction("login");
        }
    }
    public IActionResult Participantsscore(string txt)
    {
        
      try
      {
         DateTime Today=txt.ToGeorgianDateTime();
        ViewBag.User=db.ShowResultAll(Today).OrderByDescending(x=>x.Rate).ToList();
         return View();
      }
      catch (System.Exception ex)
      {
         
         return View();
      }
     
    
     
    }
    public IActionResult info(string txt)
    {
       ViewBag.User=dbUser.ShowAllUser(txt);
      return View();
    }
   
    public IActionResult paystatus(string txt)
    {
      //TODO: Implement Realistic Implementation
       try
      {
         DateTime Today=txt.ToGeorgianDateTime();
         ViewBag.walet=dbWalet.ShowResultAll().Where(x=>x.date.Date==Today).OrderByDescending(x=>x.Id).ToList();
         ViewBag.Mojodi=dbWalet.ShowResultAll().Where(x=>x.date.Date==Today).Sum(x=>x.variz);
         
      }
      catch (System.Exception ex)
      {
         
           ViewBag.walet=dbWalet.ShowResultAll().OrderByDescending(x=>x.Id).ToList();
           ViewBag.Mojodi=dbWalet.ShowResultAll().Sum(x=>x.variz);
      }
      
       
             
         return View();
    }
    public IActionResult setting()
    {
      //TODO: Implement Realistic Implementation

      return View(db.GetSet());
    }
    public IActionResult bag()
    {
      //TODO: Implement Realistic Implementation
      return View();
    }

    public IActionResult logout()
    {
        HttpContext.SignOutAsync();
        return RedirectToAction("login");
    }
    
    //agent 
    public IActionResult agentinfo(int id)
    {
      var q=dbUser.ChangeAgent(id);
      return RedirectToAction("index");
    }

    public IActionResult walet(string phone )
    {
        ViewBag.walet=dbWalet.ShowAllWalet(phone);
        //nameresives take 1 into viewbag.walet
        ViewBag.name=dbUser.ShowAllUser(phone).FirstOrDefault().FirstAndLastName + "  با شماره تلفن : " +phone ;
        ViewBag.tel=phone;
        ViewBag.mojodi=dbWalet.ShowMojodi(phone);
        //save phone to session
        if (phone != null)
        HttpContext.Session.SetString("phone",phone);
        return View();
    }
    
    public IActionResult charge(int txt)
    {
        dbWalet.ChargeAccunt(txt,HttpContext.Session.GetString("phone"));
        return RedirectToAction("walet",new {phone=HttpContext.Session.GetString("phone")});
    }
    

    public IActionResult del(int id)
    {
        // TODO: Your code here
          dbWalet.delete(id);
       return RedirectToAction("walet",new {phone=HttpContext.Session.GetString("phone")});
    }
    
    public IActionResult deatils(int id)
    {
       
          ViewBag.User=db.ShowResultUser(id).OrderByDescending(x=>x.Date).ToList();
      
       
           return View();
    }

    public IActionResult agent(string txt)
    {
        // TODO: Your code here
         ViewBag.User=dbUser.ShowAllUseragent(txt);

        return View();
    }

    public IActionResult waletall()
    {
        // TODO: Your code here
        
        return View();
    }
    

    public IActionResult addset(Vm_SetDb set)
    
    {
        db.AddSet(set);
        return RedirectToAction("setting");
    }

    public IActionResult exit()
    {
        // TODO: Your code here
        HttpContext.SignOutAsync();
        return RedirectToAction("login");
       
    }
    
    public IActionResult question(int page,int id,string txt)
   
    {
      ViewBag.Number=page;
       //page number is null
        if (page <= 0)
        {
            page = 1;
        }
        // TODO: Your code here
        if (txt != null)
        {
          ViewBag.question=db.FindQuestion(txt);
           
        }else
        {
           ViewBag.question=db.ShowAllQuestion(page);
        }
       

        if (id != 0)
        {
          var q=db.ChangeQuestion(id);
            return View(q);
        }

        

        
        return View();
    }
    

    public IActionResult delete(int id)
    
    {
        // TODO: Your code here
        db.delete(id);
        return RedirectToAction("question");
    }

    public IActionResult addquestion(VmMainQuestion question)
    
    {

        db.AddQuestion(question);
        return RedirectToAction("question");
    }
    
    

    //vige
    public IActionResult vige(int id)
    {
      var user=_context.Users.Find(id);
      if (user.Url=="vige")
      {
        user.Url="";
      }else
      {
        //first delete all vige
        var q=_context.Users.Where(x=>x.Url=="vige").FirstOrDefault();
        if (q != null)
        {
          q.Url="";
          _context.Users.Update(q);
          _context.SaveChanges();
        }
        

        user.Url="vige";
      }
      

     _context.Users.Update(user);
      _context.SaveChanges();

      
      return RedirectToAction("index");
      
    }
    
    
    

}


