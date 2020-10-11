using System;

namespace Ayb.Socket.Assignment.Shared
{
    public class ServiceResponse<T> : ServiceResponse
    {
        public T Data { get; set; }

        public ServiceResponse(bool success, T data) : base(success)
        {
            Data = data;
        }

        public ServiceResponse(bool success, string message, T data) : base(success, message)
        {
            Data = data;
        }

        public ServiceResponse(bool success, string message, Exception exception, T data) : base(success, message, exception)
        {
            Data = data;
        }

        public ServiceResponse(bool success, string message, Exception exception) : base(success, message, exception)
        {
        }
    }

    public class ServiceResponse
    {
        public ServiceResponse(bool success)
        {
            Success = success;
        }

        public ServiceResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public ServiceResponse(bool success, string message, Exception exception)
        {
            Success = success;
            Message = message;
            Exception = exception;
        }

        public bool Success { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }
    }
}
