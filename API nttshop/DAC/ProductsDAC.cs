using API_nttshop.Models.Entities;
using System.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using API_nttshop.BC;

namespace API_nttshop.DAC
{
    public class ProductsDAC
    {
        public List<Product> GetAllProducts(string language, out string messageError)
        {
           
            List<Product> result = new List<Product>();
                messageError = "";
            using (SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString()))
            {
                try
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand("sp_GetAllProducts", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@language",language);
                    SqlParameter outputResultMessage = new SqlParameter("@resultMessage", SqlDbType.NVarChar, 1000);
                    outputResultMessage.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outputResultMessage);

                   

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int productId = int.Parse(reader["idProduct"].ToString());
                            bool productExists = result.Any(p => p.idProduct == productId);

                            if (!productExists)
                            {
                                Product product = new Product
                                {
                                    idProduct = productId,
                                    stock = int.Parse(reader["Stock"].ToString()),
                                    enabled = bool.Parse(reader["Enabled"].ToString())
                                };

                                ProductDescription description = new ProductDescription
                                {
                                    idProductDescription = int.Parse(reader["idDescription"].ToString()),
                                    title = reader["Title"].ToString(),
                                    language = reader["Language"].ToString(),
                                    description = reader["Description"].ToString()
                                };

                                ProductRates rate = new ProductRates
                                {
                                    idProduct = productId,
                                    idRate = int.Parse(reader["idRate"].ToString()),
                                    price = double.Parse(reader["Price"].ToString())
                                };

                                product.description.Add(description);
                                product.rates.Add(rate);
                                result.Add(product);
                            }
                            else
                            {
                                Product existeProduct = result.First(p => p.idProduct == productId);

                                if (!existeProduct.description.Any(d => d.idProductDescription == int.Parse(reader["idDescription"].ToString())))
                                {
                                    existeProduct.description.Add(new ProductDescription
                                    {
                                        idProductDescription = int.Parse(reader["idDescription"].ToString()),
                                        title = reader["Title"].ToString(),
                                        language = reader["Language"].ToString(),
                                        description = reader["Description"].ToString()
                                    });
                                }

                                if (!existeProduct.rates.Any(r => r.idRate == int.Parse(reader["idRate"].ToString())))
                                {
                                    existeProduct.rates.Add(new ProductRates
                                    {
                                        idProduct = productId,
                                        idRate = int.Parse(reader["idRate"].ToString()),
                                        price = double.Parse(reader["Price"].ToString())
                                    });
                                }
                            }
                        }
                    }
                    messageError = (string)outputResultMessage.Value + messageError;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener todos los productos", ex);
                }
            }

            return result;
        }

        public bool UpdateProduct(Product product, out string messageError)
        {
            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());
            SqlCommand command;
            bool productoActualizado = true;
            bool descripciónActualizado = true;
            bool rateActualizado = true;

            messageError = "";
            try
            {
                conn.Open();
                command = new SqlCommand("sp_UpdateProduct", conn);
                command.CommandType = CommandType.StoredProcedure;


                command.Parameters.AddWithValue("@idProduct", product.idProduct);
                command.Parameters.AddWithValue("@newStock", product.stock);
                command.Parameters.AddWithValue("@newEnabled", product.enabled);
                command.Parameters.AddWithValue("@idLanguage", product.description[0].idProductDescription);
                command.Parameters.AddWithValue("@language", product.description[0].language);
                command.Parameters.AddWithValue("@newTitle", product.description[0].title);
                command.Parameters.AddWithValue("@newDescription", product.description[0].description);
                command.Parameters.AddWithValue("@idRate", product.rates[0].idRate);

                SqlParameter outputResultMessage = new SqlParameter("@resultMessage", SqlDbType.NVarChar, 1000);
                outputResultMessage.Direction = ParameterDirection.Output;
                command.Parameters.Add(outputResultMessage);



                
                int result = command.ExecuteNonQuery();
                messageError = (string)outputResultMessage.Value + " " + messageError;
                command.Parameters.Clear();
                if (result > 0 && productoActualizado)
                {
                    if (product.description.Count > 1 && product.description.Count != null)
                    {
                        int i = 1;
                        while (i < product.description.Count && descripciónActualizado)
                        {
                            command.Parameters.Clear();
                            command = new SqlCommand("sp_UpdateorCreateProductDescription", conn);
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@idProduct", product.idProduct);
                            command.Parameters.AddWithValue("@idLanguage", product.description[i].idProductDescription);
                            command.Parameters.AddWithValue("@language", product.description[i].language);
                            command.Parameters.AddWithValue("@newTitle", product.description[i].title);
                            command.Parameters.AddWithValue("@newDescription", product.description[i].description);

                            SqlParameter outputResultMessageDescription = new SqlParameter("@resultMessage", SqlDbType.NVarChar, 1000);
                            outputResultMessageDescription.Direction = ParameterDirection.Output;
                            command.Parameters.Add(outputResultMessageDescription);

                          
                            int rowsAfectadas = 0;
                            rowsAfectadas = command.ExecuteNonQuery();
                              messageError = "Se ha actualizado el producto a la base de datos pero no se ha introducido el lenguage: " + product.description[i].language  + ". " + (string)outputResultMessageDescription.Value + " " + messageError;

                            if (outputResultMessageDescription.Value != DBNull.Value && !string.IsNullOrEmpty(outputResultMessageDescription.Value.ToString()))
                            {
                                rateActualizado = false;
                                messageError = "Se ha producido un error al actualizar la descripcion del producto: " + outputResultMessageDescription.Value.ToString() + ". " + messageError;
                            }


                            i++;
                        }



                    };

                    if (product.rates.Count > 1 && product.rates.Count != null )
                    {
                        int i = 1;
                        while (i < product.rates.Count && rateActualizado)
                        {
                            command = new SqlCommand("sp_UpdateorCreateProductRate", conn);
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.AddWithValue("@idProduct", product.idProduct);
                            command.Parameters.AddWithValue("@idRate", product.rates[i].idRate);

                            SqlParameter outputResultMessageRate = new SqlParameter("@resultMessage", SqlDbType.NVarChar, 1000);
                            outputResultMessageRate.Direction = ParameterDirection.Output;
                            command.Parameters.Add(outputResultMessageRate);

                            int rowsAfectadas = 0;
                            rowsAfectadas = command.ExecuteNonQuery();
                            messageError = "Se ha actualizado el producto en la base de datos pero no se ha introducido el rate con id: " + product.rates[i].idRate + ". " + (string)outputResultMessageRate.Value + " " + messageError;
                            if (outputResultMessageRate.Value != DBNull.Value && !string.IsNullOrEmpty(outputResultMessageRate.Value.ToString()))
                            {
                                rateActualizado = false;
                                messageError = "Se ha producido un error al actualizar el rate del producto: " + outputResultMessageRate.Value.ToString() + ". " + messageError;
                            }
                            command.Parameters.Clear();
                            i++;
                        }

                    }
                    if(productoActualizado && descripciónActualizado &&  rateActualizado)
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
        public bool InsertProduct(Product product, out string messageError, out int idproduct)
        {
            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());
            SqlCommand commandInsert;
            int idProduct = -1;
            bool productoActualizado = true;
            bool descripciónActualizado = true;
            bool rateActualizado = true;
            try
            {
                conn.Open();

                commandInsert = new SqlCommand("sp_InsertProduct", conn);
                commandInsert.CommandType = CommandType.StoredProcedure;



                commandInsert.Parameters.AddWithValue("@newStock", product.stock);
                commandInsert.Parameters.AddWithValue("@newEnabled", product.enabled);   
                commandInsert.Parameters.AddWithValue("@language", product.description[0].language);
                commandInsert.Parameters.AddWithValue("@newTitle", product.description[0].title);
                commandInsert.Parameters.AddWithValue("@newDescription", product.description[0].description);
                commandInsert.Parameters.AddWithValue("@idRate", product.rates[0].idRate);

                // Parámetros de salida
                SqlParameter outputIdProduct = new SqlParameter("@idProduct", SqlDbType.Int);
                outputIdProduct.Direction = ParameterDirection.Output;
                commandInsert.Parameters.Add(outputIdProduct);

                SqlParameter outputResultMessage = new SqlParameter("@resultMessage", SqlDbType.NVarChar, 1000);
                outputResultMessage.Direction = ParameterDirection.Output;
                commandInsert.Parameters.Add(outputResultMessage);
                
                int result = commandInsert.ExecuteNonQuery();
                messageError = (string)outputResultMessage.Value;
                idproduct = Convert.ToInt32(commandInsert.Parameters["@idProduct"].Value);

                if (result > 0 && productoActualizado)
                {
                    idProduct = (int)outputIdProduct.Value;
                    


                    if (product.description.Count > 1 && idProduct != -1)
                    {
                        int i = 1;
                        while (i < product.description.Count)
                        {
                            commandInsert = new SqlCommand("sp_UpdateorCreateProductDescription", conn);
                            commandInsert.CommandType = CommandType.StoredProcedure;
                            commandInsert.Parameters.Clear();
                            commandInsert.Parameters.AddWithValue("@idProduct", idProduct);
                            commandInsert.Parameters.AddWithValue("@idLanguage", product.description[i].idProductDescription);
                            commandInsert.Parameters.AddWithValue("@language", product.description[i].language);
                            commandInsert.Parameters.AddWithValue("@newTitle", product.description[i].title);
                            commandInsert.Parameters.AddWithValue("@newDescription", product.description[i].description);

                            SqlParameter outputResultMessageDescription = new SqlParameter("@resultMessage", SqlDbType.NVarChar, 1000);
                            outputResultMessageDescription.Direction = ParameterDirection.Output;
                            commandInsert.Parameters.Add(outputResultMessageDescription);


                            int rowsAfectadas = 0;
                            rowsAfectadas = commandInsert.ExecuteNonQuery();
                            messageError = "Se ha insertado el producto a la base de datos pero no se ha introducido el lenguage: " + product.description[i].language + ". " + (string)outputResultMessageDescription.Value + " " + messageError;

                            if (rowsAfectadas == -1)
                            {
                                descripciónActualizado = false;
                            }

                            commandInsert.Parameters.Clear();
                            i++;
                        }



                    };

                    if (product.rates.Count > 1 && idProduct != -1)
                    {
                        int i = 1;
                        while (i < product.rates.Count)
                        {
                            commandInsert = new SqlCommand("sp_UpdateorCreateProductRate", conn);
                            commandInsert.CommandType = CommandType.StoredProcedure;

                            commandInsert.Parameters.AddWithValue("@idProduct", idProduct);
                            commandInsert.Parameters.AddWithValue("@idRate", product.rates[i].idRate);

                            SqlParameter outputResultMessageRate = new SqlParameter("@resultMessage", SqlDbType.NVarChar, 1000);
                            outputResultMessageRate.Direction = ParameterDirection.Output;
                            commandInsert.Parameters.Add(outputResultMessageRate);


                            int rowsAfectadas = 0;
                            rowsAfectadas = commandInsert.ExecuteNonQuery();
                            messageError = "Se ha insertado el producto en la base de datos pero no se ha introducido el rate con id: " + product.rates[i].idRate + ". " + (string)outputResultMessageRate.Value + " " + messageError;
                            if (rowsAfectadas == -1)
                            {
                                rateActualizado = false;
                            }
                            commandInsert.Parameters.Clear();
                            i++;
                        }

                    }

                    if (productoActualizado && descripciónActualizado && rateActualizado)
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
                throw new Exception("Error al actualizar el producto" , Ex);
            }
            finally
            {
                conn.Close();
            }

        }
        public Product GetProduct(int id, string language, out string message)
        {
            Product result = new Product();
            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conn.Open();

                SqlCommand command = new SqlCommand("sp_GetProduct", conn);
                command.CommandType = CommandType.StoredProcedure;

                // Agregar parámetro de entrada
                command.Parameters.AddWithValue("@idProduct", id);
                command.Parameters.AddWithValue("@language ", language);

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
                            ProductDescription description = new ProductDescription();
                            ProductRates rate = new ProductRates();
                            int i = 1;
                            // Asignamos los valores principales y en caso que i fuese distinto a 0, no repitiriamos la asignación de valores. Como buscamos por id y language, descripción no se repitira.
                            if (i == 1)
                            {
                                result.idProduct = int.Parse(reader["idProduct"].ToString());
                                result.stock = int.Parse(reader["Stock"].ToString());
                                result.enabled = bool.Parse(reader["Enabled"].ToString());
                            }

                            if (result.description.Count > 0)
                            {
                                bool existeDes = false; // Inicialmente suponemos que la descripción no existe en la lista

                                foreach (var desActual in result.description)
                                {
                                    if (desActual.idProductDescription == int.Parse(reader["idDescription"].ToString()))
                                    {
                                        existeDes = true; // Marcar que la descripción existe
                                        break; // Salir del bucle si se encuentra una descripción existente
                                    }
                                }

                                if (!existeDes)
                                {
                                    // Crear una nueva instancia de 'description' solo si no existe en la lista
                                    ProductDescription newDescription = new ProductDescription
                                    {
                                        idProductDescription = int.Parse(reader["idDescription"].ToString()),
                                        idProduct = int.Parse(reader["idProduct"].ToString()),
                                        title = reader["Title"].ToString(),
                                        language = reader["Language"].ToString(),
                                        description = reader["Description"].ToString()
                                    };

                                    result.description.Add(newDescription); // Agregar la nueva descripción a la lista
                                }
                            }
                            else
                            {
                                description.idProductDescription = int.Parse(reader["idDescription"].ToString());
                                description.idProduct = int.Parse(reader["idProduct"].ToString());
                                description.title = reader["Title"].ToString();

                                description.language = reader["Language"].ToString();
                                description.description = reader["Description"].ToString();

                                result.description.Add(description);
                            }

                            if (result.rates.Count > 0)
                            {
                                bool existeRate = true;
                                foreach (var rateActaul in result.rates)
                                {
                                    existeRate = true;
                                    if (rateActaul.idRate != int.Parse(reader["idRate"].ToString()))
                                    {
                                        existeRate = false;
                                    }

                                }
                                if (!existeRate)
                                {

                                    rate.idProduct = int.Parse(reader["idProduct"].ToString());
                                    rate.idRate = int.Parse(reader["idRate"].ToString());
                                    rate.price = double.Parse(reader["Price"].ToString());

                                    result.rates.Add(rate);
                                }
                            }
                            else
                            {
                                rate.idProduct = int.Parse(reader["idProduct"].ToString());
                                rate.idRate = int.Parse(reader["idRate"].ToString());
                                rate.price = double.Parse(reader["Price"].ToString());

                                result.rates.Add(rate);
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
                throw new Exception("Error al solicitar el producto", ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public bool DeleteProduct(int id)
        {
            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conn.Open();

                SqlCommand command = new SqlCommand("sp_DeleteProduct", conn);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@idProduct", id);

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
        }public bool DeleteProductDrescription(int id, int idDescription)
        {
            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conn.Open();

                SqlCommand command = new SqlCommand("DELETE FROM ProductDescription WHERE PK_ProductDescription = @idDescrip", conn);
                command.Parameters.AddWithValue("@idDescrip", idDescription);

                int rowsAffected = command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al delete productDescription", ex);
            }
            finally
            {
                conn.Close();
            }
            
        }
        public bool DeleteProductRate(int id, int idRate)
        {
            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conn.Open();

                SqlCommand command = new SqlCommand("DELETE FROM ProductRates WHERE FK_RATE = @idRa AND FK_PRODUCT = @idPr", conn);
                command.Parameters.AddWithValue("@idRa", idRate);
                command.Parameters.AddWithValue("@idPr", id);

                int rowsAffected = command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al delete productDescription", ex);
            }
            finally
            {
                conn.Close();
            }

        }
        public bool SetPrice(int idProduct, int idRate, double price, out string messageError)
        {
            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());
            messageError = "";
            try
            {
                conn.Open();

                SqlCommand command = new SqlCommand("sp_SetPriceProduct", conn);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@idProduct", idProduct);
                command.Parameters.AddWithValue("@idRate", idRate);
                command.Parameters.AddWithValue("@newPrice", price);

                SqlParameter outputResultMessage = new SqlParameter("@resultMessage", SqlDbType.NVarChar, 1000);
                outputResultMessage.Direction = ParameterDirection.Output;
                command.Parameters.Add(outputResultMessage);

                int rowsAfectado = command.ExecuteNonQuery();

                messageError = (string)outputResultMessage.Value + " " + messageError;


                return rowsAfectado > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar el precio de Producto", ex);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
