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
using THTS.DataAccess;

namespace THTS.TestCenter
{
    /// <summary>
    /// UC_distribution.xaml 的交互逻辑
    /// </summary>
    public partial class UC_distribution9 : UserControl
    {
        public UC_distribution9()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if(this.Visibility != Visibility.Visible)
                {
                    return;
;                }
                Dictionary<string, Sensor> PositionList = (Dictionary<string, Sensor>)this.Tag;

                ChangeFont(this.A, PositionList,"A");
                ChangeFont(this.B, PositionList,"B");
                ChangeFont(this.C, PositionList,"C");
                ChangeFont(this.D, PositionList,"D");
                ChangeFont(this.E, PositionList,"E");
                ChangeFont(this.F, PositionList,"F");
                ChangeFont(this.G, PositionList,"G");
                ChangeFont(this.H, PositionList,"H");
                ChangeFont(this.O, PositionList,"O");
                ChangeFont(this.Jia, PositionList,"甲");
                ChangeFont(this.Yi, PositionList,"乙");
                ChangeFont(this.Bing, PositionList,"丙");
            }
            catch (Exception)
            {

                return;
            }
        }

        private void ChangeFont(object obj, Dictionary<string, Sensor> PositionList, string name)
        {
            if (PositionList.ContainsKey(name))
            {
                ((Label)obj).Content = name + "(" + PositionList[name].SensorID + ")";
                ((Label)obj).Foreground = Brushes.Red;
                ((Label)obj).FontStyle = FontStyles.Italic;
                ((Label)obj).FontWeight = FontWeights.Bold;
            }
        }
    }
}
