using Android.Media;
using Android.Media.TV;
using CyberSiberiaApp.Model.DB;
using CyberSiberiaApp.Model.DB.EntityModels;
using CyberSiberiaApp.Model.ReportService.Models;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using RestSharp;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CyberSiberiaApp.Model.ReportService
{
    public static class ReportHandler
    {
        private static string GetJson(int flatId)
        {
            try
            {


                ScriptEngine engine = Python.CreateEngine();
                ScriptScope scope = engine.CreateScope();
                engine.Execute(@"convert = lambda filename: open(filename, ""rb"").read()", scope);
                dynamic convert = scope.GetVariable("convert");

                using (Context context = new())
                {
                    List<int> categoriesId =
                        (from defect in context.Defects
                         where defect.FlatId == flatId
                         select defect.CategoryId).ToList();

                    List<ProblemCategoryName> flatCategories = new List<ProblemCategoryName>();

                    foreach (var i in categoriesId)
                    {
                        List<Defect> categoryDefects =
                            (from defect in context.Defects
                             where defect.FlatId == flatId && defect.CategoryId == i
                             select defect).ToList();

                        foreach (var def in categoryDefects)
                        {
                            List<ReportService.Models.Image> images = new();

                            //foreach(var img in context.Images)
                            //{
                            //    FileStream stream = new FileStream(img.Path, FileMode.Open);
                            //    StreamReader reader = new StreamReader(stream);
                            //    string hui = reader.ReadToEnd();

                            //    images.Add(new Models.Image()
                            //    {

                            //        filename = img.Path,
                            //        metadata = Convert.ToBase64String(convert(img.Path))
                            //    });
                            //}
                            ProblemCategoryName problem = new();
                            problem.description = def.Description;
                            problem.gost = def.Gost;
                            problem.images = images;
                            flatCategories.Add(problem);
                        }
                    }

                    Report report = new Report()
                    {
                        flat_unique_id = new FlatUniqueId()
                        {
                            problem_category_name = flatCategories
                        }
                    };

                    string json = JsonSerializer.Serialize(report);

                    return json;
                }
            }
            catch
            {
                return "";
            }
        }

        public async static void Post(int flatId)
        {
            try
            {
                string json = GetJson(flatId);
                string url = $"http://d1ffic00lt.com/app/api/v1.0/db/detections/";

                //string username = "shushakishkish";
                //string password = "kekg123gg667";

                //string encoded = Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes($"{username}:{password}"));
                //var client = new RestClient(url);
                //var request = new RestRequest("POST");
                //request.AddJsonBody(json);
                //// request.RequestFormat = DataFormat.Json;
                //request.AddHeader("Authorization", $"Basic {encoded}");
                //RestResponse response = client.Execute(request);

                HttpClient httpClient = new HttpClient();

                HttpContent content = new StringContent(json, System.Text.Encoding.UTF8);

                HttpResponseMessage responseMessage = await httpClient.PostAsync(url, content);

                Console.WriteLine(await responseMessage.Content.ReadAsStringAsync());
            }
            catch
            {

            }
        }

        public async static void Get(int flatId)
        {
            try
            {
                string url = $"http://d1ffic00lt.com/app/api/v1.0/db/detections/{flatId}";

                //string username = "shushakishkish";
                //string password = "kekg123gg667";

                //string encoded = Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes($"{username}:{password}"));
                //var client = new RestClient(url);
                //var request = new RestRequest("POST");
                //request.AddJsonBody(json);
                //// request.RequestFormat = DataFormat.Json;
                //request.AddHeader("Authorization", $"Basic {encoded}");
                //RestResponse response = client.Execute(request);

                HttpClient httpClient = new HttpClient();

                HttpResponseMessage responseMessage = await httpClient.GetAsync(url);

                string result = await responseMessage.Content.ReadAsStringAsync();
                result = result.Remove(result.Length - 2);
                result = result.Substring(result.LastIndexOf("\"") + 1);

                byte[] buffer = Convert.FromBase64String(result);

                string file = Convert.ToString(buffer);

                string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"{DateTime.Now.ToShortTimeString()}.docx");

                await File.WriteAllBytesAsync(fileName, buffer);
            }
            catch { }
            
        }
    }
}
