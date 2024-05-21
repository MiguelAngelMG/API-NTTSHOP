using API_nttshop.Models;
using API_nttshop.Models.Entities;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
namespace API_nttshop.DAC
{
    public class UsersDAC
    {
        private readonly NttshopContext context = new NttshopContext();

        public List<User> GetAllUsers()
        {
            List<User> result = new List<User>();
         
            try
            {
                result = context.Users.ToList();
            }
            
            catch (Exception ex)
            {
                throw new Exception("Error al obtener todos los users", ex);
            }


            return result;
        }

        public bool UpdateUser(User user)
        {
             
            try
            {
                User usuario = context.Users.Find(user.PkUser);
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
        public bool InsertUser(User user, out int id)
        { 
            try
            {
                context.Users.Add(user);
                context.SaveChanges();
                id = user.PkUser;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el usuario", ex);
            }
        }


        public User GetUser(int id)
        {
            User result = new User();
          
            try
            {

                result = context.Users.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al pedir User", ex);
            }


            return result;
        }

        public bool DeleteUser(int userId)
        {
            User user = new User();
            try
            {
               user = context.Users.Find(userId);
                if (user != null)
                {
                    context.Users.Remove(user);
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
        public bool UpdateUserPassword(string passwordNew, int id)
        {

            try
            {
                User usuario = context.Users.Find(id);
                if (usuario == null)
                {
                    throw new Exception("Error: El usuario no se encontró en la base de datos");
                   
                }
                else if(usuario.Password == passwordNew)
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
                command.Parameters.AddWithValue("@Selection", 1); //Indicamos que es un usuario normal

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
            catch(Exception ex)
            {
                throw new Exception("Error al actualizar el producto", ex);
            }
            finally
            {
                conn.Close();
            }


            return esValido;
        }


    }
}
