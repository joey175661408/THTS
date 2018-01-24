using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
 
namespace THTS.TestCenter
{
    public partial class SensorSettings
    {
        public SensorSettings()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            SensorTest test = new SensorTest();
            test.ShowDialog();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SensorSelect_Click(object sender, RoutedEventArgs e)
        {
            DeviceCenter.DeviceCenter deviceSelect = new DeviceCenter.DeviceCenter(true);
            bool? result = deviceSelect.ShowDialog();
            if (result.HasValue && result.Value && deviceSelect.DeviceSelectList != null)
            {
                this.dataGridSensor.ItemsSource = deviceSelect.DeviceSelectList;
            }
        }

        private void SensorDelete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
