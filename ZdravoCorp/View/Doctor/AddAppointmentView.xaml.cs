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
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.ViewModels;

namespace ZdravoCorp.View.DoctorView
{
    /// <summary>
    /// Interaction logic for AddAppointmentView.xaml
    /// </summary>
    public partial class AddAppointmentView : Window
    {
        public AddAppointmentView()
        {
            
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
