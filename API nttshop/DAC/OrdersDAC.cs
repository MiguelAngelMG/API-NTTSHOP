using API_nttshop.BC;
using API_nttshop.Models.Entities;
using System.Data;
using System.Data.SqlClient;

namespace API_nttshop.DAC
{
    public class OrdersDAC
    {
        UsersDAC usersDAC = new UsersDAC();
        ProductsDAC productsDAC = new ProductsDAC();    

        public List<Order> GetAllOrders(DateTime? fromDate, DateTime? toDate, int? orderStatus, out string messageError)
        {
            List<Order> result = new List<Order>();
            messageError = "";
            using (SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString()))
            {
                try
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand("sp_GetAllOrders", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@fromDate", fromDate);
                    command.Parameters.AddWithValue("@toDate", toDate);
                    command.Parameters.AddWithValue("@idStatus", orderStatus);

                    SqlParameter outputResultMessage = new SqlParameter("@resultMessage", SqlDbType.NVarChar, 1000);
                    outputResultMessage.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outputResultMessage);



                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int orderId = int.Parse(reader["idOrder"].ToString());
                            bool orderExists = result.Any(o => o.idOrder == orderId);

                            if (!orderExists)
                            {
                                User user = new User();
                                OrderStatus status = new OrderStatus();
                                Order order = new Order
                                {
                                    idOrder = orderId,
                                    idUser = int.Parse(reader["idUser"].ToString()),
                                    orderDate = DateTime.Parse(reader["orderDate"].ToString()),
                                    orderStatus = int.Parse(reader["orderStatus"].ToString()),
                                    TotalPrice = decimal.Parse(reader["totalPrice"].ToString())
                                };

                                user = usersDAC.GetUser(order.idUser);
                                status = GetSatus(order.orderStatus);
                                order.status = status;

                                OrderDetail detail = new OrderDetail
                                {
                                    idOrder = orderId,
                                    idProduct = int.Parse(reader["idProduct"].ToString()),
                                    Units = int.Parse(reader["units"].ToString()),
                                    Price = decimal.Parse(reader["price"].ToString())
                                };
                                string language = user.Languages;
                                Product product = productsDAC.GetProduct(detail.idProduct, language, out messageError);
                                detail.product = product;

                                order.orderDetails.Add(detail);
                                result.Add(order);
                            }
                            else
                            {
                                Order existeOrder = result.First(o => o.idOrder == orderId);
                                User user = usersDAC.GetUser(existeOrder.idUser);

                                if (!existeOrder.orderDetails.Any(d => d.idProduct == int.Parse(reader["idProduct"].ToString())))
                                {
                                    existeOrder.orderDetails.Add(new OrderDetail
                                    {
                                        idOrder = orderId,
                                        idProduct = int.Parse(reader["idProduct"].ToString()),
                                        Units = int.Parse(reader["units"].ToString()),
                                        Price = decimal.Parse(reader["price"].ToString())
                                    });
                                    string language = user.Languages;
                                    Product product = productsDAC.GetProduct(existeOrder.orderDetails.Last().idProduct, language, out messageError);
                                   
                                    existeOrder.orderDetails.Last().product = product;
                                }


                            }
                        }
                    }
                    messageError = (string)outputResultMessage.Value + messageError;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener todos los orders", ex);
                }
            }

            return result;
        }
        public List<OrderStatus> GetAllOrdersStatus( out string messageError)
        {
            List<OrderStatus> result = new List<OrderStatus>();
            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conn.Open();

                SqlCommand command = new SqlCommand("SELECT PK_STATUS, DESCRIPTION FROM ORDERSTATUS", conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        OrderStatus status = new OrderStatus();

                        status.idStatus = int.Parse(reader["PK_STATUS"].ToString());
                        status.description = reader["DESCRIPTION"].ToString();

                        result.Add(status);
                    }
                }
                messageError = "";
            }
            catch (Exception ex)
            {
                messageError = "Error al recoger los status";
                throw new Exception("Error al recoger el status", ex);
                
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
        public bool InsertOrder(Order order, out string messageError)
        {
            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());
            SqlCommand commandInsert;
            int idOrder = -1;
            bool orderIntroducido = true;
            bool detailIntroducido = true;
            try
            {
                conn.Open();

                commandInsert = new SqlCommand("sp_InsertOrder", conn);
                commandInsert.CommandType = CommandType.StoredProcedure;



                commandInsert.Parameters.AddWithValue("@Date", order.orderDate);
                commandInsert.Parameters.AddWithValue("@OrderStatus", order.orderStatus);
                commandInsert.Parameters.AddWithValue("@idUser", order.idUser);
                commandInsert.Parameters.AddWithValue("@totalPrice", order.TotalPrice);
                commandInsert.Parameters.AddWithValue("@idProduct", order.orderDetails[0].idProduct);
                commandInsert.Parameters.AddWithValue("@price", order.orderDetails[0].Price);
                commandInsert.Parameters.AddWithValue("@units", order.orderDetails[0].Units);


                // Parámetros de salida
                SqlParameter outputIdProduct = new SqlParameter("@idOrder", SqlDbType.Int);
                outputIdProduct.Direction = ParameterDirection.Output;
                commandInsert.Parameters.Add(outputIdProduct);

                SqlParameter outputResultMessage = new SqlParameter("@resultMessage", SqlDbType.NVarChar, 1000);
                outputResultMessage.Direction = ParameterDirection.Output;
                commandInsert.Parameters.Add(outputResultMessage);

                int result = commandInsert.ExecuteNonQuery();
                messageError = (string)outputResultMessage.Value;
                if (result > 0 && orderIntroducido)
                {
                    idOrder = (int)outputIdProduct.Value;



                    if (order.orderDetails.Count > 1 && idOrder != -1)
                    {
                        int i = 1;
                        while (i < order.orderDetails.Count)
                        {
                            commandInsert = new SqlCommand("sp_CreateOrderDetail", conn);
                            commandInsert.CommandType = CommandType.StoredProcedure;
                            commandInsert.Parameters.Clear();
                            commandInsert.Parameters.AddWithValue("@idOrder", idOrder);
                            commandInsert.Parameters.AddWithValue("@idProduct", order.orderDetails[i].idProduct);
                            commandInsert.Parameters.AddWithValue("@units", order.orderDetails[i].Units);
                            commandInsert.Parameters.AddWithValue("@price", order.orderDetails[i].Price);

                            SqlParameter outputResultMessageDescription = new SqlParameter("@resultMessage", SqlDbType.NVarChar, 1000);
                            outputResultMessageDescription.Direction = ParameterDirection.Output;
                            commandInsert.Parameters.Add(outputResultMessageDescription);


                            int rowsAfectadas = 0;
                            rowsAfectadas = commandInsert.ExecuteNonQuery();
                            messageError = "Se ha insertado el order a la base de datos pero no se ha introducido el producto: " + order.orderDetails[i].idProduct + " nn el pedido. " + (string)outputResultMessageDescription.Value + " " + messageError;

                            if (rowsAfectadas == -1)
                            {
                                detailIntroducido = false;
                            }

                            commandInsert.Parameters.Clear();
                            i++;
                        }



                    };

                    if (orderIntroducido && detailIntroducido)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al actualizar el producto", Ex);
            }
            finally
            {
                conn.Close();
            }

        }


        public Order GetOrder(int id, out string message)
        {
            Order result = new Order();
            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conn.Open();

                SqlCommand command = new SqlCommand("sp_GetOrder", conn);
                command.CommandType = CommandType.StoredProcedure;

                // Agregar parámetro de entrada
                command.Parameters.AddWithValue("@idOrder", id);

                SqlParameter outputMessage = new SqlParameter("@messageError", SqlDbType.NVarChar, 1000);
                outputMessage.Direction = ParameterDirection.Output;
                command.Parameters.Add(outputMessage);

                SqlParameter outputResult = new SqlParameter("@result", SqlDbType.Int);
                outputResult.Direction = ParameterDirection.Output;
                command.Parameters.Add(outputResult);

                // Ejecutar el comando
                command.ExecuteNonQuery();

                int resultado = (int)outputResult.Value;
                message = (string)outputMessage.Value;

                if (resultado == 1)
                {

                    // Ejecutar el comando y leer los resultados
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            OrderDetail detail = new OrderDetail();
                            User user = new User();
                            Product product = new Product();
                            
                            int i = 1;
                            // Asignamos los valores principales y en caso que i fuese distinto a 0, no repitiriamos la asignación de valores. Como buscamos por id y language, descripción no se repitira.
                            if (i == 1)
                            {
                                result.idOrder = int.Parse(reader["idOrder"].ToString());
                                result.idUser = int.Parse(reader["idUser"].ToString());
                                result.orderDate = DateTime.Parse(reader["orderDate"].ToString());
                                result.orderStatus = int.Parse(reader["orderStatus"].ToString());
                                result.TotalPrice = decimal.Parse(reader["priceTotal"].ToString());

                                user = usersDAC.GetUser(result.idUser);
                                i++;

                            }

                            if (result.orderDetails.Count > 0 && user != null)
                            {

                                detail.idOrder = int.Parse(reader["idOrder"].ToString());
                                detail.idProduct = int.Parse(reader["idProduct"].ToString());
                                detail.Units = int.Parse(reader["Units"].ToString());
                                detail.Price = decimal.Parse(reader["Price"].ToString());
                                string language = user.Languages;
                                product = productsDAC.GetProduct(detail.idProduct, language, out message);
                                detail.product = product;

                                bool existeDetails = false;

                                foreach (var ordActual in result.orderDetails)
                                {
                                   
                                    if (ordActual.idProduct == int.Parse(reader["idProduct"].ToString()))
                                    {
                                        existeDetails = true;
                                    }

                                }
                                if (!existeDetails)
                                {
                                    result.orderDetails.Add(detail);
                                }
                            }
                            else if(user != null)
                            {
                                detail.idOrder = int.Parse(reader["idOrder"].ToString());
                                detail.idProduct = int.Parse(reader["idProduct"].ToString());
                                detail.Units = int.Parse(reader["Units"].ToString());
                                detail.Price = decimal.Parse(reader["Price"].ToString());

                                string language = user.Languages;
                                product = productsDAC.GetProduct(detail.idProduct, language,out message);
                                detail.product = product;
                                result.orderDetails.Add(detail);
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception(message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al solicitar el Order", ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public bool DeleteOrder(int idOrder)
        {

            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conn.Open();

                SqlCommand command = new SqlCommand("sp_DeleteOrder", conn);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@idOrder", idOrder);

                int rowsAfectadas = command.ExecuteNonQuery();

                return rowsAfectadas > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al borrar Producto", ex);
            }
            finally
            {
                conn.Close();
            }
        }

        public bool UpdateOrderStatus(int idOrder, int idStatus, out string messageError)
        {
            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());
            messageError = "";
            try
            {
                conn.Open();

                SqlCommand command = new SqlCommand("sp_UpdateOrderStatus", conn);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@idOrder", idOrder);
                command.Parameters.AddWithValue("@idStatus", idStatus);

                SqlParameter outputResultMessage = new SqlParameter("@resultMessage", SqlDbType.NVarChar, 1000);
                outputResultMessage.Direction = ParameterDirection.Output;
                command.Parameters.Add(outputResultMessage);

                int rowsAfectado = command.ExecuteNonQuery();

                messageError = (string)outputResultMessage.Value + " " + messageError;


                return rowsAfectado > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar el status del Order", ex);
            }
            finally
            {
                conn.Close();
            }
        }

        public OrderStatus GetSatus(int id)
        {
            OrderStatus result = new OrderStatus();
            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conn.Open();

                SqlCommand command = new SqlCommand("SELECT PK_STATUS, DESCRIPTION FROM OrderStatus WHERE PK_STATUS = @Id", conn);
                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = new OrderStatus();

                        result.idStatus = int.Parse(reader["PK_STATUS"].ToString());
                        result.description = reader["DESCRIPTION"].ToString();
                      
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error al pedir el estado del pedido", ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
    }
}
