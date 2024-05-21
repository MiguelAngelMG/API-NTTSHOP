using API_nttshop.BC;
using API_nttshop.Helpers;
using API_nttshop.Models.Reponse.LanguageResponse;
using API_nttshop.Models.Request;
using API_nttshop.Models;
using Microsoft.AspNetCore.Mvc;
using API_nttshop.Models.Response.OrderResponse;
using API_nttshop.Models.Entities;

namespace API_nttshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly OrderBC orderBC = new OrderBC();

        [HttpGet]
        [Route("getAllOrders")]
        public ActionResult<GetAllOrdersResponse> GetAllOrders(DateTime? fromDate, DateTime? toDate, int? orderStatus)
        {
            GetAllOrdersResponse result = orderBC.getAllOrders(fromDate, toDate, orderStatus);
            return HandleResponseH.HandleResponse(result);

        }
        [HttpGet]
        [Route("getAllOrdersStatus")]
        public ActionResult<GetAllOrdersStatusResponse> GetAllOrdersStatus()
        {
            GetAllOrdersStatusResponse result = orderBC.getAllOrdersStatus();
            return HandleResponseH.HandleResponse(result);

        }

        [HttpPut]
        [Route("UpdateOrderStatus/{id}/{status}")]
        public ActionResult<BaseReponseModel> UpdateOrderStatus(int id, int status)
        {
            BaseReponseModel result = orderBC.UpdateOrderStatus(id, status);

            return HandleResponseH.HandleResponse(result);
        }
        [HttpPost]
        [Route("insertOrder")]
        public ActionResult<BaseReponseModel> InsertOrder(OrdersRequest request)
        {
            BaseReponseModel result = orderBC.InsertOrder(request);

            if (result.httpStatus == System.Net.HttpStatusCode.OK)
            {
                return Ok();
            }
            else
            {
                return HandleResponseH.HandleResponse(result);
            }
        }

        [HttpGet("getOrder/{id}")]
        public ActionResult<BaseReponseModel> GetOrder(int id)
        {
            BaseReponseModel result = orderBC.GetOrder(id);

            return HandleResponseH.HandleResponse(result);
        }
        [HttpGet("getOrderStatus/{id}")]
        public ActionResult<BaseReponseModel> GetOrderStatus(int id)
        {
            BaseReponseModel result = orderBC.GetStatus(id);

            return HandleResponseH.HandleResponse(result);
        }
        [HttpDelete("deleteOrder/{id}")]
        public ActionResult<BaseReponseModel> DeleteOrder(int id)
        {
            BaseReponseModel result = orderBC.DeleteOrder(id);

            return HandleResponseH.HandleResponse(result);
        }

        //[Route("updateOrderStatus/{idOrder}/{idStatus}")]
        //public ActionResult<BaseReponseModel> SetPrice(int idProduct, int idRate)
        //{
        //    BaseReponseModel result = orderBC.UpdateOrderStatus(idProduct, idRate);

        //    return HandleResponseH.HandleResponse(result);
        //}
    }
}
