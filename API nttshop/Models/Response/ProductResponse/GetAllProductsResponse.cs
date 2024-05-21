using API_nttshop.Models.Entities;

namespace API_nttshop.Models.Response.ProductResponse
{
    public class GetAllProductsResponse : BaseReponseModel
    {
        public List<Product> productsList { get; set; }
      
    }
}
