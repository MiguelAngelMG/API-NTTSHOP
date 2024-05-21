using API_nttshop.Models.Entities;

namespace API_nttshop.Models.Response.UsersResponse
{
    public class UserResponse : BaseReponseModel
    {
        public User user { get; set; }
    }
}
