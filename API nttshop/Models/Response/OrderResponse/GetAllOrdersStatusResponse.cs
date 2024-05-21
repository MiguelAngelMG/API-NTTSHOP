using API_nttshop.Models.Entities;

namespace API_nttshop.Models.Response.OrderResponse
{
    public class GetAllOrdersStatusResponse : BaseReponseModel
    {
        public List<OrderStatus> ordersList { get; set; }
    }
}
