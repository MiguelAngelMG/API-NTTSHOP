using API_nttshop.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_nttshop.Helpers
{
  
    public class HandleResponseH : Controller
    {
            
        public static ActionResult HandleResponse(BaseReponseModel response)
        {
            var http = new HandleResponseH();

            if (response.httpStatus == System.Net.HttpStatusCode.OK)
            {
                return http.Ok(response);
            }
            if (response.httpStatus == System.Net.HttpStatusCode.NoContent)
            {
                return http.NoContent();
            }
            if (response.httpStatus == System.Net.HttpStatusCode.BadRequest)
            {
                return http.BadRequest();
            }
            if (response.httpStatus == System.Net.HttpStatusCode.NotFound)
            {
                return http.NotFound(response.message);
            }
            else
            {
                return http.Forbid();
            }
        }

    }
}
