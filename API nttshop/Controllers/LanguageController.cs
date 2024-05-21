using API_nttshop.BC;
using API_nttshop.Models;
using API_nttshop.Models.Entities;
using API_nttshop.Models.Reponse.LanguageResponse;
using API_nttshop.Models.Request;
using Microsoft.AspNetCore.Mvc;
using API_nttshop.Helpers;

namespace API_nttshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : Controller
    {
        private readonly LanguageBC languageBC = new LanguageBC();

        [HttpGet]
        [Route("getAllLanguages")]
        public ActionResult<GetAllLanguagesResponse> GetAllLenguages()
        {
            GetAllLanguagesResponse result = languageBC.getAllLanguages();
             return HandleResponseH.HandleResponse(result);

        }

        [HttpPut]
        [Route("updateLanguage")]
        public ActionResult<BaseReponseModel> UpdateLanguage(LanguageRequest request)
        {
            BaseReponseModel result = languageBC.UpdateLanguage(request);

            return HandleResponseH.HandleResponse(result);
        }
        [HttpPost]
        [Route("insertLanguage")]
        public ActionResult<BaseReponseModel> InsertLanguage(LanguageRequest request)
        {
            BaseReponseModel result = languageBC.InsertLanguage(request);

            if (result.httpStatus == System.Net.HttpStatusCode.OK)
            {
                return Ok();
            }
            else
            {
                return HandleResponseH.HandleResponse(result);
            }
        }

        [HttpGet("getLanguage/{id}")]
        public ActionResult<BaseReponseModel> GetLanguage(int id)
        {
            BaseReponseModel result = languageBC.GetLanguage(id);

            return HandleResponseH.HandleResponse(result);
        }

        [HttpDelete("deleteLanguage/{id}")]
        public ActionResult<BaseReponseModel> DeleteLanguage(int id)
        {
            BaseReponseModel result = languageBC.DeleteLanguage(id);

            return HandleResponseH.HandleResponse(result);
        }

    }
}
