using System.Net;

namespace API_nttshop.Models.Entities
{
    public class Rate
    {
        internal HttpStatusCode httpStatus;

        public int idRate { get; set; }

        public string descripcion { get; set; }

        public bool defaultRate { get; set; }
    }
}
