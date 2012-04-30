namespace FubuRESTInnovation.Handlers.Home
{
    public class Get
    {
        public HomeModel Invoke()
        {
            return new HomeModel {Message = "Welcome to the 7digital api"};
        }
    }

    public class HomeModel
    {
        public string Message { get; set; }
    }
}