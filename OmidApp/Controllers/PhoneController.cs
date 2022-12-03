using System.Security.Claims;
using Kavenegar;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
public class PhoneController : Controller
{
    private readonly IQuestion db;
    private readonly IUser dbuser;
     private readonly IWalet dbWalet;
     private readonly Context _context;
    

    public PhoneController(IQuestion db,IUser _dbuser,IWalet _dbWalet,Context context)
    {
        this.db = db;
        this.dbuser = _dbuser;
        this.dbWalet = _dbWalet;
        this._context = context;
    }
  
    
    public IActionResult Login()
    {
       if (User.Identity.IsAuthenticated)
            return RedirectToAction("home", "Home");
        return View();
      
    }
    public IActionResult Check(string phone,string name,string password)
    {
        // Random rnd = new Random();
        // int code = rnd.Next(1000, 9999);
        // HttpContext.Session.SetString("phone", phone);
        // var api = new KavenegarApi("3871353043697339486A70384F544A4A574C74612B51432F4C7A4B305076645457396F5267456F7A5A34383D");
        // try
        // {
        //       var result = api.VerifyLookup(phone, code.ToString(), "demo");
        //       db.Savecode(phone, code);
        // }
        // catch (System.Exception)
        // {
            
             
        // }

        //check if user exist into _context
        var user = _context.Users.FirstOrDefault(u => u.Phone == phone);
        if (user == null)
        {
            //create new user
            User us= new User
            {
                Phone = phone,
                FirstAndLastName = name,
                Email = password,
                Url="",
               Code=0,
               Cart="0",
            };
            _context.Users.Add(us);
            _context.SaveChanges();

            //tempdata success register
            TempData["success"] = "ثبت نام با موفقیت انجام شد";
            return RedirectToAction("login", "phone");

            
        }
        else
        {
            TempData["error"] = "شماره تلفن وارد شده قبلا ثبت شده است";
            return RedirectToAction("Login");
        }
       
        
    }


    //login


    public IActionResult Loginme(string phone ,string password)
    {

        var quser = _context.Users.FirstOrDefault(u => u.Phone == phone && u.Email == password);
           if (quser != null)
        {

            /////////////////////////////////////////

            ClaimsIdentity identity = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name ,quser.FirstAndLastName ) ,
                            new Claim(ClaimTypes.NameIdentifier,quser.Id.ToString() ),
                           //add new claim for add value quser.cart if null set 0

                            new Claim("cart",quser.Cart.ToString() == "0" ? "0" : quser.Cart.ToString() ),
                            new Claim("walet",dbWalet.ShowMojodi(phone).ToString()==null ? "0" : dbWalet.ShowMojodi(phone).ToString() ),
                            new Claim("vige",quser.Url.ToString() == null ? "0" : quser.Url.ToString() ),
                           

                        }, CookieAuthenticationDefaults.AuthenticationScheme);

            var princpal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties

            {
                ExpiresUtc = DateTime.UtcNow.AddYears(1),
                IsPersistent = true
            };
            HttpContext.SignInAsync(princpal, properties);
            
            return RedirectToAction("home", "home");

            


        }
        else
        {
            //tempdata
            TempData["error"] = "شماره موبایل یا رمز عبور اشتباه است";
            return RedirectToAction("login");
        }

        
    }













    public IActionResult Baresi(int code,string deviceid)
    {
        var phone = HttpContext.Session.GetString("phone").ToString();
        
        var quser = db.check(phone, code);
        // if quser.Cart is null then user is not exist
        if (quser.Cart==null)
        {
            quser.Cart="0";
        }
        
               
        if (quser != null)
        {

            // var qdevice=dbuser.checkdevice(deviceid);
            // if (qdevice==false)
            // {
            //     dbuser.AddDevice(deviceid, quser.Id);
            // }else
            // {
                
            //     dbuser.deactivedevice(deviceid);
                
            // }

            // auttocation///////////////////////////////////////////

            ClaimsIdentity identity = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name ,quser.FirstAndLastName ) ,
                            new Claim(ClaimTypes.NameIdentifier,quser.Id.ToString() ),
                           //add new claim for add value quser.cart if null set 0

                            new Claim("cart",quser.Cart.ToString() == null ? "0" : quser.Cart.ToString() ),
                            new Claim("walet",dbWalet.ShowMojodi(HttpContext.Session.GetString("phone")).ToString()==null ? "0" : dbWalet.ShowMojodi(HttpContext.Session.GetString("phone")).ToString() ),
                     
                           

                        }, CookieAuthenticationDefaults.AuthenticationScheme);

            var princpal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties

            {
                ExpiresUtc = DateTime.UtcNow.AddYears(1),
                IsPersistent = true
            };
            HttpContext.SignInAsync(princpal, properties);
            
            return RedirectToAction("home", "home");

            


        }
        else
        {
            return RedirectToAction("login");
        }


    }

    public IActionResult logout()
    {
       
     HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
     //set state false Device


       return RedirectToAction("login", "phone", new { status = "disable" });
    }

    public IActionResult checkdivace(string deviceid)
    {
        var qdevice = dbuser.checkdevice(deviceid);
        if (qdevice == false)
        {
            return Json("faild");
        }
        else
        { 
            //find userid by deviceid
            var q = dbuser.ShowUserByDevice(deviceid);
            
            ClaimsIdentity identity = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name ,q.FirstAndLastName ) ,
                            new Claim(ClaimTypes.NameIdentifier,q.Id.ToString() )
                        }, CookieAuthenticationDefaults.AuthenticationScheme);

            var princpal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties

            {
                ExpiresUtc = DateTime.UtcNow.AddYears(1),
                IsPersistent = true
            };
            HttpContext.SignInAsync(princpal, properties);
            
            return Json("success");
           
        }
    }


    //deactive device
    public void deactivedevice(string deviceid)
    {
        dbuser.deactivedevice(deviceid);
        
    }

    
public IActionResult rule()
{
    // TODO: Your code here
    return View();
}

    
    
}