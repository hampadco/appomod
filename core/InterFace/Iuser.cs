public interface IUser
{
   bool EditUserProfile(VmUser user); 
   VmUser ShowUser(int id);
   List<VmUser> ShowUserRate(int id);
   bool checkdevice(string deviceid);
   void AddDevice(string deviceid,int userid);
   
   VmUser ShowUserByDevice(string deviceid);

   void deactivedevice(string deviceid);
   List<VmUser> ShowAllUser(string txt);
   bool ChangeAgent(int id);
   List<VmUser> ShowAllUseragent(string txt);
   



   
}