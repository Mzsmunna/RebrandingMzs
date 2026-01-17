using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InsightInHealth.Infrastructure.Helper
{
    public static class CustomHttpClient
    {
        private static readonly string SecretKey = "";

        public static async Task<T?> GetOne<T>(string baseApiUrl, string callingApiMethod, string token = "", ILogger? logger = null) where T : class //new()
        {
            //T? data = new T();
            T? data = null;

            if (string.IsNullOrEmpty(token))
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseApiUrl);
                    //var responseTask = client.GetAsync(callingApiMethod);
                    //responseTask.Wait();
                    //var result = responseTask.Result;
                    var result = await client.GetAsync(callingApiMethod).ConfigureAwait(false);
                    if (result.IsSuccessStatusCode)
                    {
                        //var readTask = result.Content.ReadAsAsync<T>();
                        //readTask.Wait();
                        //data = readTask.Result;
                        var dataString = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                        data = JsonConvert.DeserializeObject<T>(dataString);
                    }
                }
            }
            else
            {
                string SCDNEndpoint = $"{ baseApiUrl }/{ callingApiMethod }";
                using (var client = new HttpClient())
                {
                    try
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        var content = await client.GetStringAsync(SCDNEndpoint).ConfigureAwait(false);
                        data = JsonConvert.DeserializeObject<T>(content);
                    }
                    catch (Exception ex)
                    {
                        if (logger != null) logger.LogWarning("CustomHttpClient.GetOne Error: " + ex.Message);
                    }
                }
            }
       
            return data;
        }

        public static async Task<List<T>> GetList<T>(string baseApiUrl, string callingApiMethod, string token = "", ILogger? logger = null) where T : class //new()
        {
            List<T> dataList = new List<T>();
            if (string.IsNullOrEmpty(token))
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseApiUrl);
                    //var responseTask = client.GetAsync(callingApiMethod);
                    //responseTask.Wait();
                    //var result = responseTask.Result;
                    var result = await client.GetAsync(callingApiMethod).ConfigureAwait(false);
                    if (result.IsSuccessStatusCode)
                    {
                        //var readTask = result.Content.ReadAsAsync<List<T>>();
                        //readTask.Wait();
                        //dataList = readTask.Result;
                        var dataListString = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                        dataList = JsonConvert.DeserializeObject<List<T>>(dataListString) ?? [];
                    }
                }
            } 
            else
            {
                string SCDNEndpoint = $"{ baseApiUrl }/{ callingApiMethod }";
                using (var client = new HttpClient())
                {
                    try
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        var content = await client.GetStringAsync(SCDNEndpoint).ConfigureAwait(false);
                        dataList = JsonConvert.DeserializeObject<List<T>>(content) ?? [];
                    }
                    catch (Exception ex)
                    {
                        if (logger != null) logger.LogWarning("CustomHttpClient.GetList Error: " + ex.Message);
                    }
                }
            }

            return dataList;
        }

        public static async Task<ResponseBody?> GetAsync<ResponseBody>(string baseApiUrl, string apiEndPoint, string token, ILogger? logger = null)
        {
            try
            {
                ResponseBody? response;
                string SCDNEndpoint = baseApiUrl + apiEndPoint;
                var client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var content = await client.GetStringAsync(SCDNEndpoint).ConfigureAwait(false);
                //data = JsonConvert.DeserializeObject<T>(content);

                if (typeof(ResponseBody) == typeof(string) || typeof(ResponseBody) == typeof(String))
                {
                    response = (ResponseBody)(object)Convert.ChangeType(content, typeof(ResponseBody));
                }
                else if (typeof(ResponseBody).BaseType == typeof(object))
                {
                    response = JsonConvert.DeserializeObject<ResponseBody>(content);
                }
                else
                {
                    response = (ResponseBody)(object)Convert.ChangeType(content, typeof(ResponseBody));
                }
                return response;
            }
            catch (Exception ex)
            {
                if (logger != null) logger.LogWarning("CustomHttpClient.GetAsync Error: " + ex.Message);
            }
            return default;
        }

        public static async Task<ResponseBody?> PostAsJsonAsync<RequestBody, ResponseBody>(RequestBody data, string baseApiUrl, string apiEndPoint, string token, ILogger? logger = null)
        {
            ResponseBody? response;

            if (data != null)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(baseApiUrl);
                var result = await client.PostAsJsonAsync(apiEndPoint, data);
                if (result.IsSuccessStatusCode)
                {
                    var responseValue = result.Content.ReadAsStringAsync().Result;
                    if (typeof(ResponseBody) == typeof(string) || typeof(ResponseBody) == typeof(String))
                    {
                        response = (ResponseBody)(object)Convert.ChangeType(responseValue, typeof(ResponseBody));
                    }
                    else if (typeof(ResponseBody).BaseType == typeof(object))
                    {
                        response = JsonConvert.DeserializeObject<ResponseBody>(responseValue);
                    }
                    else
                    {
                        response = (ResponseBody)(object)Convert.ChangeType(responseValue, typeof(ResponseBody));
                    }
                    return response;
                }
            }
            return default;
        }

        public static async Task<T2?> PostOne<T, T2>(string baseApiUrl, string callingApiMethod, T data, string token = "", ILogger? logger = null) where T : class //new()
        {
            T2? response = default(T2);

            if (data != null)
            {
                if (string.IsNullOrEmpty(token))
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(baseApiUrl);
                        //var responseTask = client.PostAsJsonAsync(callingApiMethod, data);
                        //responseTask.Wait();
                        //var result = responseTask.Result;
                        var result = await client.PostAsJsonAsync(callingApiMethod, data).ConfigureAwait(false);
                        if (result.IsSuccessStatusCode)
                        {
                            //var readTask = result.Content.ReadAsAsync<T>();
                            //readTask.Wait();
                            //data = readTask.Result;
                            var responseString = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                            response = JsonConvert.DeserializeObject<T2>(responseString);
                        }
                    }
                } 
                else 
                {
                    string SCDNEndpoint = $"{ baseApiUrl }/{ callingApiMethod }";

                    using (var client = new HttpClient())
                    {
                        try
                        {
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            var content = await client.PostAsJsonAsync(SCDNEndpoint, data).ConfigureAwait(false);

                            if (content.IsSuccessStatusCode)
                            {
                                //var readTask = content.Content.ReadAsStringAsync().Result;
                                //var value = readTask;
                                //return value;
                                var responseString = await content.Content.ReadAsStringAsync().ConfigureAwait(false);
                                response = JsonConvert.DeserializeObject<T2>(responseString);                           
                            }                      
                        }
                        catch (Exception ex)
                        {
                            if (logger != null) logger.LogWarning("CustomHttpClient.PostOne.T2 Error: " + ex.Message);
                        }
                    }
                }

                return response;
            }
            else
            {
                return default(T2);
            }
        }

        public static async Task<string> PostOne<T>(T data, string baseApiUrl, string callingApiMethod, string token = "", ILogger? logger = null) where T : class //new()
        {
            string response = string.Empty;

            if (data != null)
            {
                if (string.IsNullOrEmpty(token))
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(baseApiUrl);
                        //var responseTask = client.PostAsJsonAsync(callingApiMethod, data);
                        //responseTask.Wait();
                        //var result = responseTask.Result;
                        var result = await client.PostAsJsonAsync(callingApiMethod, data).ConfigureAwait(false);
                        if (result.IsSuccessStatusCode)
                        {
                            //var readTask = result.Content.ReadAsStringAsync().Result;
                            //response = readTask;
                            response = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                        }
                    }
                } 
                else
                {
                    string SCDNEndpoint = $"{ baseApiUrl }/{ callingApiMethod }";

                    using (var client = new HttpClient())
                    {
                        try
                        {
                            client.DefaultRequestHeaders.Clear();
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            var content = await client.PostAsJsonAsync(SCDNEndpoint, data).ConfigureAwait(false);

                            if (content.IsSuccessStatusCode)
                            {
                                //var readTask = content.Content.ReadAsStringAsync().Result;
                                //response = readTask;
                                response = await content.Content.ReadAsStringAsync().ConfigureAwait(false);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (logger != null) logger.LogWarning("CustomHttpClient.PostOne Error: " + ex.Message);
                        }
                    }
                }
                
                return response;
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GenerateQueryString<T>(T parameter)
        {
            StringBuilder queryStringBuilder = new StringBuilder();

            if (parameter is null) return string.Empty;

            // Get the type of the class
            Type type = parameter.GetType();

            // Get all properties of the class
            List<PropertyInfo> properties = type.GetProperties().ToList();

            // Loop through each property and add it to the query string
            foreach (PropertyInfo property in properties)
            {
                if (property is null) continue;

                object? value = property.GetValue(parameter);

                // Only include properties with non-null and non-empty values in the query string
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    // Encode the property name and value to handle special characters properly
                    string encodedName = Uri.EscapeDataString(property.Name);
                    string encodedValue = Uri.EscapeDataString(value.ToString() ?? "");

                    // Append the property and its value to the query string
                    queryStringBuilder.Append($"{encodedName}={encodedValue}&");
                }
            }

            // Remove the trailing '&' character from the query string if it exists
            if (queryStringBuilder.Length > 0)
            {
                queryStringBuilder.Length--; // Remove the last character (which is '&')
            }

            return queryStringBuilder.ToString();
        }
    }
}
