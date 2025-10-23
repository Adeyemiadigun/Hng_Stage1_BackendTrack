namespace Hng_Stage1_BackendTrack.Exceptions
{
        public class ConflictException : ApiException
        {
            public ConflictException(string message, object? error = null)
                : base(message, 409, "CONFLICT", error) { }
        }
    

}
