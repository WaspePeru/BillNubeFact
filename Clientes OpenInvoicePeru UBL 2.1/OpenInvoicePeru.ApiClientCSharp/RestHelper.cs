﻿using RestSharp;

namespace OpenInvoicePeru.ApiClientCSharp
{
    public static class RestHelper<TRequest, TResponse>
        where TRequest : class
        where TResponse : class, new()
    {
        public static TResponse Execute(string metodo, TRequest request)
        {
            var client = new RestClient("http://localhost/OpenInvoicePeruUBL21/api");

            var restRequest = new RestRequest(metodo, Method.POST)
            {
                RequestFormat = DataFormat.Json
            };

            restRequest.AddBody(request);

            var restResponse = client.Execute<TResponse>(restRequest);
            return restResponse.Data;
        }

    }
}