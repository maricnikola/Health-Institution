using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;

namespace ZdravoCorp.Core.ViewModels.NurseViewModel
{
    public class PatientReceptionViewModel : ViewModelBase
    {
        private ScheduleRepository _scheduleRepository;
        private PatientRepository _patientRepository;


        public PatientReceptionViewModel(PatientRepository patientRepository, ScheduleRepository scheduleRepository)
        {
            /*this._scheduleRepository = new ScheduleRepository();
            this._scheduleRepository.LoadAppointments();*/
            _patientRepository = patientRepository;
            _scheduleRepository = scheduleRepository;
            _patientEmail = "nesto@gmail.com";

            SubmitButton_OnClick = new DelegateCommand(o => AddAnamnesis());

        }

        private string _patientEmail;
        public string PatientEmail
        {
            get { return _patientEmail; }
            set { _patientEmail = value; OnPropertyChanged(nameof(PatientEmail)); }
        }

        private string _alergens;
        public string Alergens
        {
            get { return _alergens; }
            set { _alergens = value; OnPropertyChanged(nameof(Alergens)); }
        }

        private string _sympthomes;
        public string Sympthomes
        {
            get { return _sympthomes; }
            set { _sympthomes = value; OnPropertyChanged(nameof(Alergens)); }
        }

        private string _deseaseHistory;
        public string DeseaseHistory
        {
            get { return _deseaseHistory; }
            set { _deseaseHistory = value; OnPropertyChanged(nameof(DeseaseHistory)); }
        }


        public ICommand SubmitButton_OnClick{ get; }

        public void AddAnamnesis()
        {
            if (_patientRepository.GetPatientByEmail(_patientEmail) == null)
            {
                //napraviti pravljenje novog patienta za to treba i novi prozor
            }
            // _scheduleRepository. neka funkcija koja ce vracati da li pacijent ima pregled zakazan u narednih 15 minuta
            // public Appointment HasPatientAppointmentInNext15(string email)
            //ovde uraditi da ubacuje anamnezu i sta vec

        }


        /*private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            
        }*/

        //neka fuhnkcija 



    }
}
