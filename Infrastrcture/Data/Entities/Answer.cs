using System.ComponentModel.DataAnnotations;

public class Answer
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public int QuestionNumber { get; set; }
    public string UserAnswer { get; set; }
    public string status  { get; set; }
    public int Correct { get; set; }
    public int InCorrect { get; set; }
    public int Total { get; set; }
}