namespace xv_dotnet_demo_v2_domain.Exceptions
{
    public class ValidateTokenException : Exception
    {
        public ValidateTokenException(string code, string token = null) : base(GetMessageByErrorCode(code, token))
        { }

        private static string GetMessageByErrorCode(string code, string token = null)
        {
            if (code == "3001")
            {
                return "Authorization header is not present.";
            }

            if (code == "3002")
            {
                return $"An error occured when trying to validate the token: {token}";
            }

            if (code == "3003")
            {
                return $"The token: {token} is not valid";
            }

            return string.Empty;
        }
    }
}
