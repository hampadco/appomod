using System.ComponentModel.DataAnnotations;
public class MainQuestion
{
    [Key]
    public int Id { get; set; }
    public int QuestionNumber { get; set; }
    public int CorrectAnswer { get; set; }
    public string Questinon { get; set; }
    public string Answer1 { get; set; }
    public string Answer2 { get; set; }
    public string Answer3 { get; set; }
    public string Answer4 { get; set; }

}