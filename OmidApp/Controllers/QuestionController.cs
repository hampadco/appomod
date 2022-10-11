using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OmidApp.Models;
namespace OmidApp.Controllers;
public class QuestionController : Controller
{
    private readonly IQuestion db;
    public QuestionController(IQuestion _db)
    {
      db = _db;
    }
    public IActionResult Question(int id,string UserAnswer)
    {
      string userid=User.Identity.GetId();

      if (UserAnswer !="first")
      {
         db.AddToAnswer(id,UserAnswer,userid);
      }
      
      ViewBag.q = db.ShowQuestion(userid);
      ViewBag.rate=db.Rate(User.Identity.GetId()); 
      ViewBag.Max=db.MaxRate();   
       //my Rate
       var allRate=db.ShowResult().OrderByDescending(x=>x.Rate).ToList();
       
       //find my index in allRate
        int index=0;
        for (int i = 0; i < allRate.Count; i++)
        {
            if (allRate[i].Rate== ViewBag.rate )
            {
                index=i+1;
                break;
            }
        }


      ViewBag.MyRate= index; 
      return View();
    }
    
}    