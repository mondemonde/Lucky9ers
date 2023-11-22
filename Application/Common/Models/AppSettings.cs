
namespace Lucky9.Application.Common.Models
{
   
    public class AppSettings
    {

        public AppSettings()
        {
            JWT = new JWT();

        }

        public bool UseInMemoryDatabase { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public JWT JWT { get; set; }
    }



    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }

    public class JWT
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public int TokenExpirationInMinutes { get; set; }


    }
}