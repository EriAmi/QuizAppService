namespace QuizAppService.Models
{
    public class Player
    {
        public string? Id { get; set; }
        public string QuizId { get; set; }
        public string Name { get; set; }
        public List<PlayerAnswer> Answers { get; set; } = [];
    }

}
