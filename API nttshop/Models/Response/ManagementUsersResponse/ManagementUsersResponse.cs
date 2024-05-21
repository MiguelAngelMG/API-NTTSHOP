using API_nttshop.Models.Entities;

namespace API_nttshop.Models.Response.ManagementUsersResponse
{
    public class ManagementUsersResponse : BaseReponseModel
    {
        public ManagementUser getUser { get; set; }
    }
}
