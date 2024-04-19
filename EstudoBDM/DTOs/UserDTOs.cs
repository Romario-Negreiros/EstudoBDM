#pragma warning disable IDE1006

namespace EstudoBDM.DTOs
{
    public class UserDTOs
    {   
        public class LoginUserDTO
        {
            public string? name { get; set; }
            public string[]? scopes { get; set; }
        }
        public class LoggedUserDTO
        {
            public bool authenticated { get; set; }
            public DateTime expiration { get; set; }
            public string token { get; set; } = default!;
        }
    }
}
