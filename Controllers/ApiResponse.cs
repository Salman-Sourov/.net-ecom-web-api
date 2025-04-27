using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_Web_API.Controllers
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public int StatusCode { get; set; }
        public DateTime TimeStamp { get; set; }

        //Constructor for success response
        private ApiResponse(bool success, string message, T data, List<string> errors, int statusCode)
        {
            Success = success;
            Message = message;
            Data = data;
            Errors = errors;
            StatusCode = statusCode;
            TimeStamp = DateTime.UtcNow;
        }

        //Static method for creating a successful response
        public static ApiResponse<T> SuccessResponse(T data, int StatusCode, string Message = "")
        {
            return new ApiResponse<T>(true, Message, data, null, StatusCode);
        }

        //Static method for deleting a successful response
        public static ApiResponse<T> ErrorResponse(List<string> errors, int StatusCode, string Message = "")
        {
            return new ApiResponse<T>(false, Message, default(T), errors, StatusCode);
        }
    }
}
