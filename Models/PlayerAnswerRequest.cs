namespace QuizAppService.Models
{
    public class PlayerAnswerRequest
    {
      
            public string QuizId { get; set; }
            public string PlayerId { get; set; }
            public int QuestionId { get; set; }
            public string Answer { get; set; }
    }
}
