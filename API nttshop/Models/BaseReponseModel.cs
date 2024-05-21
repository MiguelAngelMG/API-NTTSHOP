using API_nttshop.Models.Entities;
using System.Net;

namespace API_nttshop.Models
{
    public class BaseReponseModel
    {
        public string message;
        public HttpStatusCode httpStatus;

        public BaseReponseModel(string message, HttpStatusCode httpStatus)
        {
            this.message = message;
            this.httpStatus = httpStatus;
        }
        
        public BaseReponseModel()
        {
        }
    }
}
