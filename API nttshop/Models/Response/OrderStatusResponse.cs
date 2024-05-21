using API_nttshop.Models.Entities;

namespace API_nttshop.Models.Response
{
    public class OrderStatusResponse : BaseReponseModel
    {
        public OrderStatus status { get; set; }
    }
}
