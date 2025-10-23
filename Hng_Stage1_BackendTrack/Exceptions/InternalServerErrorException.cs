namespace Hng_Stage1_BackendTrack.Exceptions
{
    
        public class InternalServerErrorException : ApiException
        {
            public InternalServerErrorException(string message = "An unexpected error occurred", object? error = null)
                : base(message, 500, "INTERNAL_SERVER_ERROR", error) { }
        }
    

}
