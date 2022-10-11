using System.ComponentModel.DataAnnotations;
public class SWalet : IWalet
{
      private readonly Context db;
    
        public SWalet(Context _db)
        {
            db = _db;
        }
    public List<Vm_Walet> ShowAllWalet(string phone )
    {
        var quser = db.walets.Where(a => a.senderphone == phone || a.resiverphone==phone ).ToList();
        List<Vm_Walet> list = new List<Vm_Walet>();
        foreach (var item in quser)
        {
            Vm_Walet walet = new Vm_Walet();
            walet.Id = item.Id;
            walet.senderphone = item.senderphone;
            walet.sendername=item.sendername;
            walet.resivername=item.resivername;
            walet.resiverphone = item.resiverphone;
            walet.variz = item.variz;
            walet.date = item.date;
            walet.bardasht = item.bardasht;
            walet.babat = item.babat;
             walet.tarikh=item.date.ToPersianDateString();
            list.Add(walet);
        }
        return list.OrderByDescending(a=>a.Id).ToList();
    }

    public int ShowMojodi(string phone)
    {
        int variz = db.walets.Where(a => a.resiverphone == phone ).Sum(a => a.variz);
        int bardasht = db.walets.Where(a => a.senderphone==phone).Sum(a => a.variz);
       
        return variz - bardasht;
    }

      public bool CheckMojodi(string phone)
    {
        if (db.walets.Any(a => a.senderphone == phone && a.date.Date==DateTime.Today.Date && a.variz !=0))
        {
            return true;
        }else
        {
             var set=db.Sets.FirstOrDefault();
            int variz = db.walets.Where(a => a.resiverphone == phone ).Sum(a => a.variz);
            int bardasht = db.walets.Where(a =>  a.senderphone==phone).Sum(a => a.variz);
            int sum=variz - bardasht;
            if (sum>=set.Amount)
            {
                //add walet
                Walet walet = new Walet();
                walet.senderphone=phone;
                walet.sendername=db.Users.Where(a => a.Phone == phone).Select(a => a.FirstAndLastName).FirstOrDefault();;
                walet.resiverphone = "سیستم";
                walet.resivername = "سیستم";
                walet.variz = set.Amount;
                walet.date = DateTime.Now;
                walet.bardasht = 0;
                walet.babat = " شرکت در مسابقه";
                db.walets.Add(walet);
                db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
       

       
       
    }

   public bool  ChargeAccunt(int amount,string phone)
   {

         var quser = db.Users.Where(a => a.Phone == phone).SingleOrDefault();
         if (quser != null)
         {
              Walet walet = new Walet();
              walet.senderphone = "مدیریت";
              walet.sendername ="سیستم" ;
              walet.resivername = quser.FirstAndLastName;
              walet.resiverphone = phone;
              walet.variz = amount;
              walet.date = DateTime.Now;
             
              walet.bardasht = 0;
              walet.babat = "واریزی سیستم";
              db.walets.Add(walet);
              db.SaveChanges();
              return true;
         }
         else
         {
              return false;
         }

   }

   public bool delete(int id)
   {
     //TODO: Implement Realistic Implementation
     var qwalet=db.walets.Find(id);
     db.walets.Remove(qwalet);
     db.SaveChanges();
     return true;
   }

    public List<Vm_Walet> ShowResultAll()
    {
        var quser = db.walets.ToList();
        List<Vm_Walet> list = new List<Vm_Walet>();
        foreach (var item in quser)
        {
            Vm_Walet walet = new Vm_Walet();
            walet.Id = item.Id;
            walet.senderphone = item.senderphone;
            walet.sendername=item.sendername;
            walet.resivername=item.resivername;
            walet.resiverphone = item.resiverphone;
            walet.variz = item.variz;
            walet.date = item.date;
            walet.bardasht = item.bardasht;
            walet.babat = item.babat;
             walet.tarikh=item.date.ToPersianDateString();
            list.Add(walet);
        }
        return list;
    }

    public bool AddWalet(VmUser me, int amount, VmUser user)
    {
        if (ShowMojodi(me.Phone)>= amount)
        {
              Walet walet = new Walet();
        walet.senderphone = me.Phone;
        walet.sendername = me.FirstAndLastName;
        walet.resivername = user.FirstAndLastName;
        walet.resiverphone = user.Phone;
        walet.variz = amount;
        walet.date = DateTime.Now;
        walet.bardasht = 0;
        walet.babat = "شارژ حساب";
        db.walets.Add(walet);
        db.SaveChanges();
        return true;
        }else
        {
            return false;
        }
      
    }
}
