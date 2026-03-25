using System;
using System.Threading;
using AGVNew.Models;

namespace AGVNew.Services
{
    public class MockPLCService
    {
        private static MockPLCService _instance;
        private readonly Random _random;
        private Thread _mockThread;
        public bool IsRunning { get; private set; }

        public static MockPLCService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MockPLCService();
                }
                return _instance;
            }
        }

        private MockPLCService()
        {
            _random = new Random();
        }

        public void StartMock()
        {
            if (_mockThread == null || !_mockThread.IsAlive)
            {
                ManagerLog.Instance.AddLog("System", "Mock", "Starting mock data simulation for all AGVs");
                IsRunning = true;
                _mockThread = new Thread(MockThreadRun) { IsBackground = true };
                _mockThread.Start();
            }
        }

        public void StopMock()
        {
            IsRunning = false;
            if (_mockThread != null && _mockThread.IsAlive)
            {
                try
                {
                    _mockThread.Interrupt();
                    _mockThread.Join(1000); // Timeout 1s
                }
                catch { }
                ManagerLog.Instance.AddLog("System", "Mock", "Stopped mock data simulation");
            }
            _mockThread = null;
        }

        private void MockThreadRun()
        {
            while (IsRunning)
            {
                try
                {
                    // Mock data cho TẤT CẢ AGV instances
                    foreach (var kvp in AGVData.All)
                    {
                        string agvKey = kvp.Key;
                        AGVData agvData = kvp.Value;

                        lock (AGVData._lock)
                        {
                            var state = new AGVData.State();
                            state.action_0 = _random.Next(0, 2) == 1;
                            state.battery = _random.Next(0, 101);
                            state.tag_id = agvKey == "AGV1" ? "00011" : "00022";
                            state.direction = _random.Next(0, 2) == 1;
                            state.state_0 = _random.Next(0, 2) == 1;
                            state.state_1 = false;
                            state.error = _random.Next(0, 4);
                            state.speed = _random.Next(0, 4);
                            state.action_1 = _random.Next(0, 3);
                            state.action_2 = _random.Next(0, 3);
                            state.mode = _random.Next(0, 2) == 1;

                            agvData.state = state;

                            ManagerLog.Instance.AddLog("System", "Mock", $"[{agvKey}] Mock data: battery={state.battery}%, error={state.error}, tag_id={state.tag_id}");
                        }
                    }
                    Thread.Sleep(500);
                }
                catch (ThreadInterruptedException)
                {
                    ManagerLog.Instance.AddLog("System", "Mock", "Mock thread interrupted");
                    break;
                }
                catch (Exception ex)
                {
                    ManagerLog.Instance.AddLog("System", "Mock", "MockThreadRun error: " + ex.Message);
                }
            }
        }
    }
}