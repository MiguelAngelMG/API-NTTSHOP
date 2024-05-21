using API_nttshop.Models.Entities;

namespace API_nttshop.Models.Reponse.LanguageResponse
{
    public class GetAllLanguagesResponse : BaseReponseModel
    {
        public List<Language> languageList { get; set; }
    }
}
