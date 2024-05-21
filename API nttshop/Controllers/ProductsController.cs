using API_nttshop.BC;
using API_nttshop.Helpers;
using API_nttshop.Models.Reponse;
using API_nttshop.Models.Request;
using API_nttshop.Models;
using Microsoft.AspNetCore.Mvc;
using API_nttshop.Models.Response.ProductResponse;

namespace API_nttshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly ProductsBC productBC = new ProductsBC();

        [HttpGet]
        [Route("getAllProducts/{language}")]
        public ActionResult<GetAllProductsResponse> GetAllProducts(string language)
        {
            GetAllProductsResponse result = productBC.getAllProducts(language);
            return HandleResponseH.HandleResponse(result);

        }

        [HttpPut]
        [Route("updateProduct")]
        public ActionResult<BaseReponseModel> UpdateProduct(ProductRequest request)
        {
            BaseReponseModel result = productBC.UpdateProduct(request);

            return HandleResponseH.HandleResponse(result);
        }
        [HttpPut]
        [Route("setPrice/{idProduct}/{idRate}/{price}")]
        public ActionResult<BaseReponseModel> SetPrice(int idProduct, int idRate, double price )
        {
            BaseReponseModel result = productBC.SetPrice(idProduct,  idRate, price);

            return HandleResponseH.HandleResponse(result);
        }
        [HttpPost]
        [Route("InsertProduct")]
        public ActionResult<ProductInsertResponse> InsertProduct(ProductRequest request)
        {
            ProductInsertResponse result = productBC.InsertProduct(request);
           
            return HandleResponseH.HandleResponse(result);
            
        }

        [HttpGet("getProduct/{id}/{language}")]
        public ActionResult<BaseReponseModel> GetProduct(int id, string language)
        {
            BaseReponseModel result = productBC.GetProduct(id, language);

            return HandleResponseH.HandleResponse(result);
        }

        [HttpDelete("DeleteProduct/{id}")]
        public ActionResult<BaseReponseModel> DeleteProduct(int id)
        {
            BaseReponseModel result = productBC.DeleteProduct(id);

            return HandleResponseH.HandleResponse(result);
        }
        [HttpDelete("DeleteProductDescription/{id}/{idDescription}")]
        public ActionResult<BaseReponseModel> DeleteProductDrescription(int id, int idDescription)
        {
            BaseReponseModel result = productBC.DeleteProductDescription(id, idDescription);

            return HandleResponseH.HandleResponse(result);
        }
        [HttpDelete("DeleteProductRate/{id}/{idRate}")]
        public ActionResult<BaseReponseModel> DeleteProductRate(int id, int idRate)
        {
            BaseReponseModel result = productBC.DeleteProductRate(id, idRate);

            return HandleResponseH.HandleResponse(result);
        }
    }
}
