using QuizApp.Controllers;
using QuizAppService.Models;

namespace QuizAppService.Services
{
    public interface IQuizService
    {
        Task<Quiz> CreateQuiz(Quiz quiz);
        Task StartQuiz(string quizId);
        Task NextQuestion(string quizId);
        Task <Quiz> GetQuizAsync(string id);
        List<Quiz> GetAllQuizzes();
        Task<OperationResult> CreatePlayerAsync(Player player);
        OperationResult SubmitAnswer(PlayerAnswerRequest request);
       Task <List<Player>> GetPlayers(string quizId);

    }
}
