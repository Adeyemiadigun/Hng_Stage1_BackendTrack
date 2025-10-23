using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng_Stage1_BackendTrack.Exceptions
{
    /// <summary>
    /// Base exception for all API errors.
    /// </summary>
    public class ApiException : Exception
    {
        public int StatusCode { get; }
        public string? ErrorCode { get; }
        public object? Error { get; }

        public ApiException(string message, int statusCode = 400, string? errorCode = null, object? error = null)
            : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
            Error = error;
        }
    }
}
