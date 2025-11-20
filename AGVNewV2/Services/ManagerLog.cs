using System;
using AGVNew.Views;

namespace AGVNew.Services
{
    public class ManagerLog
    {
        private static ManagerLog _instance;
        private static readonly object _lock = new object();
        private MainForm _view;

        public static ManagerLog Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ManagerLog();
                    }
                    return _instance;
                }
            }
        }


        public MainForm View
        {
            get
            {
                lock (_lock) { return _view; }
            }
            set
            {
                lock (_lock) { _view = value; }
            }
        }

        private ManagerLog() { }

        public void AddLog(string category, string type, string message)
        {
            string log = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{category}] [{type}] {message}";
            Console.WriteLine(log); // Log ra console để debug

            MainForm view;
            lock (_lock)
            {
                view = _view;
            }

            if (view == null || view.IsDisposed)
            {
                Console.WriteLine("ManagerLog: View is null or disposed, cannot append to TextBox");
                return;
            }

            try
            {
                view.AppendLog(log);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ManagerLog: Error appending log: {ex.Message}");
            }
        }
    }
}