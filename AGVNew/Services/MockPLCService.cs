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
                ManagerLog.Instance.AddLog("System", "Mock", "Starting mock data simulation");
                IsRunning = true;
                _mockThread = new Thread(MockThreadRun) { IsBackground = true };
                _mockThread.Start();
            }
        }

        public void StopMock()
        {
            if (_mockThread != null && _mockThread.IsAlive)
            {
                IsRunning = false;
                _mockThread.Interrupt();
                ManagerLog.Instance.AddLog("System", "Mock", "Stopped mock data simulation");
            }
        }

        private void MockThreadRun()
        {
            while (IsRunning)
            {
                try
                {
                    lock (AGVData._lock) // Dùng lock từ AGVData
                    {
                        var state = new AGVData.State();
                       // state.agv_id = 1;
                        state.action_0 = _random.Next(0, 2) == 1;
                      //  state.action_1 = _random.Next(0, 2) == 1;
                   
                       // state.action_2 = _random.Next(0, 2) == 1;
                       
                        state.battery = _random.Next(0, 101);
                        state.tag_id = "00011";
                      //  state.speed_0 = _random.Next(0, 2) == 1;
                      //  state.speed_1 = _random.Next(0, 2) == 1;
                        state.direction = _random.Next(0, 2) == 1;
                        state.state_0 = _random.Next(0, 2) == 1;
                        state.state_1 = false;
                        state.error = _random.Next(0, 4);
                     //   state.auto_mode = _random.Next(0, 2) == 1;
                     //   state.manual_mode = !state.auto_mode;
                    //    state.check_connect = true;

                        AGVData.Instance.state = state;

                        ManagerLog.Instance.AddLog("System", "Mock", $"Mock data generated: agv_id={state.agv_id}, battery={state.battery}%, error={state.error}, tag_id={state.tag_id}");
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