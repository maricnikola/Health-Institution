using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Repositories.MedicalRecord;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;

namespace ZdravoCorp.Core.ViewModels.NurseViewModel
{
    public class UrgentAppointmentViewModel
    {
        private MedicalRecordRepository _medicalRecordRepository;
        private DoctorRepository _doctorRepository;
        private ScheduleRepository _scheduleRepository;

        public UrgentAppointmentViewModel(MedicalRecordRepository medicalRecordRepository,  ScheduleRepository scheduleRepository, DoctorRepository doctorRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _doctorRepository = doctorRepository;
            _scheduleRepository = scheduleRepository;
        }
    }
}
