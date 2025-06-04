using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using QuizAppService.Models;
using QuizAppService.Services;

namespace QuizApp.Controllers
{
    [ApiController]
    [Route("api/quiz")]
    public class QuizController : ControllerBase
    {
        private IQuizService _quizService;
        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpPost("create")]
        public IActionResult CreateQuiz([FromBody] Quiz quiz)
        {
            if (string.IsNullOrWhiteSpace(quiz.Name) || quiz.Questions.Count == 0)
            {
                return BadRequest("Quiz must have a name and at least one question.");
            }

            var createdQuiz = _quizService.CreateQuiz(quiz);
            return Ok(new { quiz.Id, message = "Quiz created successfully!" });
        }



        [HttpGet("start-quiz")]
        public async Task<IActionResult> StartQuiz(string quizId)
        {
            var quiz = _quizService.GetQuizAsync(quizId);
            if (quiz == null)
            {
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
                return NotFound("No quizzes");
            }
            return Ok(quizzes);
        }

        [HttpPost("create-player")]
        public async Task<IActionResult> CreatePlayerAsync([FromBody] Player player)
        {
            if (string.IsNullOrWhiteSpace(player.Name))
            {
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
        public IActionResult GetPlayers(string quizId)
        {
            var quiz = _quizService.GetQuizAsync(quizId);
            if (quiz == null) return NotFound("Quiz does not exist");

            return Ok(_quizService.GetPlayers(quizId));
        }

        [HttpGet("next-question")]
        public IActionResult NextQuestion(string quizId)
        {
            var quiz = _quizService.GetQuizAsync(quizId);
            if (quiz == null) return NotFound("Quiz does not exist");

            return Ok(_quizService.NextQuestion(quizId));
        }
    }
}
