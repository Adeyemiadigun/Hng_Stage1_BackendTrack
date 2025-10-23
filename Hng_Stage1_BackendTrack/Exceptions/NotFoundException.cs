namespace Hng_Stage1_BackendTrack.Exceptions
{
  
        public class NotFoundException : ApiException
        {
            public NotFoundException(string message, object? error = null)
                : base(message, 404, "NOT_FOUND", error) { }
        }
    

}
