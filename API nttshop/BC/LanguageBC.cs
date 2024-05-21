using API_nttshop.DAC;
using API_nttshop.Models;
using API_nttshop.Models.Entities;
using API_nttshop.Models.Reponse.LanguageResponse;
using API_nttshop.Models.Request;
using API_nttshop.Models.Response.LanguagesResponse;

namespace API_nttshop.BC
{
    public class LanguageBC
    {
        private readonly LanguageDAC languageDAC = new LanguageDAC();

        public GetAllLanguagesResponse getAllLanguages()
        {
            GetAllLanguagesResponse result = new GetAllLanguagesResponse();

            result.languageList = languageDAC.GetAllLanguages();

            if (result.languageList.Count() > 0)
            {
                result.httpStatus = System.Net.HttpStatusCode.OK;
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.NoContent;
                result.message = "No content";
            }

            return result;
        }

        public BaseReponseModel UpdateLanguage(LanguageRequest request)
        {
            BaseReponseModel result = new BaseReponseModel();

            if (UpdateLanguageValidation(request))
            {
                bool correctOperation = languageDAC.UpdateLanguage(request.language);

                if (correctOperation)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;

                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
            }


            return result;
        }
        public BaseReponseModel InsertLanguage(LanguageRequest request)
        {
            BaseReponseModel result = new BaseReponseModel();

            if (InsertLanguageValidation(request.language))
            {
                bool correctOperation = languageDAC.InsertLanguage(request.language);

                if (correctOperation)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;

                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
            }


            return result;
        }
        public BaseReponseModel DeleteLanguage(int request)
        {
            BaseReponseModel result = new BaseReponseModel();


            if (GetLanguageValidation(request))
            {
              bool eliminado = languageDAC.DeleteLanguage(request);

                if (eliminado)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = "No content";

                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
            }


            return result;
        }
        public BaseReponseModel GetLanguage(int request)
        {
            LanguageResponse result = new LanguageResponse();
   

            if (GetLanguageValidation(request))
            {
                result.GetLanguage = languageDAC.GetLanguage(request);

                if (result != null)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = "No content";

                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
            }


            return result;
        }
        private bool UpdateLanguageValidation(LanguageRequest request)
        {
            if (request != null
                && request.language != null
                && !string.IsNullOrWhiteSpace(request.language.descripcion)
                && !string.IsNullOrWhiteSpace(request.language.iso)
                 && !request.language.iso.Any(char.IsDigit)
                && request.language.idLanguage > 0
               )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool InsertLanguageValidation(Language request)
        {
            Language l = request;
            if (request != null
                && l != null
                && !string.IsNullOrWhiteSpace(l.descripcion)
                && !string.IsNullOrWhiteSpace(l.iso)
                && !request.iso.Any(char.IsDigit)

               )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool GetLanguageValidation(int request)
        {
            if (request != null && request >= 0 )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}