using System;
using System.ComponentModel.DataAnnotations;

public class Walet
{
    [Key]
    public int Id { get; set; }
    public string senderphone { get; set; }
    public string sendername { get; set; }
    
    public string resiverphone { get; set; }
    public string resivername { get; set; }
     public string babat { get; set; }
    public int variz { get; set; }
    public int bardasht { get; set; }
    public DateTime date { get; set; }
    
    
    
    
    
    
    
    
}