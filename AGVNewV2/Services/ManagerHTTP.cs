using System;
using System.Diagnostics;
using System.Threading;
using System.Net.Http;
using Newtonsoft.Json;
using AGVNew.Models;
using AGVNew.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AGVNew.Services
{
    public class ManagerHTTP
    {
        public const int Sleep_Thread = 500;
        private static ManagerHTTP _instance;
        public Thread Thread;
        public bool Check_Thread;
        public bool Communication_OK_Check;
        public bool IsServerConnected { get; private set; }
        private readonly Stopwatch sw_Receive = new Stopwatch();
        public int fullpath_request_count;
        public int http_result;
        private readonly HttpClient _httpClient;
        private const int MaxRetryAttempts = 3;
        private const int RetryDelayMs = 2000;

        public static ManagerHTTP Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ManagerHTTP();
                }
                return _instance;
            }
        }

        private ManagerHTTP()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri($"http://{AGVData.Instance.option.IP}:{AGVData.Instance.option.Port}"),
                Timeout = TimeSpan.FromMilliseconds(AGVData.Instance.option.Http_TimeOut_ms)
            };
            Communication_OK_Check = false;
            ManagerLog.Instance.AddLog("System", "HTTP", "HttpClient initialized");
        }

        public void Http_Thread_Start()
        {
            if (Thread != null && Thread.IsAlive)
            {
                ManagerLog.Instance.AddLog("System", "HTTP", "Thread - Already running");
                return;
            }

            Check_Thread = true;
            Thread = new Thread(Http_Thread_Run) { IsBackground = true };
            Thread.Start();
            ManagerLog.Instance.AddLog("System", "HTTP", "Thread - Start");
        }

        public void StopThread()
        {
            Check_Thread = false;
            if (Thread != null && Thread.IsAlive)
            {
                Thread.Interrupt();
                Thread.Join();
                ManagerLog.Instance.AddLog("System", "HTTP", "Thread - Stopped");
            }
            Thread = null;
        }

        private async void Http_Thread_Run()
        {
            while (Check_Thread)
            {
                try
                {
                    MitsubishiPLC.Instance.UpdateStateFromPLC();

                    string endpoint = "/agv/report/";
                    string real_send_msg = Make_Send_2_ACS_Set();
                    var content = new FormUrlEncodedContent(ParseQueryString(real_send_msg));

                    bool success = false;
                    for (int attempts = 1; attempts <= MaxRetryAttempts && !success; attempts++)
                    {
                        try
                        {
                            ManagerLog.Instance.AddLog("System", "HTTP-Send", $"Attempt {attempts}: {real_send_msg}");

                            var response = await _httpClient.PostAsync(endpoint, content)
                                .ConfigureAwait(false);

                            if (response.IsSuccessStatusCode)
                            {
                                string json = await response.Content.ReadAsStringAsync()
                                    .ConfigureAwait(false);

                                ManagerLog.Instance.AddLog("System", "HTTP-Receive", json);
                                Parsing_Real_Message_2_AGV(json);
                                timeout_Init();
                                success = true;
                                IsServerConnected = true;
                            }
                            else
                            {
                                http_result = (int)response.StatusCode;
                                ManagerLog.Instance.AddLog("System", "HTTP-Result", $"Status: {http_result}");
                                if (attempts < MaxRetryAttempts)
                                    await Task.Delay(RetryDelayMs).ConfigureAwait(false);
                            }
                        }
                        catch (Exception ex)
                        {
                            ManagerLog.Instance.AddLog("System", "HTTP", $"Error (Attempt {attempts}): {ex.Message}");
                            if (attempts < MaxRetryAttempts)
                                await Task.Delay(RetryDelayMs).ConfigureAwait(false);
                        }
                    }

                    if (!success)
                    {
                        IsServerConnected = false;
                        timeout_Check();
                    }

                    await Task.Delay(Sleep_Thread).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    IsServerConnected = false;
                    timeout_Check();
                    ManagerLog.Instance.AddLog("System", "HTTP", "Error: " + e);
                    await Task.Delay(Sleep_Thread).ConfigureAwait(false);
                }
            }
        }
        public void timeout_Init()
        {
            sw_Receive.Restart();
            Communication_OK_Check = true;
            IsServerConnected = true;
            ManagerLog.Instance.AddLog("System", "HTTP", "Timeout initialized, server connected");
        }

        public void timeout_Check()
        {
            if (sw_Receive.IsRunning)
            {
                long Run_sec = sw_Receive.ElapsedMilliseconds / 1000;
                if (Run_sec >= AGVData.Instance.communication_TimeOut_ms / 1000)
                {
                    Communication_OK_Check = false;
                    IsServerConnected = false;
                    ManagerLog.Instance.AddLog("System", "HTTP", "Timeout exceeded, server disconnected");
                }
            }
            else
            {
                sw_Receive.Start();
            }
        }

        private Dictionary<string, string> ParseQueryString(string query)
        {
            return query.Split('&').Select(p => p.Split('=')).ToDictionary(k => k[0], v => v.Length > 1 ? v[1] : "");
        }

        private string Make_Send_2_ACS_Set()
        {
            AGVData.Instance.Real_Command_2_ACS.agv_id = AGVData.Instance.option.AGV_ID;
            AGVData.Instance.Real_Command_2_ACS.action_0 = AGVData.Instance.state.action_0 ? "G" : "S";

            // Ánh xạ action_1
            switch (AGVData.Instance.state.action_1)
            {
                case 0: AGVData.Instance.Real_Command_2_ACS.action_1 = "S"; break;
                case 1: AGVData.Instance.Real_Command_2_ACS.action_1 = "L"; break;
                case 2: AGVData.Instance.Real_Command_2_ACS.action_1 = "R"; break;
                default: AGVData.Instance.Real_Command_2_ACS.action_1 = "U"; break;
            }

            // Ánh xạ action_2
            switch (AGVData.Instance.state.action_2)
            {
                case 0: AGVData.Instance.Real_Command_2_ACS.action_2 = "N"; break;
                case 1: AGVData.Instance.Real_Command_2_ACS.action_2 = "U"; break;
                case 2: AGVData.Instance.Real_Command_2_ACS.action_2 = "L"; break;
                default: AGVData.Instance.Real_Command_2_ACS.action_2 = "N"; break;
            }

            AGVData.Instance.Real_Command_2_ACS.battery = Battery_Percent(AGVData.Instance.state.battery);
            AGVData.Instance.Real_Command_2_ACS.tag_id = AGVData.Instance.state.tag_id;

            // Ánh xạ speed
            switch (AGVData.Instance.state.speed)
            {
                case 0: AGVData.Instance.Real_Command_2_ACS.speed = "S"; break;
                case 1: AGVData.Instance.Real_Command_2_ACS.speed = "H"; break;
                case 2: AGVData.Instance.Real_Command_2_ACS.speed = "M"; break;
                case 3: AGVData.Instance.Real_Command_2_ACS.speed = "L"; break;
                default: AGVData.Instance.Real_Command_2_ACS.speed = "S"; break;
            }

            AGVData.Instance.Real_Command_2_ACS.direction = AGVData.Instance.state.direction ? "B" : "F";
            AGVData.Instance.Real_Command_2_ACS.state_0 = AGVData.Instance.state.state_0 ? 0 : 1;  // 0=working
            AGVData.Instance.Real_Command_2_ACS.state_1 = AGVData.Instance.state.state_1 ? 1 : 0;
            AGVData.Instance.Real_Command_2_ACS.error = AGVData.Instance.state.error;
            if (AGVData.Instance.Line_Out >= AGVData.Instance.option.AGV_LineCount_MAX_Count)
            {
                AGVData.Instance.Real_Command_2_ACS.error = 8888;
            }
            return $"agv_id={AGVData.Instance.Real_Command_2_ACS.agv_id}&action_0={AGVData.Instance.Real_Command_2_ACS.action_0}&action_1={AGVData.Instance.Real_Command_2_ACS.action_1}&action_2={AGVData.Instance.Real_Command_2_ACS.action_2}&battery={AGVData.Instance.Real_Command_2_ACS.battery}&tag_id={AGVData.Instance.Real_Command_2_ACS.tag_id}&speed={AGVData.Instance.Real_Command_2_ACS.speed}&direction={AGVData.Instance.Real_Command_2_ACS.direction}&state_0={AGVData.Instance.Real_Command_2_ACS.state_0}&state_1={AGVData.Instance.Real_Command_2_ACS.state_1}&error={AGVData.Instance.Real_Command_2_ACS.error}";
        }

        public int Battery_Percent(double value)
        {
            if (AGVData.Instance.option.AGV_Battery_MIN >= value)
                return 0;
            if (AGVData.Instance.option.AGV_Battery_MAX <= value)
                return 100;
            return (int)((value - AGVData.Instance.option.AGV_Battery_MIN) / Math.Abs(AGVData.Instance.option.AGV_Battery_MAX - AGVData.Instance.option.AGV_Battery_MIN) * 100.0);
        }

        private bool Parsing_Real_Message_2_AGV(string parsing_str)
        {
            try
            {
                RealACS2AGV ACS_Data = JsonConvert.DeserializeObject<RealACS2AGV>(parsing_str);
                if (ACS_Data != null)
                {
                    if (ACS_Data.error != null)
                    {
                        ManagerLog.Instance.AddLog("System", "HTTP-Parsing", "Error: " + ACS_Data.error);
                    }
                    else
                    {
                        AGVData.Instance.Real_command_2_AGV.agv_id = ACS_Data.agv_id;
                        AGVData.Instance.Real_command_2_AGV.action_0 = ACS_Data.action_0;
                        AGVData.Instance.Real_command_2_AGV.action_1 = ACS_Data.action_1;
                        AGVData.Instance.Real_command_2_AGV.action_2 = ACS_Data.action_2;
                        AGVData.Instance.Real_command_2_AGV.tag_id = ACS_Data.tag_id;
                        AGVData.Instance.Real_command_2_AGV.speed = ACS_Data.speed;
                        AGVData.Instance.Real_command_2_AGV.front_sensor = ACS_Data.front_sensor;
                        AGVData.Instance.Real_command_2_AGV.agv_mode = ACS_Data.agv_mode;
                        AGVData.Instance.Real_command_2_AGV.acs_command = ACS_Data.acs_command;
                        switch (ACS_Data.wait_for != null ? ACS_Data.wait_for.Length : 0)
                        {
                            case 0:
                                AGVData.Instance.Real_command_2_AGV.wait_for = "0000";
                                break;

                            case 1:
                                AGVData.Instance.Real_command_2_AGV.wait_for = "000" + ACS_Data.wait_for;
                                break;
                            case 2:
                                AGVData.Instance.Real_command_2_AGV.wait_for = "00" + ACS_Data.wait_for;
                                break;
                            case 3:
                                AGVData.Instance.Real_command_2_AGV.wait_for = "0" + ACS_Data.wait_for;
                                break;
                            default:
                                AGVData.Instance.Real_command_2_AGV.wait_for = ACS_Data.wait_for;
                                break;
                        }
                        AGVData.Instance.Real_command_2_AGV.alarm = ACS_Data.alarm;
                        AGVData.Instance.Real_command_2_AGV.depot = ACS_Data.depot;
                        AGVData.Instance.Real_command_2_AGV.location = ACS_Data.location ?? new List<string>();
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                IsServerConnected = false;
                ManagerLog.Instance.AddLog("System", "HTTP-Parsing", "Error: " + ex.Message);
                return false;
            }
        }
    }
}