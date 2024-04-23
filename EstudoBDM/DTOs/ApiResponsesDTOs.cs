#pragma warning disable IDE1006

namespace EstudoBDM.DTOs
{
    public class ApiResponsesDTOs
    {
        public class ExceptionDTO(string _message)
        {
            public string message { get; set; } = _message;
        }

        public class NullFieldExceptionDTO(string field)
        {
            public string message { get; set; } = Properties.Resources.NullFieldException.Replace("{}", field);
        }
    }
}
