namespace Hng_Stage1_BackendTrack.Exceptions
{
    
        public class ForbiddenException : ApiException
        {
            public ForbiddenException(string message = "Forbidden", object? error = null)
                : base(message, 403, "FORBIDDEN", error) { }
        }
    

}
