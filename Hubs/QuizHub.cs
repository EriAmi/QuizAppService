using Microsoft.AspNetCore.SignalR;

namespace QuizAppService.Hubs
{
    public class QuizHub : Hub
    {
        //create interface, make all hub methods to decouple from service.
        public async Task StartQuiz(string quizId)
        {
            await Clients.Group(quizId).SendAsync("QuizStarted");
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