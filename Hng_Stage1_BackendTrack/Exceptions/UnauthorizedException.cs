namespace Hng_Stage1_BackendTrack.Exceptions
{ 


        public class UnauthorizedException : ApiException
        {
            public UnauthorizedException(string message = "Unauthorized", object? error = null)
                : base(message, 401, "UNAUTHORIZED", error) { }
        }
    

}
