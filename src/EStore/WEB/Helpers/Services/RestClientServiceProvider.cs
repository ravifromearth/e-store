using DAL.Models.Common;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;
using WEB.Helpers.Services.Base;

namespace WEB.Helpers.Services
{
    public class RestClientServiceProvider : IRestClientServiceProvider, IDisposable
    {
        protected readonly AppConfigration _appConfigration;
        protected RestClient _RestClient;
        protected RestClientOptions _RestClientOptions;

        public RestClientServiceProvider(IOptions<AppConfigration> options)
        {
            _appConfigration = options.Value;

            _RestClientOptions = new RestClientOptions()
            {
                BaseUrl = new Uri(_appConfigration.ApiBaseUrlSSL),
            };
            
            _RestClient = new RestClient(_RestClientOptions, configureSerialization: s => s.UseNewtonsoftJson());
        }

        public void SetAuthenticator(string token)
        {
            _RestClientOptions.Authenticator = string.IsNullOrWhiteSpace(token) ? null : new JwtAuthenticator(token);
            _RestClient = new RestClient(_RestClientOptions, configureSerialization: s => s.UseNewtonsoftJson());
        }

        public async Task<T> Get<T>(string path)
        {
            return await _RestClient.GetAsync<T>(new RestRequest(path)).ConfigureAwait(false);
        }

        public async Task<T> Post<T>(string path, object data)
        {
            return await _RestClient.PostAsync<T>(new RestRequest(path).AddJsonBody(data)).ConfigureAwait(false);
        }

        public async Task<T> Put<T>(string path, object data)
        {
            return await _RestClient.PutAsync<T>(new RestRequest(path).AddJsonBody(data)).ConfigureAwait(false);
        }

        public async Task<T> Delete<T>(string path)
        {
            return await _RestClient.DeleteAsync<T>(new RestRequest(path)).ConfigureAwait(false);
        }

        #region Dispose

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _RestClient?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ServiceProvider()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
