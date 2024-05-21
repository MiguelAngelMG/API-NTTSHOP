using API_nttshop.Models.Entities;
using System.Data.SqlClient;

namespace API_nttshop.DAC
{
    public class LanguageDAC
    {
        public List<Language> GetAllLanguages()
        {
            List<Language> result = new List<Language>();
            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conn.Open();

                SqlCommand command = new SqlCommand("SELECT PK_LANGUAGE, DESCRIPTION, ISO FROM LANGUAGES", conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Language l = new Language();

                        l.idLanguage = int.Parse(reader["PK_LANGUAGE"].ToString());
                        l.descripcion = reader["DESCRIPTION"].ToString();
                        l.iso = reader["ISO"].ToString();

                        result.Add(l);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar idioma", ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public bool UpdateLanguage(Language language)
        {
            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conn.Open();

                SqlCommand command = new SqlCommand("UPDATE LANGUAGES SET DESCRIPTION=@description, ISO=@iso WHERE PK_LANGUAGE=@idLanguage", conn);
                command.Parameters.AddWithValue("@description", language.descripcion);
                command.Parameters.AddWithValue("@iso", language.iso);
                command.Parameters.AddWithValue("@idLanguage", language.idLanguage);

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
                throw new Exception("Error al actualizar idioma", Ex);
            }
            finally
            {
                conn.Close();
            }
        }
        public bool InsertLanguage(Language language)
        {
            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conn.Open();

                SqlCommand command = new SqlCommand("INSERT INTO LANGUAGES (DESCRIPTION, ISO) VALUES (@description, @iso)", conn);
                command.Parameters.AddWithValue("@description", language.descripcion);
                command.Parameters.AddWithValue("@iso", language.iso);

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
                throw new Exception("Error al insertar idioma", Ex);
            }
            finally
            {
                conn.Close();
            }
        }


        public Language GetLanguage(int id)
        {
            Language result = new Language();
            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conn.Open();

                SqlCommand command = new SqlCommand("SELECT PK_LANGUAGE, DESCRIPTION, ISO FROM LANGUAGES WHERE PK_LANGUAGE = @Id", conn);
                command.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = new Language();

                        result.idLanguage = int.Parse(reader["PK_LANGUAGE"].ToString());
                        result.descripcion = reader["DESCRIPTION"].ToString();
                        result.iso = reader["ISO"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error al pedir idioma", ex);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public bool DeleteLanguage(int languageId)
        {
            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());

            try
            {
                conn.Open();

                SqlCommand command = new SqlCommand("DELETE FROM LANGUAGES WHERE PK_LANGUAGE = @LanguageId", conn);
                command.Parameters.AddWithValue("@LanguageId", languageId);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0; 
            }
            catch (Exception ex)
            {
                throw new Exception("Error al delete idioma", ex);
            }
            finally
            {
                conn.Close();
            }
        }


    }
}
