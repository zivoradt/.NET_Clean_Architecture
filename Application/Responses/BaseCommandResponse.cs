using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class BaseCommandResponse
    {
        public BaseCommandResponse(int ID, MessagesEnum messagesEnum, bool success, string message, List<string> errors)
        {
            Id = ID; _messagesEnum = messagesEnum; Success = success; Message = message; Errors = errors ?? new List<string>();
        }

        private MessagesEnum _messagesEnum { get; }
        public int Id { get; set; }
        public bool Success { get; set; }

        public string Message { get; set; }

        public List<string> Errors { get; set; }

        public static BaseCommandResponse Successful(int ID, string? message = null)
        {
            return new BaseCommandResponse(ID, MessagesEnum.Sucessfull, true, message ?? "Creation Successfull", new List<string>());
        }

        public static BaseCommandResponse Failed(List<string> errors, string? message = null)
        {
            return new BaseCommandResponse(default, MessagesEnum.Failed, false, message ?? "Creation Failed", errors);
        }
    }
}

public enum MessagesEnum
{
    Sucessfull = 1,
    Failed = 2
}