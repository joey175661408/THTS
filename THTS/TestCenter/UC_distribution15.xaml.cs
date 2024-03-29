﻿using System;
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
    public partial class UC_distribution15 : UserControl
    {
        public UC_distribution15()
        {
            InitializeComponent();
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.Visibility != Visibility.Visible)
                {
                    return;
                }

                if(this.Tag == null)
                {
                    return;
                }

                Dictionary<string, Sensor> PositionList = (Dictionary<string, Sensor>)this.Tag;

                ChangeFont(this.T1, PositionList, "1");
                ChangeFont(this.T2, PositionList, "2");
                ChangeFont(this.T3, PositionList, "3");
                ChangeFont(this.T4, PositionList, "4");
                ChangeFont(this.T5, PositionList, "5");
                ChangeFont(this.T6, PositionList, "6");
                ChangeFont(this.T7, PositionList, "7");
                ChangeFont(this.T8, PositionList, "8");
                ChangeFont(this.T9, PositionList, "9");
                ChangeFont(this.T10, PositionList, "10");
                ChangeFont(this.T11, PositionList, "11");
                ChangeFont(this.T12, PositionList, "12");
                ChangeFont(this.T13, PositionList, "13");
                ChangeFont(this.T14, PositionList, "14");
                ChangeFont(this.T15, PositionList, "15");
                ChangeFont(this.A, PositionList, "A");
                ChangeFont(this.B, PositionList, "B");
                ChangeFont(this.C, PositionList, "C");
                ChangeFont(this.O, PositionList, "O");
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
                ((Label)obj).Content = name + "(" + PositionList[name].Device.FactoryNo + ")";
                ((Label)obj).Foreground = Brushes.Red;
                ((Label)obj).FontStyle = FontStyles.Italic;
                ((Label)obj).FontWeight = FontWeights.Bold;
            }
        }
    }
}
