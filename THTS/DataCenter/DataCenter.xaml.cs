
using System.Windows;


namespace THTS.DataCenter
{
    /// <summary>
    /// DataCenter.xaml 的交互逻辑
    /// </summary>
    public partial class DataCenter
    {
        public DataCenter()
        {
            InitializeComponent();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            this.flyoutSearch.IsOpen = !this.flyoutSearch.IsOpen;
        }
    }
}
