using QuizAppService.Models;

namespace QuizAppService.Hubs
{
    public interface IQuizHub
    {
        Task QuizStarted(bool started);
        Task PlayerAdded(Player player);
        Task NextQuestion(string quizId);
    }
}
