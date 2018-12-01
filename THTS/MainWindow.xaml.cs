using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro;
using THTS.TestCenter;

namespace THTS
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设置调试系统
        /// </summary>
        public void SetDebugMode()
        {
            this.HelpTile.Visibility = Visibility.Collapsed;
            this.DebugTile.Visibility = Visibility.Visible;
        }

        private void TestCenter_Click(object sender, RoutedEventArgs e)
        {
            TestCenter.TestCenter testCenter = new TestCenter.TestCenter();
            testCenter.ShowDialog();
        }

        private void DeviceCenter_Click(object sender, RoutedEventArgs e)
        {
            DeviceCenter.DeviceCenter deviceCenter = new DeviceCenter.DeviceCenter(false);
            deviceCenter.ShowDialog();
        }

        private void DataCenter_Click(object sender, RoutedEventArgs e)
        {
            DataCenter.DataCenter dataCenter = new DataCenter.DataCenter();
            dataCenter.ShowDialog();
        }

        private void UserCenter_Click(object sender, RoutedEventArgs e)
        {
            UserCenter.UserCenter userCenter = new UserCenter.UserCenter();
            userCenter.ShowDialog();
        }

        private void SettingCenter_Click(object sender, RoutedEventArgs e)
        {
            SettingCenter.SettingCenter settingCenter = new SettingCenter.SettingCenter();
            settingCenter.ShowDialog();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Environment.CurrentDirectory + "/help/温湿度场自动测试系统用户手册.pdf");
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            DebugCenter.TiredTestView tired = new DebugCenter.TiredTestView();
            tired.ShowDialog();
        }

    }
}
