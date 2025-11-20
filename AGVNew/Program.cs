using AGVNew.Models;
using System;
using System.Windows.Forms;

namespace AGVNew
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var model = AGVData.Instance; // Dùng singleton instance
            var view = new Views.MainForm();
            var presenter = new Presenters.MainPresenter(model, view);

            Application.Run(view);
        }
    }
}