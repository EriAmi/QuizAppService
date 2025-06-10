using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuizAppService.Models;
using QuizAppService.Services;

namespace QuizApp.Controllers
{
    [ApiController]
    [Route("api/quiz")]
    public class QuizController : ControllerBase
    {
        private IQuizService _quizService;
        private readonly ILogger<QuizController> _logger;
        public QuizController(IQuizService quizService, ILogger<QuizController> logger)
        {
            _quizService = quizService;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateQuiz([FromBody] Quiz quiz)
        {
            if (string.IsNullOrWhiteSpace(quiz.Name) || quiz.Questions.Count == 0)
            {
                _logger.LogWarning("Invalid quiz creation attempt, quiz name or questions was empty");
                return BadRequest("Quiz must have a name and at least one question.");
            }

            var createdQuiz =  await _quizService.CreateQuiz(quiz);
             _logger.LogInformation("Quiz {QuizId} created", createdQuiz.Id);
            return Ok(new { createdQuiz.Id, message = "Quiz created successfully!" });
        }



        [HttpGet("start-quiz")]
        public async Task<IActionResult> StartQuiz(string quizId)
        {
            var quiz = await _quizService.GetQuizAsync(quizId);
            if (quiz == null)
            {
                _logger.LogWarning("Attempt to start non-existing quiz {QuizId}", quizId);
                return BadRequest("Quiz does not exist");
            }

            await _quizService.StartQuiz(quizId);
            return Ok(new { message = "Quiz started" });
        }




        [HttpGet("get-quiz")]
        public async Task<IActionResult> GetQuiz(string id)
        {

            var quiz = await _quizService.GetQuizAsync(id);
            if (quiz == null)
            {
                _logger.LogInformation("No quiz found");
                return NotFound($"quiz with id: {id} does not exist");
            }
            return Ok(quiz);
        }

        [HttpGet("get-quizzes")]
        public IActionResult GetQuizzes()
        {
            var quizzes = _quizService.GetAllQuizzes();
            if (quizzes == null || quizzes.Count() == 0)
            {
                _logger.LogInformation("No quizzes found");
                return NotFound("No quizzes");
            }
            return Ok(quizzes);
        }

        [HttpPost("create-player")]
        public async Task<IActionResult> CreatePlayerAsync([FromBody] Player player)
        {
            if (string.IsNullOrWhiteSpace(player.Name))
            {
                _logger.LogWarning("Invalid player name");
                return BadRequest("Player name cannot be empty.");
            }

            var result = await _quizService.CreatePlayerAsync(player);

            return result == OperationResult.Success 
                ? Ok(player) 
                : NotFound("No quiz found"); 
                    
        }

        [HttpPost("submit-answer")]
        public IActionResult SubmitAnswer([FromBody] PlayerAnswerRequest request)
        {

            var result = _quizService.SubmitAnswer(request);

            return result == OperationResult.Success
                ? Ok(new { message = "Answer submitted successfully!" })
                : NotFound("Player or quiz not found");
        }

        [HttpGet("get-players")]
        public async Task<IActionResult> GetPlayers(string quizId)
        {
            var quiz = await _quizService.GetQuizAsync(quizId);
            if (quiz == null)
            {
                _logger.LogWarning("Quiz {QuizId} not found when fetching players", quizId);
                return NotFound("Quiz does not exist");
            }
            return Ok(_quizService.GetPlayers(quizId));
        }

        [HttpGet("next-question")]
        public async Task<IActionResult> NextQuestion(string quizId)
        {
            //var quiz = await _quizService.GetQuizAsync(quizId);
            // se över felhantering osv
             await _quizService.NextQuestion(quizId);

            return Ok();
        }
    }
}
