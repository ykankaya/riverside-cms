using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace Riverside.Cms.Utilities.RestSharpExtensions
{
    /// <summary>
    /// Credit: https://stackoverflow.com/questions/41390647/how-to-use-restsharp-netcore-in-asp-net-core
    /// </summary>
    public static class RestClientExtensions
    {
        public static async Task<RestResponse<T>> ExecuteAsync<T>(this RestClient client, RestRequest request) where T : new()
        {
            TaskCompletionSource<IRestResponse<T>> taskCompletion = new TaskCompletionSource<IRestResponse<T>>();
            RestRequestAsyncHandle handle = client.ExecuteAsync<T>(request, r => taskCompletion.SetResult(r));
            return (RestResponse<T>)(await taskCompletion.Task);
        }
    }
}
