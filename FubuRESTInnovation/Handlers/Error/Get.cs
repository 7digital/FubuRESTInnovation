namespace FubuRESTInnovation.Handlers.Error
{
    public class Get
    {
        public ErrorResponse Invoke(ErrorRequest input)
        {
            return new ErrorResponse { Message = input.Message };
        }
    }

    public class ErrorRequest
    {
        public string Message { get; set; }
    }

    public class ErrorResponse
    {
        public string Message { get; set; }
    }
}