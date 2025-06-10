using Microsoft.AspNetCore.SignalR;

namespace QuizAppService.Hubs
{
    public class QuizHub : Hub<IQuizHub>

    {
        //create interface, make all hub methods to decouple from service.
        public async Task StartQuiz(string quizId)
        {
            await Clients.Group(quizId).QuizStarted(true);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var quizId = httpContext.Request.Query["quizId"];

            await Groups.AddToGroupAsync(Context.ConnectionId, quizId);
            await base.OnConnectedAsync();
        }
    }
}