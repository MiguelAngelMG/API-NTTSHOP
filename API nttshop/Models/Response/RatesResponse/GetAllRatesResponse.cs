using API_nttshop.Models.Entities;

namespace API_nttshop.Models.Response.RatesResponse
{
    public class GetAllRatesResponse : BaseReponseModel
    {
        public List<Rate> ratesList { get; set; }
    }
}
