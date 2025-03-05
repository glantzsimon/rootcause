using K9.Globalisation;

namespace K9.WebApplication.Options
{
    public class AjaxPopupOptions
    {
        public string SuccessMessage { get; set; } = Dictionary.RecordUpdatedSuccessfully;
        public string FailureMessage { get; set; } = Dictionary.RecordUpdateFailed;
    }
}