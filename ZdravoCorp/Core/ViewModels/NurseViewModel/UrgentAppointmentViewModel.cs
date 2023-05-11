using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.Repositories.MedicalRecord;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.Appointment;

namespace ZdravoCorp.Core.ViewModels.NurseViewModel
{
    public class UrgentAppointmentViewModel : ViewModelBase
    {
        private MedicalRecordRepository _medicalRecordRepository;
        private DoctorRepository _doctorRepository;
        private ScheduleRepository _scheduleRepository;

        public ICommand FindUrgentAppointmentCommand { get; set; }

        private string _patientEmail;
        public string PatientEmail
        {
            get { return _patientEmail; }
            set { _patientEmail = value; }
        }

        private Doctor.SpecializationType _specializationType;
        public Doctor.SpecializationType SpecializationType
        {
            get { return _specializationType; }
            set { _specializationType = value; }
        }

        public UrgentAppointmentViewModel(MedicalRecordRepository medicalRecordRepository,  ScheduleRepository scheduleRepository, DoctorRepository doctorRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _doctorRepository = doctorRepository;
            _scheduleRepository = scheduleRepository;
            FindUrgentAppointmentCommand = new DelegateCommand(o => FindUrgentAppointment());
        }

        public void FindUrgentAppointment()
        {
            DateTime now = DateTime.Now;
            List<Doctor> suitableDoctors = _doctorRepository.GetAllSpecialized(_specializationType);
            if (suitableDoctors.Count == 0)
            {
                //napraviti pop up obavestenja da nema doktora za ovaj posao
            }
            else
            {
                List<List<Appointment>> allDoctorsAppointments = new List<List<Appointment>> { };
                foreach(Doctor suitableDoctor in suitableDoctors)
                {
                    allDoctorsAppointments.Add(_scheduleRepository.GetDoctorAppointments(suitableDoctor));
                }



            }
        }


    }
}
