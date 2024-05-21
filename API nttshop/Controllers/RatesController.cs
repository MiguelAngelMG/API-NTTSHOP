using API_nttshop.BC;
using API_nttshop.Models.Response.RatesResponse;
using API_nttshop.Models.Request;
using API_nttshop.Models;
using Microsoft.AspNetCore.Mvc;
using API_nttshop.Helpers;

namespace API_nttshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatesController : Controller
    {
        private readonly RatesBC ratesBC = new RatesBC();

        [HttpGet]
        [Route("getAllRates")]
        public ActionResult<GetAllRatesResponse> GetAllRates()
        {
            GetAllRatesResponse result = ratesBC.getAllRates();
            return HandleResponseH.HandleResponse(result);

        }

        [HttpPut]
        [Route("updateRate")]
        public ActionResult<BaseReponseModel> UpdateRate(RatesRequest request)
        {
            BaseReponseModel result = ratesBC.UpdateRate(request);

            return HandleResponseH.HandleResponse(result);
        }
        [HttpPost]
        [Route("insertRate")]
        public ActionResult<BaseReponseModel> InsertRate(RatesRequest request)
        {
            BaseReponseModel result = ratesBC.InsertRate(request);

            if (result.httpStatus == System.Net.HttpStatusCode.OK)
            {
                return Ok();
            }
            else
            {
                return HandleResponseH.HandleResponse(result);
            }
        }

        [HttpGet("getRate/{id}")]
        public ActionResult<BaseReponseModel> GetRate(int id)
        {
            BaseReponseModel result = ratesBC.GetRate(id);

            return HandleResponseH.HandleResponse(result);
        }

        [HttpDelete("deleteRate/{id}")]
        public ActionResult<BaseReponseModel> DeleteRate(int id)
        {
            BaseReponseModel result = ratesBC.DeleteRate(id);

            return HandleResponseH.HandleResponse(result);
        }
    }
}
