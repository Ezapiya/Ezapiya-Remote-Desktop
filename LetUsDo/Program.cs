using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LetUsDo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frm_Connect_to_server());
          //  Application.Run(new frm_speed_test());
            //  Application.Run(new frm_Drawing());

        }
    }
}
