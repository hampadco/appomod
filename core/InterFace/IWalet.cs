public interface IWalet
{
    List<Vm_Walet> ShowAllWalet(string phone);
    int ShowMojodi(string phone);
    bool ChargeAccunt(int amount,string phone);
    bool delete(int id);
    List<Vm_Walet> ShowResultAll();
    bool CheckMojodi(string phone);

    bool AddWalet(VmUser me,int amount,VmUser user);
    
}