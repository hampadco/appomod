using System.ComponentModel.DataAnnotations;

public class SetDb
{
    [Key]
    public int Id { get; set; }
    public int Amount { get; set; }
    public int RateCorrect { get; set; }

    public int RateInCorrect { get; set; }

    public int TimeStart { get; set; }

    public int TimeEnd { get; set; }

    
    
}