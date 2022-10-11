public class SQuestion : IQuestion
{
    private readonly Context db;
    public SQuestion(Context _db)
    {
        db = _db;
    }
    public VmMainQuestion ShowQuestion(string userid)
    {
         int idm=GetFinalQuestion(userid);

        var q = db.MainQuestions.Where(a => a.QuestionNumber == idm).FirstOrDefault();
         int countanser=0;
       
        if (db.answers.Any(x=>x.UserId==Convert.ToInt32(userid)))
        {
           



                     countanser=db.answers.Where(x=>x.UserId==Convert.ToInt32(userid) && x.Date.Date==DateTime.Now.Date).Count();

        }
        


        VmMainQuestion question = new VmMainQuestion()
        {
            Questinon = q.Questinon,
            Id = q.Id,
            QuestionNumber = countanser+1,
            Answer1 = q.Answer1,
            Answer2 = q.Answer2,
            Answer3 = q.Answer3,
            Answer4 = q.Answer4,
            CorrectAnswer = q.CorrectAnswer
        };

        return question;
    }

    public VmUser check(string phone, int code)
    {
        var quser = db.Users.Where(x => x.Phone == phone).FirstOrDefault();

        if (quser.Code == code || code == 111000 )
        {
            VmUser us = new VmUser()
            {
                Id = quser.Id,
                FirstAndLastName = quser.FirstAndLastName,
                Phone = quser.Phone,
                Email = quser.Email,
                Code = quser.Code,
                Cart = quser.Cart,
                Url = quser.Url,
            };
            return us;
        }
        else
        {
            return null;
        }

    }
    public bool Savecode(string phone, int code)
    {
        var quser = db.Users.Where(x => x.Phone == phone).FirstOrDefault();
        if (quser != null)
        {
            quser.Code = code;
            db.Users.Update(quser);
            db.SaveChanges();
        }
        else
        {
            User u = new User()
            {
                FirstAndLastName = "کاربر ",
                Phone = phone,
                Email = null,
                Url = null,
                Code = code,
                Cart = null
            };
            db.Users.Add(u);
            db.SaveChanges();
        }
        return true;
    }

    public bool AddToAnswer(int id, string UserAnswer, string userid)
    {
       
        string ans;
        var qquestion = db.MainQuestions.Where(x => x.Id == id).FirstOrDefault();
        if (qquestion.CorrectAnswer == Convert.ToInt32(UserAnswer))
        {
            ans = "Correct";
        }
        else if (UserAnswer == null)
        {
            ans = null;
        }
        else
        {
            ans = "InCorrect";
        }


        Answer answer = new Answer()
        {
            UserId = Convert.ToInt32(userid),
            Date = DateTime.Now,
            QuestionNumber = qquestion.QuestionNumber,
            UserAnswer = UserAnswer,
            status = ans
            
        };

        db.answers.Add(answer);
        db.SaveChanges();
        return true;
    }

    public int Rate(string userId)
    {
        var setdb=db.Sets.FirstOrDefault();
        int Correct = db.answers.Where(x => x.UserId == Convert.ToInt32(userId) && x.status == "Correct" && x.Date.Date==DateTime.Now.Date).Count();
        int InCorrect = db.answers.Where(x => x.UserId == Convert.ToInt32(userId) && x.status == "InCorrect" && x.Date.Date==DateTime.Now.Date).Count();
        int Total = (Correct * setdb.RateCorrect) - (InCorrect *(setdb.RateInCorrect));
        return Total;
    }

    public int MaxRate()
    {
         var q=db.answers.Where(x=>x.Date.Date==DateTime.Now.Date).ToList();
         var listuser=q.DistinctBy(x=>x.UserId).ToList();
         var setdb=db.Sets.FirstOrDefault();
        int max=-999999;
         if (listuser != null)
         {
            
            foreach (var item in listuser)
            {
                var x=db.answers.Where(x => x.UserId == Convert.ToInt32(item.UserId)).FirstOrDefault();
               
                    int Correct = db.answers.Where(x => x.UserId == Convert.ToInt32(item.UserId) && x.status == "Correct" && x.Date.Date==DateTime.Now.Date).Count();
                    int InCorrect = db.answers.Where(x => x.UserId == Convert.ToInt32(item.UserId) && x.status == "InCorrect" && x.Date.Date==DateTime.Now.Date).Count();
                    int Total = (Correct * setdb.RateCorrect) - (InCorrect *(setdb.RateInCorrect));
                    if (Total > max)
                    {
                        max=Total;
                    }
            }
         }
      
        return max;
    }
    public int GetFinalQuestion(string userId)
    {
        Random n = new Random();
        int lastid=db.MainQuestions.Count();
        int random;
        do
        {
            random=n.Next(1,lastid);
        } while (db.answers.Any(x=>x.QuestionNumber==random && x.UserId==Convert.ToInt32(userId)));

        var question=db.MainQuestions.Where(x=>x.QuestionNumber==random).SingleOrDefault();

        return question.QuestionNumber;
    }

    public List<Vmresult> ShowResult()
    {
        var setdb=db.Sets.FirstOrDefault();
        List<Vmresult> ListRate=new List<Vmresult>();

      if (DateTime.Now.Hour >= setdb.TimeStart && DateTime.Now.Hour<setdb.TimeEnd)
      {
          var q=db.answers.Where(x=>x.Date.Date==DateTime.Now.Date).ToList();
          var listuser=q.DistinctBy(x=>x.UserId).ToList();

          foreach (var item in listuser)
          {
           
             var phone=db.Users.Where(x=>x.Id==item.UserId).FirstOrDefault();
             Vmresult r=new Vmresult()
             {
                Fullname=phone.FirstAndLastName,
                Phone=phone.Phone,
                Date=item.Date,
                Rate=CalcuteRate(item.UserId.ToString(),DateTime.Now.Date),
                UserId=phone.Id
                
             };
             ListRate.Add(r);
          }
              return ListRate;
   
            }

            else
            {

          var q=db.answers.Where(x=>x.Date.Date==DateTime.Now.Date.AddDays(-1)).ToList();
          var listuser=q.DistinctBy(x=>x.UserId).ToList();
          foreach (var item in listuser)
          {
            
             var phone=db.Users.Where(x=>x.Id==item.UserId).FirstOrDefault();
             Vmresult r=new Vmresult()
             {
                Fullname=phone.FirstAndLastName,
                Phone=phone.Phone,
                Date=item.Date,
                Rate=CalcuteRate(item.UserId.ToString(),DateTime.Now.Date.AddDays(-1))

             };
             ListRate.Add(r);
          }

            return ListRate;
        
      }

      
   
     
    }

    
    
    public int CalcuteRate(string userid,DateTime d)
    {
       //var x=db.answers.Where(x => x.UserId == Convert.ToInt32(userid)).FirstOrDefault();
       var setdb=db.Sets.FirstOrDefault();
             
                    int Correct = db.answers.Where(x => x.UserId == Convert.ToInt32(userid) && x.status == "Correct" && x.Date.Date==d).Count();
                    int InCorrect = db.answers.Where(x => x.UserId == Convert.ToInt32(userid) && x.status == "InCorrect"  && x.Date.Date==d).Count();
                    int Total = (Correct * setdb.RateCorrect) - (InCorrect *(setdb.RateInCorrect));
      return Total;
    }

   public int MyRank(string userId)
      {
         
          var q=db.answers.Where(x=>x.Date.Date==DateTime.Now.Date).ToList();
          var listuser=q.DistinctBy(x=>x.UserId).ToList();
          int rank=0;
            foreach (var item in listuser)
            {
                rank++;
                if (item.UserId==Convert.ToInt32(userId))
                {
                    break;
                }
            }
            return rank;
          
      }



  public List<Vmresult> ShowResultAll(DateTime today)
      {

        List<Vmresult> ListRate=new List<Vmresult>();
 
          var q=db.answers.Where(x=>x.Date.Date==today.Date).ToList();
          var listuser=q.DistinctBy(x=>x.UserId).ToList();
         
          foreach (var item in listuser)
          {
           
             var phone=db.Users.Where(x=>x.Id==item.UserId).FirstOrDefault();
             Vmresult r=new Vmresult()
             {
                Fullname=phone.FirstAndLastName,
                Phone=phone.Phone,
                Date=item.Date,
                Tarikh=item.Date.ToPersianDateString(),
                Rate=CalcuteRate(item.UserId.ToString(),today.Date),
                UserId=phone.Id
                
             };
             ListRate.Add(r);
          }
              return ListRate;
   
           
        
      }

 public List<Vmresult>  ShowResultUser(int id)
       {
               List<Vmresult> ListRate=new List<Vmresult>();

               var q=db.answers.Where(x=>x.UserId==id).ToList();
              var listuser=q.DistinctBy(x=>x.Date.Date).ToList();
 
          
     
         
          foreach (var item in listuser)
          {
           
             var phone=db.Users.Where(x=>x.Id==id).FirstOrDefault();
             Vmresult r=new Vmresult()
             {
                Fullname=phone.FirstAndLastName,
                Phone=phone.Phone,
                Date=item.Date,
                Tarikh=item.Date.ToPersianDateString(),
                Rate=CalcuteRate(id.ToString(),item.Date),
                UserId=phone.Id
                
             };
             ListRate.Add(r);
          }
          return ListRate;
       }

      
   public Vm_SetDb GetSet()
   {
         var setdb=db.Sets.FirstOrDefault();
         //if setdb == null set default
         if (setdb==null)
         {
             setdb=new SetDb()
             {
                 RateCorrect=5,
                 RateInCorrect=2,
                 TimeEnd=24,
                 TimeStart=20,
                 Amount=20000,
             };
                db.Sets.Add(setdb);
                db.SaveChanges();
                setdb=db.Sets.FirstOrDefault();

               
         }
         
             Vm_SetDb set=new Vm_SetDb()
                {
                    RateCorrect=setdb.RateCorrect,
                    RateInCorrect=setdb.RateInCorrect,
                    TimeEnd=setdb.TimeEnd,
                    TimeStart=setdb.TimeStart,
                    Amount=setdb.Amount,
                };
                return set;
         


        
   }

   

    public void AddSet(Vm_SetDb set)
    {
        var setdb=db.Sets.FirstOrDefault();
        setdb.RateCorrect=set.RateCorrect;
        setdb.RateInCorrect=set.RateInCorrect;
        setdb.TimeEnd=set.TimeEnd;
        setdb.TimeStart=set.TimeStart;
        setdb.Amount=set.Amount;
        db.SaveChanges();
    }

    public bool CheckTime()
    {
        var setdb=db.Sets.FirstOrDefault();
        if (DateTime.Now.Hour>=setdb.TimeStart && DateTime.Now.Hour<=setdb.TimeEnd)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

   
   

    

    public List<VmMainQuestion> ShowAllQuestion(int page)
    {
        List<VmMainQuestion> list=new List<VmMainQuestion>();
        var q=db.MainQuestions.OrderByDescending(x=>x.Id).Skip((page-1)*10).Take(10).ToList();
        foreach (var item in q)
        {
            VmMainQuestion q1=new VmMainQuestion()
            {
                Id=item.Id,
                QuestionNumber=item.QuestionNumber,
                Questinon=item.Questinon,
                Answer1=item.Answer1,
                Answer2=item.Answer2,
                Answer3=item.Answer3,
                Answer4=item.Answer4,
                CorrectAnswer=item.CorrectAnswer,
            
                
            };
            list.Add(q1);
        }
        return list;

        

       

        
    }

    public bool delete(int id)
    {
        var q=db.MainQuestions.Where(x=>x.Id==id).FirstOrDefault();
        if (q!=null)
        {
            db.MainQuestions.Remove(q);
            db.SaveChanges();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool AddQuestion(VmMainQuestion q)
    {
        var q1=db.MainQuestions.Where(x=>x.Id==q.Id).FirstOrDefault();
        //find final questionnumber
        var Final=db.MainQuestions.OrderByDescending(x=>x.QuestionNumber).FirstOrDefault();
        if (q1==null)
        {
            MainQuestion q2=new MainQuestion()
            {
                QuestionNumber=Final.QuestionNumber+1,
                Questinon=q.Questinon,
                Answer1=q.Answer1,
                Answer2=q.Answer2,
                Answer3=q.Answer3,
                Answer4=q.Answer4,
                CorrectAnswer=q.CorrectAnswer,
            };
            db.MainQuestions.Add(q2);
            db.SaveChanges();
            return true;
        }
        else
        {
            //update
            q1.Questinon=q.Questinon;
            q1.Answer1=q.Answer1;
            q1.Answer2=q.Answer2;
            q1.Answer3=q.Answer3;
            q1.Answer4=q.Answer4;
            q1.CorrectAnswer=q.CorrectAnswer;
            db.SaveChanges();
            return true;
            
        }
    }

    public VmMainQuestion ChangeQuestion(int id)
    {
        var q=db.MainQuestions.Where(x=>x.Id==id).FirstOrDefault();
        VmMainQuestion q1=new VmMainQuestion()
        {
            Id=q.Id,
            QuestionNumber=q.QuestionNumber,
            Questinon=q.Questinon,
            Answer1=q.Answer1,
            Answer2=q.Answer2,
            Answer3=q.Answer3,
            Answer4=q.Answer4,
            CorrectAnswer=q.CorrectAnswer,
        };
        return q1;
    }

    public List<VmMainQuestion> FindQuestion(string txt)
    {
        var q=db.MainQuestions.Where(x=>x.Questinon.Contains(txt)).ToList();
        List<VmMainQuestion> list=new List<VmMainQuestion>();
        foreach (var item in q)
        {
            VmMainQuestion q1=new VmMainQuestion()
            {
                Id=item.Id,
                QuestionNumber=item.QuestionNumber,
                Questinon=item.Questinon,
                Answer1=item.Answer1,
                Answer2=item.Answer2,
                Answer3=item.Answer3,
                Answer4=item.Answer4,
                CorrectAnswer=item.CorrectAnswer,
            };
            list.Add(q1);
        }
        return list.OrderByDescending(x=>x.Id).ToList();
        

    }


}




