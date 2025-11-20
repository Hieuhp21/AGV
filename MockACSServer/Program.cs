using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Collections.Specialized;
using Newtonsoft.Json; // CẦN NUGET: Install-Package Newtonsoft.Json

namespace MockACSServer
{
    class Program
    {
        private static HttpListener _listener;
        private static readonly string Prefix = "http://localhost:8000/"; // THAY IP/PORT NẾU CẦN

        static void Main(string[] args)
        {
            Console.WriteLine("Mock ACS Server - C# Version");
            Console.WriteLine("=====================================");
            Console.WriteLine($"Listening at: {Prefix}");
            Console.WriteLine("Press CTRL+C to stop...\n");

            StartServer();
        }

        static void StartServer()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(Prefix);
            _listener.Start();

            Console.WriteLine("Server started. Waiting for AGV requests...\n");

            while (true)
            {
                try
                {
                    var context = _listener.GetContext();
                    ThreadPool.QueueUserWorkItem(_ => HandleRequest(context));
                }
                catch (Exception ex)
                {
                    if (_listener?.IsListening == false) break;
                    Console.WriteLine($"[ERROR] {ex.Message}");
                }
            }
        }

        static void HandleRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            try
            {
                if (request.HttpMethod == "POST" && request.Url.AbsolutePath == "/agv/report/")
                {
                    // Đọc dữ liệu POST (form-urlencoded)
                    using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
                    string postData = reader.ReadToEnd();
                    var form = System.Web.HttpUtility.ParseQueryString(postData);

                    // Log request
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] POST /agv/report/");
                    foreach (string key in form.AllKeys)
                    {
                        Console.WriteLine($"  {key} = {form[key]}");
                    }

                    // Tạo response giả lập (RealACS2AGV)
                    var agvResponse = new
                    {
                        agv_id = form["agv_id"] ?? "KD130",
                        action_0 = "G",     // Go
                        action_1 = "S",     // Straight
                        action_2 = "N",     // None
                        tag_id = "TAG001",
                        speed = "M",
                        front_sensor = true,
                        agv_mode = "auto",
                        acs_command = "move_forward",
                        wait_for = "0000",
                        alarm = 0,
                        depot = "DEPOT_A",
                        location = new[] { "LOC1", "LOC2" },
                        error = (string)null
                    };

                    string jsonResponse = JsonConvert.SerializeObject(agvResponse, Formatting.Indented);
                    byte[] buffer = Encoding.UTF8.GetBytes(jsonResponse);

                    // Gửi response
                    response.ContentType = "application/json";
                    response.ContentLength64 = buffer.Length;
                    response.OutputStream.Write(buffer, 0, buffer.Length);

                    Console.WriteLine($"[RESPONSE] 200 OK - Sent JSON");
                    Console.WriteLine(jsonResponse);
                    Console.WriteLine("─────────────────────────────────────\n");
                }
                else
                {
                    // 404
                    string msg = "Endpoint not found";
                    byte[] buffer = Encoding.UTF8.GetBytes(msg);
                    response.StatusCode = 404;
                    response.ContentType = "text/plain";
                    response.ContentLength64 = buffer.Length;
                    response.OutputStream.Write(buffer, 0, buffer.Length);
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 404 - {request.Url}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Message}");
                response.StatusCode = 500;
            }
            finally
            {
                response.Close();
            }
        }

        public static void StopServer()
        {
            _listener?.Stop();
            _listener?.Close();
            Console.WriteLine("Server stopped.");
        }
    }
}