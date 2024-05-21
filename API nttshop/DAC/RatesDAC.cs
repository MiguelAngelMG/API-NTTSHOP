using API_nttshop.Models.Entities;
using System.Data.SqlClient;

namespace API_nttshop.DAC
{
    public class RatesDAC
    {
        public List<Rate> GetAllRates()
        {
            List<Rate> result = new List<Rate>();
            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conn.Open();

                SqlCommand command = new SqlCommand("SELECT PK_RATE, DESCRIPTION,  [DEFAULT] AS isDefault FROM RATES", conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Rate r = new Rate();

                        r.idRate = int.Parse(reader["PK_RATE"].ToString());
                        r.descripcion = reader["DESCRIPTION"].ToString();
                        r.defaultRate = bool.Parse(reader["IsDefault"].ToString());

                        result.Add(r);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los rates", ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public bool UpdateRate(Rate rates)
        {
            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conn.Open();

                SqlCommand command = new SqlCommand("UPDATE RATES SET DESCRIPTION=@description, [DEFAULT]=@defaultRates WHERE PK_RATE=@idRate", conn);
                command.Parameters.AddWithValue("@description", rates.descripcion);
                command.Parameters.AddWithValue("@defaultRates", rates.defaultRate);
                command.Parameters.AddWithValue("@idRate", rates.idRate);

                int result = command.ExecuteNonQuery();

                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al actualizar el rate", Ex);
            }
            finally
            {
                conn.Close();
            }
        }
        public bool InsertRate(Rate rates)
        {
            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conn.Open();

                SqlCommand command = new SqlCommand("INSERT INTO RATES (DESCRIPTION, [DEFAULT]) VALUES (@description, @defaul)", conn);
                command.Parameters.AddWithValue("@description", rates.descripcion);
                command.Parameters.AddWithValue("@defaul", rates.defaultRate);

                int result = command.ExecuteNonQuery();

                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al insertar Rate", Ex);
            }
            finally
            {
                conn.Close();
            }
        }


        public Rate GetRate(int id)
        {
            Rate result = new Rate();
            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conn.Open();

                SqlCommand command = new SqlCommand("SELECT PK_RATE, DESCRIPTION, [DEFAULT] FROM RATES WHERE PK_RATE = @Id", conn);
                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = new Rate();

                        result.idRate = int.Parse(reader["PK_RATE"].ToString());
                        result.descripcion = reader["DESCRIPTION"].ToString();
                        result.defaultRate = bool.Parse(reader["DEFAULT"].ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error al pedir Rate", ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public bool DeleteRate(int rateId)
        {
            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conn.Open();

                SqlCommand command = new SqlCommand("DELETE FROM RATES WHERE PK_RATE = @rateId", conn);
                command.Parameters.AddWithValue("@rateId", rateId);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar rate", ex);
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
