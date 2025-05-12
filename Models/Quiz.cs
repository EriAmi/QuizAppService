namespace QuizAppService.Models
{
    public class Quiz
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public List<Question> Questions { get; set; } = [];
        public List<Player> Players { get; set; } = [];
    }
}