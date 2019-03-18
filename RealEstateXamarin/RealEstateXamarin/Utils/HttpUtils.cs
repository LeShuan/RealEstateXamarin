using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RealEstateXamarin.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateXamarin.Utils
{
    class HttpUtils //<T>
    {
        /// <summary>
        /// Google autocomplete request
        /// </summary>
        /// <param name="url"></param>
        /// <returns>List of predictions of places based on the input string.</returns>
        public static async Task<List<Predictions>> GetPredictions(string url)
        {
            try
            {
                    List<Predictions> predictions = new List<Predictions>();
                    string json;

                    using (var httpClient = new HttpClient())
                    {
                         json = await httpClient.GetStringAsync(url);
                    }

                    JObject ans = JObject.Parse(json);
                    JArray arr = (ans["predictions"] as JArray);
            

                    foreach (JObject obj in arr)
                    {
                        predictions.Add(
                            new Predictions {
                            Name = ""+obj["description"].Value<string>(),
                            PlaceId = ""+obj["place_id"].Value<string>()
                            }
                        );
                    }


                return predictions;
               

            }
            catch (Exception e)
            {
                Debug.WriteLine("Error in GET: " + e.Message);
                return null;
            }

        }

        public static async Task<Address> GetLocationLonLat(string url)
        {
            try
            {
                List<Predictions> predictions = new List<Predictions>();
                string json;

                using (var httpClient = new HttpClient())
                {
                    json = await httpClient.GetStringAsync(url);
                }

                JObject ans = JObject.Parse(json);
                string name = "" + (string)ans.SelectToken("result.name");
                string addres = "" + (string)ans.SelectToken("result.formatted_address");
                double lat = (double)ans.SelectToken("result.geometry.location.lat");
                double lon = (double)ans.SelectToken("result.geometry.location.lng");
                Address address = new Address {

                Name =name,
                AddressData=addres,
                Latitud=lat,
                Longitud=lon

                };
            


                return address;


            }
            catch (Exception e)
            {
                Debug.WriteLine("Error in GET: " + e.Message);
                return null;
            }

        }

        public static async Task<string> directionsGetAddress(string url)
        {
            try
            {
                List<Predictions> predictions = new List<Predictions>();
                string json;

                using (var httpClient = new HttpClient())
                {
                    json = await httpClient.GetStringAsync(url);

                }

                JObject ans = JObject.Parse(json);

                string address = (string)ans["routes"][0]["legs"][0]["start_address"];

                return address;


            }
            catch (Exception e)
            {
                Debug.WriteLine("Error in GET: " + e.Message);
                return null;
            }

        }

        public static async Task<PropertyDetails> GetPropertyDetails(string id)
        {
            try
            {
                PropertyDetails propertyDetails = new PropertyDetails();
                string json;

                using (var httpClient = new HttpClient())
                {
                    json = await httpClient.GetStringAsync("https://www.vablemporium.com/Images/Property/" + id);

                }

                propertyDetails = JsonConvert.DeserializeObject<PropertyDetails>(json);

                return propertyDetails;


            }
            catch (Exception e)
            {
                Debug.WriteLine("Error in GET Property details: " + e.Message);
                return null;
            }

        }

        public static async Task<string> Upload(PropertyUpload propertyUpload)
        {
            try
            {
                string responsestr="";
             
                using (var httpClient = new HttpClient())
                    {
                        using (MultipartFormDataContent content = new MultipartFormDataContent())
                        {


                            content.Add(new StringContent(propertyUpload.Name), "Name");
                            content.Add(new StringContent("" + propertyUpload.Price), "Price");
                            content.Add(new StringContent(propertyUpload.Latitud + ""), "Latitud");
                            content.Add(new StringContent(propertyUpload.Longitud + ""), "Longitud");
                            content.Add(new StringContent(propertyUpload.Size + ""), "Size");
                            content.Add(new StringContent(propertyUpload.Address), "Address");
                            content.Add(new StringContent(propertyUpload.Rooms + ""), "Rooms");
                            content.Add(new StringContent(propertyUpload.ParkingSlots + ""), "ParkingSlots");
                            content.Add(new StringContent(propertyUpload.Bathrooms + ""), "Bathrooms");
                            content.Add(new StringContent(propertyUpload.Description), "Description");


                            foreach (var file in propertyUpload.Images)
                            {
                                content.Add(new StreamContent(file.GetStream()), "Images", file.FileName);
                            }

                            var response = await httpClient.PostAsync("https://www.vablemporium.com/Images/CreateProperty", content);


                            responsestr = response.Content.ReadAsStringAsync().Result;
                        }
                    }
                
                Debug.WriteLine(responsestr);

                return responsestr;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error in POST: " + e.Message);
                Debug.WriteLine("Error in POST: " + e.StackTrace);
                return null;
            }

        }

        public static async Task<string> HttpsTester(string url)
        {
            try
            {
                string json;
              
                using (var httpClient = new HttpClient())
                {
                    json = await httpClient.GetStringAsync(url);
                }

                return json;


            }
            catch (Exception e)
            {
                Debug.WriteLine("Error in HTTPSTESTER: " + e.Message);
                return null;
            }

        }

        public static async Task<List<PropertyItems>> GetPropertyItems(Search searchData)
        {
            try
            {
                List<PropertyItems> propertyItems = new List<PropertyItems>();

                using (var httpClient = new HttpClient())
                {
                    using (MultipartFormDataContent content = new MultipartFormDataContent())
                    {


                        content.Add(new StringContent(""+searchData.Distance), "Distance");
                        content.Add(new StringContent("" + searchData.Latitud), "Latitud");
                        content.Add(new StringContent("" + searchData.Longitud), "Longitud");
                        content.Add(new StringContent("" + searchData.PriceMin), "PriceMin");
                        content.Add(new StringContent("" + searchData.PriceMax), "PriceMax");
                        

                        var response = await httpClient.PostAsync("https://www.vablemporium.com/Images/Search", content);


                        string data = response.Content.ReadAsStringAsync().Result;

                        propertyItems = JsonConvert.DeserializeObject<List<PropertyItems>>(data);

                        Debug.WriteLine(data);
                    }
                }

                

                return propertyItems;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error in POST Search: " + e.Message);
                Debug.WriteLine("Error in POST Search: " + e.StackTrace);
                return null;
            }
        }

        /**
             using (HttpClientHandler handler = new HttpClientHandler()) { 
                 handler.ClientCertificateOptions = ClientCertificateOption.Manual;
             handler.SslProtocols = SslProtocols.Ssl2;
             string certFileName = Path.Combine("drverguzon.cer");
             bool doesExist = File.Exists(certFileName);
             X509Certificate2 cert = new X509Certificate2(certFileName);
             handler.ClientCertificates.Add(cert);
             //load the X.509 certificate and add to the web request X509Certificate2 cert = new X509Certificate2(certFileName); handler.ClientCertificates.Add(cert); client = new HttpClient(handler);
                 using (var httpClientHandler = new HttpClientHandler())
              {
             httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };


             **/

        /**
        public async Task UploadImageAsync(Stream image, string fileName)
        {
            HttpContent fileStreamContent = new StreamContent(image);
            fileStreamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "file", FileName = fileName };
            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(fileStreamContent);
                var response = await client.PostAsync("url", formData);
               // return response.IsSuccessStatusCode;
            }
        }
        async Task SendFileToServer(byte[] image, long folderName)
        {
            try
            {
                Uri webService = new Uri(URL + "upload/" + folderName);
                using (var content = new MultipartFormDataContent("----MyGreatBoundary"))
                {
                    using (var memoryStream = new MemoryStream(image))
                    {
                        using (var stream = new StreamContent(memoryStream))
                        {
                            content.Add(stream, "file", Guid.NewGuid().ToString() + ".jpg");
                            using (var message = await Client.PostAsync(webService, content))
                            {
                                if (message.ReasonPhrase.ToLower() == "OK".ToLower())
                                {
                                    imagesToUpload.Remove(imagesToUpload.FirstOrDefault(f => f.imageBytes == image));
                                    content.Dispose();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e) { }
        }


    **/


    }
}
