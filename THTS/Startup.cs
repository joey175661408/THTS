using System;
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
            UserCenter.Login login = new UserCenter.Login();
            bool? log = login.ShowDialog();
            if(log.HasValue && log.Value)
            {

                App app = new App();
                app.InitialAppResources();

                MainWindow mainWindow = new MainWindow();
                app.ShutdownMode = ShutdownMode.OnMainWindowClose;

                app.Run(mainWindow);
            }
        }
    }
}
