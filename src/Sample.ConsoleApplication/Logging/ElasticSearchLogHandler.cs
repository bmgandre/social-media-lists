using Elasticsearch.Net;
using System;
using System.Text;

namespace SocialMediaLists.Sample.ConsoleApplication.Logging
{
    internal static class ElasticSearchLogHandler
    {
        public static void Handle(IApiCallDetails apiCallDetails)
        {
            if (!apiCallDetails.Success)
            {
                HandleRequest(apiCallDetails);
                HandleResponse(apiCallDetails);
            }
        }

        private static void HandleRequest(IApiCallDetails apiCallDetails)
        {
            if (apiCallDetails.RequestBodyInBytes != null)
            {
                Console.WriteLine(
                    $"{apiCallDetails.HttpMethod} {apiCallDetails.Uri} \n" +
                    $"{Encoding.UTF8.GetString(apiCallDetails.RequestBodyInBytes)}");
            }
            else
            {
                Console.WriteLine($"{apiCallDetails.HttpMethod} {apiCallDetails.Uri}");
            }
        }

        private static void HandleResponse(IApiCallDetails apiCallDetails)
        {
            if (apiCallDetails.ResponseBodyInBytes != null)
            {
                Console.WriteLine($"Status: {apiCallDetails.HttpStatusCode}\n" +
                         $"{Encoding.UTF8.GetString(apiCallDetails.ResponseBodyInBytes)}\n" +
                         $"{new string('-', 30)}\n");
            }
            else
            {
                Console.WriteLine($"Status: {apiCallDetails.HttpStatusCode}\n" +
                         $"{new string('-', 30)}\n");
            }
        }
    }
}