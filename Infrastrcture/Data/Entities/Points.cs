using System.ComponentModel.DataAnnotations;

public class Points
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Point { get; set; }
    public int TotalPoints { get; set; }
}