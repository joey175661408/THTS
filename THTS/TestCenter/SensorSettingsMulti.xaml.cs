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
    public partial class SensorSettingsMulti
    {
        public SensorSettingsMulti(TestInfo Info)
        {
            InitializeComponent();

            this.DataContext = new SensorSettingsMultiViewModel(Info);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
