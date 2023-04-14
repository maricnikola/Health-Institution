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
using System.Windows.Shapes;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.ViewModels;

namespace ZdravoCorp.View
{
    /// <summary>
    /// Interaction logic for MakeAppointmentView.xaml
    /// </summary>
    public partial class MakeAppointmentView : Window
    {
        //private DoctorRepository _doctorRepository;
        public MakeAppointmentView(DoctorRepository drRepository)
        {
            MakeAppointmentViewModel MAV = new MakeAppointmentViewModel(drRepository._doctors);
            DataContext = MAV;
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
