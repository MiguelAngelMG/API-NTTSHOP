using API_nttshop.DAC;
using API_nttshop.Models.Entities;
using API_nttshop.Models.Reponse.LanguageResponse;
using API_nttshop.Models.Request;
using API_nttshop.Models.Response.LanguagesResponse;
using API_nttshop.Models;
using API_nttshop.Models.Response.ProductResponse;
using Microsoft.EntityFrameworkCore;

namespace API_nttshop.BC
{
    public class ProductsBC
    {
        private readonly ProductsDAC productsDAC = new ProductsDAC();

        public GetAllProductsResponse getAllProducts(string language)
        {
            GetAllProductsResponse result = new GetAllProductsResponse();

            if(language == "-1")
            {
                language = "";
            }

            result.productsList = productsDAC.GetAllProducts( language, out string messageError);
            {

                if (result.productsList.Count() > 0)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.NoContent;
                    result.message = messageError;
                }

                return result;
            }
        }

        public BaseReponseModel UpdateProduct(ProductRequest request)
        {
               BaseReponseModel result = new BaseReponseModel();

            if (UpdateProductValidation(request))
            {
                bool correctOperation = productsDAC.UpdateProduct(request.product, out string messageError);

                if (correctOperation)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = messageError;

                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
            }


            return result;

        }
        public BaseReponseModel SetPrice(int idProduct, int idRate, double price)
        {
            BaseReponseModel result = new BaseReponseModel();

            if ( idProduct > 0 && idRate >0 && price >= 0)
            {
                bool correctOperation = productsDAC.SetPrice(idProduct, idRate, price, out string messageError);

                if (correctOperation)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                   result.message = messageError;

                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
            }


            return result;

        }
        public ProductInsertResponse InsertProduct(ProductRequest request)
        {
            ProductInsertResponse result = new ProductInsertResponse();
            string message = "";

            if (InsertProductValidation(request))
            {
                bool operacionCorrecta = productsDAC.InsertProduct(request.product, out message, out int idProduct);

                if (operacionCorrecta)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                    result.idProduct = idProduct;
                }
                else
                {
                    result.message = message;
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;

                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
            }


            return result;

        }
    public BaseReponseModel DeleteProduct(int request)
        {
            BaseReponseModel result = new BaseReponseModel();


            if (GetProductValidation(request))
            {
                bool eliminado = productsDAC.DeleteProduct(request);

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
        public BaseReponseModel DeleteProductDescription(int request, int idDescription)
        {
            BaseReponseModel result = new BaseReponseModel();


            if (GetProductValidation(request))
            {
                bool eliminado = productsDAC.DeleteProductDrescription(request, idDescription);

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
        public BaseReponseModel DeleteProductRate(int request, int idRate)
        {
            BaseReponseModel result = new BaseReponseModel();


            if (GetProductValidation(request))
            {
                bool eliminado = productsDAC.DeleteProductRate(request, idRate);

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
        public BaseReponseModel GetProduct(int requestId, string requestLanguage)
        {
            ProductResponse result = new ProductResponse();

            if (requestLanguage == "-1")
            {
                requestLanguage = "";
            }
            if (GetProductValidation(requestId))
            {
                result.getProduct = productsDAC.GetProduct(requestId, requestLanguage, out string message);

                if (result != null)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = message;

                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
            }


            return result;
        }
        private bool UpdateProductValidation(ProductRequest request)
        {
          
            if (request == null || request.product == null)
            {
                return false;
            }
           
            if (request.product.idProduct <= 0 || request.product.stock < 0 || request.product.enabled == null)
            {
                return false;
            }

            if (request.product.description != null)
            {
                foreach (ProductDescription d in request.product.description)
                {
                    if (d.idProductDescription < 0 || string.IsNullOrWhiteSpace(d.language) || string.IsNullOrWhiteSpace(d.title))
                    {
                        return false;
                    }
                }
            }

            if (request.product.rates != null)
            {
                foreach (ProductRates r in request.product.rates)
                {
                    if (r.idRate < 0 || r.price < 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool InsertProductValidation(ProductRequest request)
        {
            if (request == null || request == null)
            {
                return false;
            }

            if (request.product.idProduct == null || request.product.stock < 0 || request.product.enabled == null)
            {
                return false;
            }

            if (request.product.description != null)
            {
                foreach (ProductDescription d in request.product.description)
                {
                    if (d.idProductDescription < 0 || string.IsNullOrWhiteSpace(d.language) || string.IsNullOrWhiteSpace(d.title))
                    {
                        return false;
                    }
                }
            }

            if (request.product.rates != null)
            {
                foreach (ProductRates r in request.product.rates)
                {
                    if (r.idRate < 0 || r.price < 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        private bool GetProductValidation(int request)
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
