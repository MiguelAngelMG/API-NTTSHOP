using API_nttshop.BC;
using API_nttshop.Helpers;
using API_nttshop.Models.Request;
using API_nttshop.Models.Response.UsersResponse;
using API_nttshop.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_nttshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly UsersBC usersBC = new UsersBC();

        [HttpGet]
        [Route("getAllUsers")]
        public ActionResult<GetAllUsersResponse> GetAllUsers()
        {
            GetAllUsersResponse result = usersBC.getAllUsers();
            return HandleResponseH.HandleResponse(result);

        }

        [HttpPut]
        [Route("updateUser")]
        public ActionResult<BaseReponseModel> UpdateUser(UsersRequest request)
        {
            BaseReponseModel result = usersBC.UpdateUser(request);

            return HandleResponseH.HandleResponse(result);
        }
        [HttpPost]
        [Route("insertUser")]
        public ActionResult<UserResponse> InsertUser(UsersRequest request)
        {
            UserResponse result = usersBC.InsertUser(request);

            if (result.httpStatus == System.Net.HttpStatusCode.OK)
            {
                return Ok();
            }
            else
            {
                return HandleResponseH.HandleResponse(result);
            }
        }

        [HttpGet("getUser/{id}")]
        public ActionResult<BaseReponseModel> GetUser(int id)
        {
            BaseReponseModel result = usersBC.GetUser(id);

            return HandleResponseH.HandleResponse(result);
        }

        [HttpDelete("deleteUser/{id}")]
        public ActionResult<BaseReponseModel> DeleteUser(int id)
        {
            BaseReponseModel result = usersBC.DeleteUser(id);

            return HandleResponseH.HandleResponse(result);
        }
        [HttpPut]
        [Route("UpdateUserPassword/{id}/{password}")]
        public ActionResult<BaseReponseModel> UpdateUserPassword(string password, int id)
        {
            BaseReponseModel result = usersBC.UpdateUserPassword(password, id);

            return HandleResponseH.HandleResponse(result);
        }


    }
}
