namespace emmad.Services
{
    public class HelperService
    {
        /// <summary>
        /// Retourne l'extension de la photo
        /// </summary>
        /// <param name="base64String">base64 string.</param>
        /// <returns>string de l'extension de la photo</returns>
        public static string GetFileExtension(string base64String)
        {
            var data = base64String.Substring(0, 5);

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                case "AAABA":
                    return "ico";
                default:
                    return string.Empty;
            }
        }
    }
}
