using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;



public class SUser : IUser
{
    private readonly Context db;
    private readonly IWebHostEnvironment env;
    public SUser(Context _db, IWebHostEnvironment _env)
    {
       db=_db; 
       env=_env;
    }
        public string GetImageUrl(IFormFile file)
    {
        string FileExtension2 = Path.GetExtension(file.FileName);
        string NewFileName = String.Concat(Guid.NewGuid().ToString(), FileExtension2);
        var path = $"{env.WebRootPath}\\fileupload\\{NewFileName}";
        using (var stream = new FileStream(path, FileMode.Create))
            file.CopyTo(stream);

        return NewFileName;
    }
     public bool EditUserProfile(VmUser user)
    {
      var q= db.Users.Where(x=>x.Phone==user.Phone).FirstOrDefault();
         
          q.FirstAndLastName=user.FirstAndLastName;
          q.Phone=user.Phone;
          if ( user.Urlfirst != null)
          {
            q.Url=GetImageUrl(user.Urlfirst);   
          }
                 
          q.Email=user.Email;
          
        
        db.Users.Update(q);
        db.SaveChanges();

        
        
      return true;
    }
    public VmUser ShowUser(int id)
    {
      var q= db.Users.Where(a=>a.Id==id).FirstOrDefault();
      VmUser u = new VmUser()
      {
         
          Cart=q.Cart,
          Url=q.Url,
          FirstAndLastName=q.FirstAndLastName,
          Phone=q.Phone,
          Email=q.Email,
          Code=q.Code,
         
      };
      
      return u;
    }

    public List<VmUser> ShowUserRate(int id)
    {
        throw new NotImplementedException();
    }

    public bool checkdevice(string deviceid)
    {
        var q=db.Devices.Where(a=>a.DeviceId==deviceid && a.state==true ).FirstOrDefault();
        if (q!=null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddDevice(string deviceid, int userid)
    {
        Device d = new Device()
        {
            DeviceId=deviceid,
            UserId=userid,
            state=true
        };
        db.Devices.Add(d);
        db.SaveChanges();
    }

    public VmUser ShowUserByDevice(string deviceid)
    {
        var q=db.Devices.Where(a=>a.DeviceId==deviceid && a.state==true).FirstOrDefault();
        var quser=db.Users.Where(a=>a.Id==q.UserId).FirstOrDefault();
        VmUser u = new VmUser()
        {
            Id=quser.Id,
            Cart=quser.Cart,
            Url=quser.Url,
            FirstAndLastName=quser.FirstAndLastName,
            Phone=quser.Phone,
            Email=quser.Email,
            Code=quser.Code,
        };
        return u;
    }

    public void deactivedevice(string deviceid)
    {
        var q=db.Devices.Where(a=>a.DeviceId==deviceid && a.state==true).FirstOrDefault();
        q.state= !q.state;
        db.Devices.Update(q);
        db.SaveChanges();
    }

    public List<VmUser> ShowAllUser(string txt)
    {
              
              var q=db.Users.ToList();
              if (txt != null)
              {
                 q=db.Users.Where(x=>x.Phone.Contains(txt) || x.FirstAndLastName.Contains(txt)).ToList();
              }
           

        
        List<VmUser> u = new List<VmUser>();
        foreach (var item in q)
        {
            VmUser v = new VmUser()
            {
                Id=item.Id,
                Cart=item.Cart,
                Url=item.Url,
                FirstAndLastName=item.FirstAndLastName,
                Phone=item.Phone,
                Email=item.Email,
                Code=item.Code,
            };
            u.Add(v);
        }
        return u.OrderByDescending(x=>x.Id).ToList();
    }

     public List<VmUser> ShowAllUseragent(string txt)
    {
              
              var q=db.Users.Where(x=>x.Cart=="agent").ToList();
              if (txt != null)
              {
                 q=db.Users.Where(x=>(x.Phone.Contains(txt) || x.FirstAndLastName.Contains(txt)) && x.Cart=="agent").ToList();
              }
           

        
        List<VmUser> u = new List<VmUser>();
        foreach (var item in q)
        {
            VmUser v = new VmUser()
            {
                Id=item.Id,
                Cart=item.Cart,
                Url=item.Url,
                FirstAndLastName=item.FirstAndLastName,
                Phone=item.Phone,
                Email=item.Email,
                Code=item.Code,
            };
            u.Add(v);
        }
        return u.OrderByDescending(x=>x.Id).ToList();
    }


    public bool ChangeAgent(int id)
    {
        var q=db.Users.Where(a=>a.Id==id).FirstOrDefault();

        if (q.Cart == null)
        {
            q.Cart="agent";
        }
        else
        {
            q.Cart=null;
            
        }
        
        db.Users.Update(q);
        db.SaveChanges();
        return true;
    }
}