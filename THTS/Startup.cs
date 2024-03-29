﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace THTS
{
    public class Startup
    {

        [STAThread]
        static void Main(params string[] args)
        {

            App app = new App();
            app.InitialAppResources();

            MainWindow mainWindow = new MainWindow();
            app.ShutdownMode = ShutdownMode.OnMainWindowClose;

            UserCenter.Login login = new UserCenter.Login();
            bool? log = login.ShowDialog();

            if (login.testMode)
            {
                mainWindow.SetDebugMode();
            }

            if (log.HasValue && log.Value)
            {
                app.Run(mainWindow);
            }
        }
    }
}
