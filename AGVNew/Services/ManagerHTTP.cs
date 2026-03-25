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
		public const int Sleep_Thread = 1000;
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

		// Per-AGV: tracking last sent message và thời gian gửi
		private readonly Dictionary<string, string> _lastSentMessages = new Dictionary<string, string>();
		private readonly Dictionary<string, DateTime> _lastSentTimes = new Dictionary<string, DateTime>();
		private readonly object _lock = new object();
		private const int HeartbeatIntervalSec = 30; // Gửi heartbeat mỗi 30s dù không thay đổi

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
				try
				{
					Thread.Interrupt();
					Thread.Join(2000);
				}
				catch { }
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
					foreach (var kvp in AGVData.All)
					{
						string agvKey = kvp.Key;
						AGVData agvData = kvp.Value;

						// Đọc PLC cho AGV này
						MitsubishiPLC.Instance.UpdateStateFromPLC(agvData);

						string endpoint = "/agv/report/";
						string real_send_msg = Make_Send_2_ACS_Set(agvData);

						// Kiểm tra có thay đổi hoặc đến lúc heartbeat
						bool shouldSend = false;
						lock (_lock)
						{
							string lastMsg;
							_lastSentMessages.TryGetValue(agvKey, out lastMsg);
							DateTime lastTime;
							_lastSentTimes.TryGetValue(agvKey, out lastTime);
							bool heartbeatDue = (DateTime.Now - lastTime).TotalSeconds >= HeartbeatIntervalSec;

							if (real_send_msg != lastMsg || heartbeatDue)
							{
								_lastSentMessages[agvKey] = real_send_msg;
								_lastSentTimes[agvKey] = DateTime.Now;
								shouldSend = true;
							}
						}

						if (shouldSend)
						{
							bool success = false;
							using (var content = new FormUrlEncodedContent(ParseQueryString(real_send_msg)))
							{
								for (int attempts = 1; attempts <= MaxRetryAttempts && !success; attempts++)
								{
									try
									{
										ManagerLog.Instance.AddLog("System", "HTTP-Send", $"[{agvKey}] Gửi: {real_send_msg}");
										var response = await _httpClient.PostAsync(endpoint, content).ConfigureAwait(false);

										if (response.IsSuccessStatusCode)
										{
											string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
											ManagerLog.Instance.AddLog("System", "HTTP-Receive", $"[{agvKey}] {json}");
											Parsing_Real_Message_2_AGV(json, agvData);
											timeout_Init();
											success = true;
											IsServerConnected = true;
										}
										else
										{
											http_result = (int)response.StatusCode;
											ManagerLog.Instance.AddLog("System", "HTTP-Result", $"[{agvKey}] Status: {http_result}");
											if (attempts < MaxRetryAttempts)
												await Task.Delay(RetryDelayMs).ConfigureAwait(false);
										}
									}
									catch (Exception ex)
									{
										ManagerLog.Instance.AddLog("System", "HTTP", $"[{agvKey}] Error (Attempt {attempts}): {ex.Message}");
										if (attempts < MaxRetryAttempts)
											await Task.Delay(RetryDelayMs).ConfigureAwait(false);
									}
								}
							}

							if (!success)
							{
								IsServerConnected = false;
								timeout_Check();
							}
						}
						// Không thay đổi và chưa đến heartbeat → bỏ qua
					}

					await Task.Delay(Sleep_Thread).ConfigureAwait(false);
				}
				catch (Exception e)
				{
					IsServerConnected = false;
					timeout_Check();
					ManagerLog.Instance.AddLog("System", "HTTP", "Error: " + e.Message);
					await Task.Delay(Sleep_Thread).ConfigureAwait(false);
				}
			}
		}

		public void timeout_Init()
		{
			sw_Receive.Restart();
			Communication_OK_Check = true;
			IsServerConnected = true;
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
			return query.Split('&')
						.Select(p => p.Split('='))
						.ToDictionary(k => k[0], v => v.Length > 1 ? v[1] : "");
		}

		private string Make_Send_2_ACS_Set(AGVData agvData)
		{
			agvData.Real_Command_2_ACS.agv_id = agvData.option.AGV_ID;
			agvData.Real_Command_2_ACS.action_0 = agvData.state.action_0 ? "G" : "S";

			switch (agvData.state.action_1)
			{
				case 0: agvData.Real_Command_2_ACS.action_1 = "S"; break;
				case 1: agvData.Real_Command_2_ACS.action_1 = "L"; break;
				case 2: agvData.Real_Command_2_ACS.action_1 = "R"; break;
				default: agvData.Real_Command_2_ACS.action_1 = "U"; break;
			}

			switch (agvData.state.action_2)
			{
				case 0: agvData.Real_Command_2_ACS.action_2 = "N"; break;
				case 1: agvData.Real_Command_2_ACS.action_2 = "U"; break;
				case 2: agvData.Real_Command_2_ACS.action_2 = "L"; break;
				default: agvData.Real_Command_2_ACS.action_2 = "N"; break;
			}

			agvData.Real_Command_2_ACS.battery = Battery_Percent(agvData.state.battery, agvData);
			agvData.Real_Command_2_ACS.tag_id = agvData.state.tag_id;

			switch (agvData.state.speed)
			{
				case 0: agvData.Real_Command_2_ACS.speed = "S"; break;
				case 1: agvData.Real_Command_2_ACS.speed = "H"; break;
				case 2: agvData.Real_Command_2_ACS.speed = "M"; break;
				case 3: agvData.Real_Command_2_ACS.speed = "L"; break;
				default: agvData.Real_Command_2_ACS.speed = "S"; break;
			}

			agvData.Real_Command_2_ACS.direction = agvData.state.direction ? "B" : "F";
			agvData.Real_Command_2_ACS.state_0 = agvData.state.state_0 ? 0 : 1;
			agvData.Real_Command_2_ACS.state_1 = agvData.state.state_1 ? 1 : 0;
			agvData.Real_Command_2_ACS.error = agvData.state.error;

			if (agvData.Line_Out >= agvData.option.AGV_LineCount_MAX_Count)
			{
				agvData.Real_Command_2_ACS.error = 8888;
			}

			return $"agv_id={agvData.Real_Command_2_ACS.agv_id}&action_0={agvData.Real_Command_2_ACS.action_0}&action_1={agvData.Real_Command_2_ACS.action_1}&action_2={agvData.Real_Command_2_ACS.action_2}&battery={agvData.Real_Command_2_ACS.battery}&tag_id={agvData.Real_Command_2_ACS.tag_id}&speed={agvData.Real_Command_2_ACS.speed}&direction={agvData.Real_Command_2_ACS.direction}&state_0={agvData.Real_Command_2_ACS.state_0}&state_1={agvData.Real_Command_2_ACS.state_1}&error={agvData.Real_Command_2_ACS.error}";
		}

		public int Battery_Percent(double value, AGVData agvData)
		{
			if (agvData.option.AGV_Battery_MIN >= value)
				return 0;
			if (agvData.option.AGV_Battery_MAX <= value)
				return 100;
			return (int)((value - agvData.option.AGV_Battery_MIN) / Math.Abs(agvData.option.AGV_Battery_MAX - agvData.option.AGV_Battery_MIN) * 100.0);
		}

		public int Battery_Percent(double value)
		{
			return Battery_Percent(value, AGVData.Instance);
		}

		private bool Parsing_Real_Message_2_AGV(string parsing_str, AGVData agvData)
		{
			try
			{
				RealACS2AGV ACS_Data = JsonConvert.DeserializeObject<RealACS2AGV>(parsing_str);
				if (ACS_Data != null)
				{
					if (ACS_Data.error != null)
					{
						ManagerLog.Instance.AddLog("System", "HTTP-Parsing", $"[{agvData.option.AGV_Key}] Error: " + ACS_Data.error);
					}
					else
					{
						agvData.Real_command_2_AGV.agv_id = ACS_Data.agv_id;
						agvData.Real_command_2_AGV.action_0 = ACS_Data.action_0;
						agvData.Real_command_2_AGV.action_1 = ACS_Data.action_1;
						agvData.Real_command_2_AGV.action_2 = ACS_Data.action_2;
						agvData.Real_command_2_AGV.tag_id = ACS_Data.tag_id;
						agvData.Real_command_2_AGV.speed = ACS_Data.speed;
						agvData.Real_command_2_AGV.front_sensor = ACS_Data.front_sensor;
						agvData.Real_command_2_AGV.agv_mode = ACS_Data.agv_mode;
						agvData.Real_command_2_AGV.acs_command = ACS_Data.acs_command;

						string waitFor = ACS_Data.wait_for ?? "";
						switch (waitFor.Length)
						{
							case 0: agvData.Real_command_2_AGV.wait_for = "0000"; break;
							case 1: agvData.Real_command_2_AGV.wait_for = "000" + waitFor; break;
							case 2: agvData.Real_command_2_AGV.wait_for = "00" + waitFor; break;
							case 3: agvData.Real_command_2_AGV.wait_for = "0" + waitFor; break;
							default: agvData.Real_command_2_AGV.wait_for = waitFor; break;
						}

						agvData.Real_command_2_AGV.alarm = ACS_Data.alarm;
						agvData.Real_command_2_AGV.depot = ACS_Data.depot;
						agvData.Real_command_2_AGV.location = ACS_Data.location ?? new List<string>();
					}
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				IsServerConnected = false;
				ManagerLog.Instance.AddLog("System", "HTTP-Parsing", $"[{agvData.option.AGV_Key}] Error: " + ex.Message);
				return false;
			}
		}
	}
}