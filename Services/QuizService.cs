using Microsoft.AspNetCore.SignalR;
using QuizApp.Controllers;
using QuizAppService.Hubs;
using QuizAppService.Models;

namespace QuizAppService.Services
{
    public class QuizService : IQuizService
    {
        private static readonly List<Quiz> _quizzes = [];
        private readonly IHubContext<QuizHub> _hubContext;

        public QuizService(IHubContext<QuizHub> hubContext)
        {
         _hubContext = hubContext;   
        }
        public Quiz CreateQuiz(Quiz quiz)
        {
            quiz.Id = Guid.NewGuid().ToString();
            _quizzes.Add(quiz);
            return quiz;
        }

        public async Task StartQuiz(string quizId)
        {
            await _hubContext.Clients.Group(quizId).SendAsync("QuizStarted", true);
        }

        public List<Quiz> GetAllQuizzes()
        {
            return _quizzes;
        }

        public Quiz GetQuiz(string id)
        {

            return _quizzes.FirstOrDefault(quiz => quiz.Id == id);
        }

        public async Task<OperationResult> CreatePlayerAsync(Player player)
        {
            
            var quiz = _quizzes.FirstOrDefault(quiz => quiz.Id == player.QuizId);
            if (quiz == null) return OperationResult.Fail;

            player.Id = Guid.NewGuid().ToString();
            quiz.Players.Add(player);
            await _hubContext.Clients.Group(player.QuizId).SendAsync("PlayerAdded", player);

            return OperationResult.Success;

        }

        public List<Player> GetPlayers(string quizId)
        {
            var quiz = GetQuiz(quizId);

            return quiz.Players;
        }


        public OperationResult SubmitAnswer(PlayerAnswerRequest request)
        {
            var quiz = _quizzes.FirstOrDefault(q => q.Players.Any(p => p.Id == request.PlayerId));
            var player = quiz?.Players.FirstOrDefault(p => p.Id == request.PlayerId);
            if (player == null) return OperationResult.Fail;

            player.Answers.Add(new PlayerAnswer
            {
                QuestionId = request.QuestionId,
                Answer = request.Answer
            });
            return OperationResult.Success;
        }

        public async Task NextQuestion(string quizId)
        {
            var quiz = GetQuiz(quizId);
           await _hubContext.Clients.Group(quizId).SendAsync("NextQuestion", quiz.Id);
        }
    }
}
