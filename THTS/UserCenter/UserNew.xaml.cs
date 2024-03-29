﻿using System;
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
using System.Windows.Shapes;

namespace THTS.UserCenter
{
    /// <summary>
    /// DeviceNew.xaml 的交互逻辑
    /// </summary>
    public partial class UserNew
    {
        DataAccess.User _newUser;
        public DataAccess.User NewUser
        {
            get { return _newUser; }
        }

        public UserNew()
        {
            InitializeComponent();
            _newUser = new DataAccess.User();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (DataAccess.UserDAO.IsExist(this.tbUserName.Text.Trim()))
            {
                MessageBox.Show("此用户名已存在，请重新输入！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!this.pbPasswordOk.Password.Equals(this.pbPassword.Password))
            {
                MessageBox.Show("两次密码不一致，请重新输入！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _newUser.UserName = this.tbUserName.Text;
            _newUser.Password = this.pbPassword.Password;
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
