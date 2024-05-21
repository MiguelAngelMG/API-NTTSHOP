using API_nttshop.DAC;
using API_nttshop.Models.Entities;
using API_nttshop.Models.Request;
using API_nttshop.Models.Response.UsersResponse;
using API_nttshop.Models;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace API_nttshop.BC
{
    public class UsersBC
    {
        private readonly UsersDAC userDAC = new UsersDAC();

        public GetAllUsersResponse getAllUsers()
        {
            GetAllUsersResponse result = new GetAllUsersResponse();

            result.usersList = userDAC.GetAllUsers();

            if (result.usersList.Count() > 0)
            {
                result.httpStatus = System.Net.HttpStatusCode.OK;
            }
            else
            {
                result.httpStatus = System.Net.HttpStatusCode.NoContent;
                result.message = "No content";
            }

            return result;
        }
        public BaseReponseModel UpdateUser(UsersRequest request)
        {
            BaseReponseModel result = new BaseReponseModel();

            if (UpdateUserValidation(request))
            {
                bool correctOperation = userDAC.UpdateUser(request.user);

                if (correctOperation)
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

        public BaseReponseModel UpdateUserPassword(string password, int id)
        {
            BaseReponseModel result = new BaseReponseModel();

            if (ValidationPassword(password))
            {
                password = EncryptMD5(password);
                bool correctOperation = userDAC.UpdateUserPassword(password, id);

                if (correctOperation)
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
        public UserResponse InsertUser(UsersRequest request)
        {
             UserResponse result = new UserResponse();

            if (InsertUserValidation(request.user) )
            {
                string pass = EncryptMD5(request.user.Password);
                request.user.Password = pass;
                int id = 0;

                bool correctOperation = userDAC.InsertUser(request.user, out id);

                if (correctOperation)
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
        public BaseReponseModel DeleteUser(int request)
        {
            BaseReponseModel result = new BaseReponseModel();


            if (GetUserValidation(request))
            {
                bool eliminado = userDAC.DeleteUser(request);

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
        public UserResponse GetUser(int request)
        {
            UserResponse result = new UserResponse();


            if (GetUserValidation(request))
            {
                result.user = userDAC.GetUser(request);

                if (result != null)
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
        private bool UpdateUserValidation(UsersRequest request)
        {
            if (request != null
                && !string.IsNullOrWhiteSpace(request.user.Login)
                && !string.IsNullOrWhiteSpace(request.user.Password)
                && !string.IsNullOrWhiteSpace(request.user.Name)
                && !string.IsNullOrWhiteSpace(request.user.Surname1)
                && !string.IsNullOrWhiteSpace(request.user.Email)
                && !string.IsNullOrWhiteSpace(request.user.Languages)
                && request.user.PkUser > 0
                && request.user.Rate != 0
               )
            {
                return true;
            }
            else
            {
                return false;
            }
             
        }
        private bool InsertUserValidation(User request)
        {
            if (request != null
                && !string.IsNullOrWhiteSpace(request.Login)
                && !string.IsNullOrWhiteSpace(request.Password)
                && !string.IsNullOrWhiteSpace(request.Name)
                && !string.IsNullOrWhiteSpace(request.Surname1)
                && !string.IsNullOrWhiteSpace(request.Email)
                && !string.IsNullOrWhiteSpace(request.Languages)
                && request.Rate != 0
                && ValidationPassword(request.Password))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        private bool GetUserValidation(int request)
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
        private bool ValidationPassword(string password)
        {
            string regex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{1,}$"; //Debe tener minimo una mayuscula, minuscula y numero 

            if (Regex.IsMatch(password, regex))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private string EncryptMD5(string pass)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Metemos la contraseña en un array y para transjormarlo tenemos que calcular el hash MD5
                byte[] data = md5Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));

                // Un StringBuilder para convertir los byte en texto
                System.Text.StringBuilder text = new System.Text.StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    text.Append(data[i].ToString("x2")); //X2 es para encryptarlo hexadecimal 
                }

                // Devolver la cadena hexadecimal
                return text.ToString();
            }
        }
    }
}
