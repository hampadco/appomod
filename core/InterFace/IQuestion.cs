public interface IQuestion
{
    VmMainQuestion ShowQuestion(string userid);
    VmUser check(string phone, int code);
    bool Savecode(string phone, int code);
    bool AddToAnswer(int id, string UserAnswer, string userid);
    int Rate(string userId);
    int GetFinalQuestion(string userId);
    int MaxRate();
    List<Vmresult>  ShowResult();

    
    List<Vmresult>  ShowResultAll(DateTime today);
    //GetFinalQuestion
    int MyRank(string userId);
    
     List<Vmresult>  ShowResultUser(int id);
   
    Vm_SetDb GetSet();
    void AddSet(Vm_SetDb set);
    bool CheckTime();
    List<VmMainQuestion>  ShowAllQuestion(int page);
    bool delete(int id);
    bool AddQuestion(VmMainQuestion question);
    VmMainQuestion ChangeQuestion(int id);
    List<VmMainQuestion> FindQuestion(string txt);


}