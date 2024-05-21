using API_nttshop.Models.Entities;
using API_nttshop.Models;
using API_nttshop.Controllers;
using System.Data;
using System.Data.SqlClient;

namespace API_nttshop.DAC
{
    public class ManagementUsersDAC
    {
        private readonly NttshopContext context = new NttshopContext();

        public List<ManagementUser> GetAllManagementUsers()
        {
            List<ManagementUser> result = new List<ManagementUser>();

            try
            {
                result = context.ManagementUsers.ToList();
            }

            catch (Exception ex)
            {
                throw new Exception("Error al obtener todos los users", ex);
            }


            return result;
        }

        public bool UpdateManagementUser(ManagementUser user)
        {

            try
            {

                ManagementUser usuario = context.ManagementUsers.Find(user.PkUser);
                user.Password = usuario.Password;
                if (usuario == null)
                {
                    throw new Exception("El usuario no se encontró en la base de datos");
                }
                context.Entry(usuario).CurrentValues.SetValues(user);

                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el usuario", ex);
            }
        }
        public bool InsertManagementUser(ManagementUser user)
        {
            try
            {
                context.ManagementUsers.Add(user);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el usuario", ex);
            }
        }


        public ManagementUser GetManagementUser(int id)
        {
            ManagementUser result = new ManagementUser();

            try
            {

                result = context.ManagementUsers.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al pedir User", ex);
            }


            return result;
        }

        public bool DeleteManagementUser(int userId)
        {
            ManagementUser user = new ManagementUser();
            try
            {
                user = context.ManagementUsers.Find(userId);
                if (user != null)
                {
                    context.ManagementUsers.Remove(user);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el usuario", ex);
            }

        }
        public bool UpdateManagementUserPassword(string passwordNew, int id)
        {

            try
            {
                ManagementUser usuario = context.ManagementUsers.Find(id);
                if (usuario == null)
                {
                    throw new Exception("Error: El usuario no se encontró en la base de datos");

                }
                else if (usuario.Password == passwordNew)
                {
                    throw new Exception("Error: La contraseña es la misma. utilice un acontraseña nueva");

                }
                else
                {
                    usuario.Password = passwordNew;
                    context.Entry(usuario).CurrentValues.SetValues(usuario);

                    context.SaveChanges();
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el usuario", ex);
            }
        }
        public bool getUserLogin(string user, string pass, out string message, out int idUser)
        {
            message = "";
            bool esValido = false;
            int result = 0;

            SqlConnection conn = new SqlConnection(ConnectionManager.getConnectionString());
            SqlCommand command;

            try
            {
                conn.Open();

                command = new SqlCommand("sp_GetUserLogin", conn);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Login", user);
                command.Parameters.AddWithValue("@Password", pass);
                command.Parameters.AddWithValue("@Selection", 2); //Indicamos que es un usuario normal

                // Parámetros de salida
                SqlParameter outputIdProduct = new SqlParameter("@Result", SqlDbType.Int);
                outputIdProduct.Direction = ParameterDirection.Output;
                command.Parameters.Add(outputIdProduct);

                // Parámetros de salida
                SqlParameter outputIdUserProduct = new SqlParameter("@idUser", SqlDbType.Int);
                outputIdUserProduct.Direction = ParameterDirection.Output;
                command.Parameters.Add(outputIdUserProduct);

                command.ExecuteNonQuery();

                result = Convert.ToInt32(command.Parameters["@Result"].Value);
                idUser = Convert.ToInt32(command.Parameters["@idUser"].Value);

                if (result != 1)
                {
                    esValido = false;
                }
                else
                {
                    esValido = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtner el usuario", ex);
            }
            finally
            {
                conn.Close();
            }


            return esValido;
        }


    }
}
