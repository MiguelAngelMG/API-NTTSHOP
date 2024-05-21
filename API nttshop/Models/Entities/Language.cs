using System.Net;

namespace API_nttshop.Models.Entities
{
    public class Language
    {
        internal HttpStatusCode httpStatus;

        public int idLanguage { get; set; }

        public string descripcion { get; set; }

        public string iso { get; set; }
    }
}