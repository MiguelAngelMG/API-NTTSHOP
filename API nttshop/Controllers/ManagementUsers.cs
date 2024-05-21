using API_nttshop.BC;
using API_nttshop.Helpers;
using API_nttshop.Models.Request;
using API_nttshop.Models.Response.UsersResponse;
using API_nttshop.Models;
using Microsoft.AspNetCore.Mvc;
using API_nttshop.Models.Response.ManagementUsersResponse;
using API_nttshop.DAC;

namespace API_nttshop.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ManagementUsers : Controller
    {
        private readonly ManagementUsersBC managementUsersBC = new ManagementUsersBC();

        [HttpGet]
        [Route("getAllManagementUsers")]
        public ActionResult<GetAllManagementUsersResponse> GetAllManagementUsers()
        {
            GetAllManagementUsersResponse result = managementUsersBC.getAllManagementUsers();
            return HandleResponseH.HandleResponse(result);

        }

        [HttpPut]
        [Route("updateManagementUser")]
        public ActionResult<BaseReponseModel> UpdateUser(ManagementUsersRequest request)
        {
            BaseReponseModel result = managementUsersBC.UpdateManagementUser(request);

            return HandleResponseH.HandleResponse(result);
        }
        [HttpPost]
        [Route("insertManagementUser")]
        public ActionResult<BaseReponseModel> InsertUser(ManagementUsersRequest request)
        {
            BaseReponseModel result = managementUsersBC.InsertManagementUser(request);

            if (result.httpStatus == System.Net.HttpStatusCode.OK)
            {
                return Ok();
            }
            else
            {
                return HandleResponseH.HandleResponse(result);
            }
        }

        [HttpGet("getManagementUser/{id}")]
        public ActionResult<BaseReponseModel> GetUser(int id)
        {
            BaseReponseModel result = managementUsersBC.GetManagementUser(id);

            return HandleResponseH.HandleResponse(result);
        }

        [HttpDelete("deleteManagementUser/{id}")]
        public ActionResult<BaseReponseModel> DeleteUser(int id)
        {
            BaseReponseModel result = managementUsersBC.DeleteManagementUser(id);

            return HandleResponseH.HandleResponse(result);
        }
        [HttpPut]
        [Route("UpdateManagementUserPassword/{id}/{password}")]
        public ActionResult<BaseReponseModel> UpdateUserPassword(string password, int id)
        {
            BaseReponseModel result = managementUsersBC.UpdateManagementUserPassword(password, id);

            return HandleResponseH.HandleResponse(result);
        }
    }
}
