using API_nttshop.DAC;
using API_nttshop.Models;
using API_nttshop.Models.Response.ManagementUsersResponse;
using System.Security.Cryptography;

namespace API_nttshop.BC
{
    public class ManagementLoginBC
    {
        private readonly ManagementUsersDAC users = new ManagementUsersDAC();
        public ManagementUsersLoginResponse getLogin(string user, string pass)
        {
            ManagementUsersLoginResponse result = new ManagementUsersLoginResponse();


            if (loginValidation(user, pass))
            {
                pass = EncryptMD5(pass);
                bool esValido = users.getUserLogin(user, pass, out string message, out int idUser);

                if (esValido)
                {
                    result.httpStatus = System.Net.HttpStatusCode.OK;
                    result.idUser = idUser;
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
        private bool loginValidation(string user, string pass)
        {
            if (!string.IsNullOrWhiteSpace(user)
                && !string.IsNullOrWhiteSpace(pass))
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
