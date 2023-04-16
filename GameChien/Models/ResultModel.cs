using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameChien.Models
{
    public class ResultModel<T>
    {
        public ResultModel(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
    public class ResultModel
    {
        public ResultModel(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}