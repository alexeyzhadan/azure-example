namespace AzureExample.Models
{
    public class ResultModel
    {
        public ResultModel() 
        {
            Success = true;
        }

        public ResultModel(string errorMessage)
        {
            Success = false;
            ErrorMessage = errorMessage;
        }

        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
