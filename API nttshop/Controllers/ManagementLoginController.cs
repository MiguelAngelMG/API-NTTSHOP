using API_nttshop.BC;
using API_nttshop.Helpers;
using API_nttshop.Models;
using API_nttshop.Models.Response.ManagementUsersResponse;
using Microsoft.AspNetCore.Mvc;

namespace API_nttshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagementLoginController
    {

        private readonly ManagementLoginBC userLoginBC = new ManagementLoginBC();

        [HttpGet]
        [Route("getLogin/{user}/{pass}")]
        public ActionResult<ManagementUsersLoginResponse> GetLogin(string user, string pass)
        {
            ManagementUsersLoginResponse result = userLoginBC.getLogin(user, pass);
            return HandleResponseH.HandleResponse(result);

        }
    }
}
