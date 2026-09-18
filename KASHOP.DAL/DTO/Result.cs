using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.DTO
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public List<string> Errors { get; set; }
        public static Result<T> Ok(T? data,  string Message = "Success")
        {
            if(data == null)
            {
                return new Result<T> { Success = true, Message = Message};
            }
            return new Result<T> { Success = true, Message = Message, Data = data};}
        public static Result<T> Fail(string Message)
        {return new Result<T> { Success = false, Message = Message};}
        public static Result<T> Fail(string Message, List<string> errors)
        { return new Result<T> { Success = false, Message = Message, Errors = errors }; }
    }
}
