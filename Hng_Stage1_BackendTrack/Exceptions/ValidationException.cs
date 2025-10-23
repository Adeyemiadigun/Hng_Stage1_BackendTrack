namespace Hng_Stage1_BackendTrack.Exceptions
{
    
        public class ValidationException : ApiException
        {
            public ValidationException(string message, object? error = null)
                : base(message, 400, "VALIDATION_ERROR", error) { }
        }
    

}
