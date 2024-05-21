using API_nttshop.Models.Entities;

namespace API_nttshop.Models.Response.OrderResponse
{
    public class GetAllOrdersResponse : BaseReponseModel
    {
        public List<Order> ordersList { get; set; }
    }
}
