using API_nttshop.Models.Entities;

namespace API_nttshop.Models.Response.OrderResponse
{
    public class OrderResponse : BaseReponseModel
    {
        public Order order { get; set; }
    }
}
