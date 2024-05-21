using API_nttshop.DAC;
using API_nttshop.Models.Entities;
using API_nttshop.Models.Request;
using API_nttshop.Models.Response.OrderResponse;
using API_nttshop.Models;
using API_nttshop.Models.Response;

namespace API_nttshop.BC
{
    public class OrderBC
    {
        private readonly OrdersDAC ordersDAC = new OrdersDAC();

        public GetAllOrdersResponse getAllOrders(DateTime? fromDate, DateTime? toDate, int? orderStatus)
        {
            GetAllOrdersResponse result = new GetAllOrdersResponse();

            result.ordersList = ordersDAC.GetAllOrders(fromDate, toDate, orderStatus, out string message);

            if (result.ordersList.Count() > 0)
            {
                result.httpStatus = System.Net.HttpStatusCode.OK;
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.NoContent;
                result.message = message;
            }

            return result;
        }
        public GetAllOrdersStatusResponse getAllOrdersStatus()
        {
            GetAllOrdersStatusResponse result = new GetAllOrdersStatusResponse();

            result.ordersList = ordersDAC.GetAllOrdersStatus(out string message);

            if (result.ordersList.Count() > 0)
            {
                result.httpStatus = System.Net.HttpStatusCode.OK;
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.NoContent;
                result.message = message;
            }

            return result;
        }
        public BaseReponseModel InsertOrder(OrdersRequest request)
        {
            BaseReponseModel result = new BaseReponseModel();

            if (InsertOrderValidation(request.order))
            {
                bool correctOperation = ordersDAC.InsertOrder(request.order, out string meesageError);

                if (correctOperation)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    result.httpStatus = System.Net.HttpStatusCode.NotFound;
                    result.message = meesageError;

                }
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.BadRequest;
            }


            return result;


        }
        public BaseReponseModel DeleteOrder(int request)
        {
            BaseReponseModel result = new BaseReponseModel();


            if (GetOrderValidation(request))
            {
                bool eliminado = ordersDAC.DeleteOrder(request);

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
        public BaseReponseModel GetOrder(int requestId)
        {
            OrderResponse result = new OrderResponse();

            if (GetOrderValidation(requestId))
            {
                result.order = ordersDAC.GetOrder(requestId, out string message);

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
        public OrderStatusResponse GetStatus(int requestId)
        {
            OrderStatusResponse result = new OrderStatusResponse();

            if (GetOrderValidation(requestId))
            {
                result.status = ordersDAC.GetSatus(requestId);

                if (result != null)
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

        public BaseReponseModel UpdateOrderStatus(int idOrder, int idStatus)
        {
            BaseReponseModel result = new BaseReponseModel();

            if (idOrder > 0 && idStatus > 0)
            {
                bool correctOperation = ordersDAC.UpdateOrderStatus(idOrder, idStatus, out string messageError);

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

        private bool InsertOrderValidation(Order request)
        {
            if (request == null || request == null)
            {
                return false;
            }

            if ( request.idUser <= 0 || request.TotalPrice < 0 || request.orderDate <= DateTime.MinValue || request.orderStatus <= 0)
            {
                return false;
            }

            if (request.orderDetails != null)
            {
                foreach (OrderDetail d in request.orderDetails)
                {
                    if (d.idProduct < 0 || d.Price < 0 || d.Units < 0)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
         
            return true;
        }
        private bool GetOrderValidation(int request)
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
