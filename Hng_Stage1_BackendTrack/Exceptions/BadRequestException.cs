namespace Hng_Stage1_BackendTrack.Exceptions 
{
    
        public class BadRequestException : ApiException
        {
            public BadRequestException(string message, object? error = null)
                : base(message, 400, "BAD_REQUEST", error) { }
        }
    

}
