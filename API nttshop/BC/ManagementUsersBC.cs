using API_nttshop.DAC;
using API_nttshop.Models.Entities;
using API_nttshop.Models.Request;
using API_nttshop.Models.Response.UsersResponse;
using API_nttshop.Models;
using API_nttshop.Models.Response.ManagementUsersResponse;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace API_nttshop.BC
{
    public class ManagementUsersBC
    {
        private readonly ManagementUsersDAC managementUserDAC = new ManagementUsersDAC();

        public GetAllManagementUsersResponse getAllManagementUsers()
        {
            GetAllManagementUsersResponse result = new GetAllManagementUsersResponse();

            result.usersList = managementUserDAC.GetAllManagementUsers();

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
        public BaseReponseModel UpdateManagementUser(ManagementUsersRequest request)
        {
            BaseReponseModel result = new BaseReponseModel();

            if (UpdateManagementUserValidation(request))
            {
                bool correctOperation = managementUserDAC.UpdateManagementUser(request.user);

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
        public BaseReponseModel InsertManagementUser(ManagementUsersRequest request)
        {
            BaseReponseModel result = new BaseReponseModel();

            if (InsertManagementUserValidation(request.user))
            {
                string pass = EncryptMD5(request.user.Password);
                request.user.Password = pass;
                bool correctOperation = managementUserDAC.InsertManagementUser(request.user);

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
        public BaseReponseModel DeleteManagementUser(int request)
        {
            BaseReponseModel result = new BaseReponseModel();


            if (GetManagementUserValidation(request))
            {
                bool eliminado = managementUserDAC.DeleteManagementUser(request);

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
        public BaseReponseModel GetManagementUser(int request)
        {
            ManagementUsersResponse result = new ManagementUsersResponse();


            if (GetManagementUserValidation(request))
            {
                result.getUser = managementUserDAC.GetManagementUser(request);

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
        public BaseReponseModel UpdateManagementUserPassword(string password, int id)
        {
            BaseReponseModel result = new BaseReponseModel();

            if (ValidationPassword(password))
            {
                password = EncryptMD5(password);
                bool correctOperation = managementUserDAC.UpdateManagementUserPassword(password, id);

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
        private bool UpdateManagementUserValidation(ManagementUsersRequest request)
        {
            if (request != null
                && !string.IsNullOrWhiteSpace(request.user.Login)
                && !string.IsNullOrWhiteSpace(request.user.Password)
                && !string.IsNullOrWhiteSpace(request.user.Name)
                && !string.IsNullOrWhiteSpace(request.user.Surname1)
                && !string.IsNullOrWhiteSpace(request.user.Email)
                && !string.IsNullOrWhiteSpace(request.user.Languages)
                && request.user.PkUser > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        private bool InsertManagementUserValidation(ManagementUser request)
        {
            if (request != null
                && !string.IsNullOrWhiteSpace(request.Login)
                && !string.IsNullOrWhiteSpace(request.Password)
                && !string.IsNullOrWhiteSpace(request.Name)
                && !string.IsNullOrWhiteSpace(request.Surname1)
                && !string.IsNullOrWhiteSpace(request.Email)
                && !string.IsNullOrWhiteSpace(request.Languages)
                && ValidationPassword(request.Password))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        private bool GetManagementUserValidation(int request)
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
