using API_nttshop.BC;
using API_nttshop.Helpers;
using API_nttshop.Models;
using API_nttshop.Models.Entities;
using API_nttshop.Models.Response.UsersResponse;
using Microsoft.AspNetCore.Mvc;

namespace API_nttshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : Controller
    {

        private readonly UserLoginBC userLoginBC = new UserLoginBC();

        [HttpGet]
        [Route("getLogin/{user}/{pass}")]
        public ActionResult<UserLoginResponse> GetLogin(string user, string pass)
        {
            UserLoginResponse result = userLoginBC.getLogin(user, pass);
            return HandleResponseH.HandleResponse(result);

        }
    }
}
