using API_nttshop.Models.Entities;
namespace API_nttshop.Models.Response.UsersResponse
{
    public class GetAllUsersResponse : BaseReponseModel
    {
       public List<User> usersList { get; set; }
        
    }
}
