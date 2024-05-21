using API_nttshop.Models.Entities;

namespace API_nttshop.Models.Response.ManagementUsersResponse
{
    public class GetAllManagementUsersResponse : BaseReponseModel
    {
        public List<ManagementUser> usersList { get; set; }
    }
}
