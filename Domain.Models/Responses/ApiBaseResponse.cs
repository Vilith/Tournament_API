using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Responses
{
    public class ApiBaseResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public ApiBaseResponse(T data, string? message = null)
        {
            Success = true;
            Data = data;
            Message = message;
        }

        public ApiBaseResponse(List<string> errors, string? message = null)
        {
            Success = false;
            Errors = errors;
            Message = message;
        }
        public ApiBaseResponse(string error, string? message = null)
        {
            Success = false;
            Errors = new List<string> { error };
            Message = message;
        }
    }
}
