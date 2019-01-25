using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace SocialBL.Utils
{

    public class ConnectionError : Exception
    {

    }

    public class ApiAccess
    {

        private Uri _apiServerUri = null;


        public ApiAccess(string apiServerAddress)
        {

            _apiServerUri = new Uri(apiServerAddress);

        }


        public async Task<HttpResponseMessage> GetData(string query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = _apiServerUri;

                try
                {
                    var result = await client.GetAsync(query);
                    return result;
                }
                catch (Exception ex)
                {
                    throw new ConnectionError();
                }
            }
        }


        public async Task<Tuple<HttpResponseMessage, Model>> GetData<Model>(string query)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = _apiServerUri;

                try
                {
                    var result = await client.GetAsync(query);

                    switch (result.StatusCode)
                    {
                        case System.Net.HttpStatusCode.OK:
                            var content = await result.Content.ReadAsStringAsync();

                            JavaScriptSerializer JSserializer = new JavaScriptSerializer();
                            Model model = JSserializer.Deserialize<Model>(content);
                            return new Tuple<HttpResponseMessage, Model>(result, model);

                        default:
                            return new Tuple<HttpResponseMessage, Model>(result, default(Model));
                    }
                }
                catch (Exception ex)
                {
                    throw new ConnectionError();
                }
            }
        }


        public async Task<Tuple<HttpResponseMessage, Response>> PostData<Response, Model>(string query, Model model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = _apiServerUri;

                var jsonContent = new StringContent((new JavaScriptSerializer()).Serialize(model), Encoding.UTF8, "application/json");

                try
                {
                    var result = await client.PostAsync(query, jsonContent);


                    switch (result.StatusCode)
                    {
                        case System.Net.HttpStatusCode.OK:
                            var content = await result.Content.ReadAsStringAsync();


                            JavaScriptSerializer JSserializer = new JavaScriptSerializer();

                            Response response = JSserializer.Deserialize<Response>(content);//JSserializer.Deserialize<Response>(content);

                            return new Tuple<HttpResponseMessage, Response>(result, response);

                        default:
                            {
                                return new Tuple<HttpResponseMessage, Response>(result, default(Response));
                            }
                    }
                }
                catch (Exception ex)
                {
                    throw new ConnectionError();
                }

            }

        }


        public async Task<HttpResponseMessage> PostData<Model>(string query, Model model)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = _apiServerUri;

                var jsonContent = new StringContent(new JavaScriptSerializer().Serialize(model), Encoding.UTF8, "application/json");

                try
                {
                    var result = await client.PostAsync(query, jsonContent);
                    return result;
                }
                catch (Exception ex)
                {
                    throw new ConnectionError();
                }
            }
        }
    }
}