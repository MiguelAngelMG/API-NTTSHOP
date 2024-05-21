using System.Net;

namespace API_nttshop.Models.Entities
{
    public class Product
    {
        internal HttpStatusCode httpStatus;

        public int idProduct { get; set; }
        public int stock { get; set; }
        public bool enabled { get; set; }
        public List<ProductDescription> description { get; set; }
        public List<ProductRates> rates { get; set; }

        public Product()
        {
            description = new List<ProductDescription>();
            rates = new List<ProductRates>();
        }

    }
}
