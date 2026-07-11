using CodeWalker;
using System;
using System.Windows.Forms;

namespace CodeWalker.Props
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PropForm());

            GTAFolder.UpdateSettings();
        }
    }
}
