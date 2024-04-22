#pragma warning disable IDE1006

namespace EstudoBDM.DTOs
{
    public class ApiResponsesDTOs
    {
        public class ExceptionDTO(string _message)
        {
            public string message { get; set; } = _message;
        }
    }
}
