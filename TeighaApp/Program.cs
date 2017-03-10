using System;
using System.Windows.Forms;
using Ranplan.iBuilding.TeighaApp.Forms;
using Teigha.Runtime;

namespace Ranplan.iBuilding.TeighaApp
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (new Services())
            {
                Application.Run(new FormDwgReader());
            }
        }
    }
}
