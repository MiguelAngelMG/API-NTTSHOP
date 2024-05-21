using System.Net;

namespace API_nttshop.Models.Entities
{
    public class Order
    {
        public Order()
        {
            orderDetails = new List<OrderDetail>();
        }
        internal HttpStatusCode httpStatus;

        public int idOrder { get; set; }
        public int idUser { get; set; }
        public DateTime orderDate { get; set; }
        public int orderStatus { get; set; } 
        public decimal TotalPrice { get; set; }
        public OrderStatus status { get; set; }
        public List<OrderDetail> orderDetails { get; set; }
     

        public void calcularPriceTotal()
        { 
            if (orderDetails.Count > 0)
            {
                TotalPrice = 0;
                 foreach(OrderDetail detail in orderDetails) 
                 {
                    TotalPrice += detail.Price;

                 }
            }
        }
        
    }
}
