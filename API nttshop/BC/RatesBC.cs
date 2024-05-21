using API_nttshop.DAC;
using API_nttshop.Models.Entities;
using API_nttshop.Models.Request;
using API_nttshop.Models;
using API_nttshop.Models.Response.RatesResponse;

namespace API_nttshop.BC
{
    public class RatesBC
    { 
        private readonly RatesDAC rateDAC = new RatesDAC();

        public GetAllRatesResponse getAllRates()
        {
            GetAllRatesResponse result = new GetAllRatesResponse();

            result.ratesList = rateDAC.GetAllRates();

            if (result.ratesList.Count() > 0)
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
        public BaseReponseModel UpdateRate(RatesRequest request)
        {
            BaseReponseModel result = new BaseReponseModel();

            if (UpdateRateValidation(request))
            {
                bool correctOperation = rateDAC.UpdateRate(request.rate);

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
        public BaseReponseModel InsertRate(RatesRequest request)
        {
            BaseReponseModel result = new BaseReponseModel();

            if (InsertRateValidation(request.rate))
            {
                bool correctOperation = rateDAC.InsertRate(request.rate);

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
        public BaseReponseModel DeleteRate(int request)
        {
            BaseReponseModel result = new BaseReponseModel();


            if (GetRateValidation(request))
            {
                bool eliminado = rateDAC.DeleteRate(request);

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
        public BaseReponseModel GetRate(int request)
        {
            RatesResponse result = new RatesResponse();


            if (GetRateValidation(request))
            {
                result.getRates = rateDAC.GetRate(request);

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
        private bool UpdateRateValidation(RatesRequest request)
        {
            if (request != null
                && request.rate != null
                && !string.IsNullOrWhiteSpace(request.rate.descripcion)
                && request.rate.defaultRate != null
                && request.rate.idRate > 0
               )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool InsertRateValidation(Rate request)
        {
            Rate rateInsert = request;
            if (request != null
                && rateInsert != null
                && !string.IsNullOrWhiteSpace(rateInsert.descripcion)
                && rateInsert.defaultRate != null

               )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool GetRateValidation(int request)
        {
            if (request != null && request >= 0)
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
