using System.Threading.Tasks;

namespace EchoNest.Song
{
    public class Identify : EchoNestService
    {
        #region Fields

        private const string Url = "song/identify";

        #endregion Fields

        #region Methods

        public IdentifyResponse Execute(FingerprintArgument argument)
        {
            argument.ApiKey = ApiKey;
            argument.BaseUrl = Url;

            return Execute<IdentifyResponse>(argument.ToString());
        }

        public Task<IdentifyResponse> ExecuteAsync(FingerprintArgument argument)
        {
            argument.ApiKey = ApiKey;
            argument.BaseUrl = Url;

            return ExecuteAsync<IdentifyResponse>(argument.ToString());
        }

        #endregion Methods
    }
}