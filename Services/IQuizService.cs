using QuizApp.Controllers;
using QuizAppService.Models;

namespace QuizAppService.Services
{
    public interface IQuizService
    {
        Quiz CreateQuiz(Quiz quiz);
        Task StartQuiz(string quizId);
        Task NextQuestion(string quizId);
        Quiz GetQuiz(string id);
        List<Quiz> GetAllQuizzes();
        Task<OperationResult> CreatePlayerAsync(Player player);
        OperationResult SubmitAnswer(PlayerAnswerRequest request);
        List<Player> GetPlayers(string quizId);
    }
}
