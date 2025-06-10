using Microsoft.AspNetCore.DataProtection.KeyManagement;
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
        private readonly IRedisCacheService _cache;

        public QuizService(IHubContext<QuizHub> hubContext, IRedisCacheService cache)
        {
            _hubContext = hubContext;
            _cache = cache;
        }
        public async Task<Quiz> CreateQuiz(Quiz quiz)
        {
            quiz.Id = Guid.NewGuid().ToString();
            _quizzes.Add(quiz);
            await _cache.SetCachedValueAsync($"Quiz:{quiz.Id}", quiz);

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

        public async Task<Quiz?> GetQuizAsync(string id)
        {
            var cachedQuiz = await _cache.GetCachedValueAsync<Quiz>($"Quiz:{id}");
            if (cachedQuiz != null)
                return cachedQuiz;

            var quiz = _quizzes.FirstOrDefault(quiz => quiz.Id == id);

            return quiz;
        }

        public async Task<OperationResult> CreatePlayerAsync(Player player)
        {

            var quiz = _quizzes.FirstOrDefault(quiz => quiz.Id == player.QuizId);
            if (quiz == null) return OperationResult.Fail;

            player.Id = Guid.NewGuid().ToString();
            quiz.Players.Add(player);

            await _hubContext.Clients.Group(player.QuizId).SendAsync("PlayerAdded", player);
            await _cache.SetCachedValueAsync($"Quiz:{quiz.Id}", quiz);

            return OperationResult.Success;

        }

        public async Task<List<Player>> GetPlayers(string quizId)
        {
            //await _cache.ClearCachedValueAsync($"quiz:{quizId}"); måste cleara när ändringar görs
            var quiz = await GetQuizAsync(quizId);

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
            var quiz = await GetQuizAsync(quizId);
            if (quiz != null)
                await _hubContext.Clients.Group(quizId).SendAsync("NextQuestion", quiz.Id);
        }
    }
}
