using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.User;

namespace ZdravoCorp.Core.ViewModels.NurseViewModel
{
    public class PatientReceptionViewModel : ViewModelBase
    {
        //private ScheduleRepository _scheduleRepository;
        private PatientRepository _patientRepository;


        public PatientReceptionViewModel()
        {
            /*this._scheduleRepository = new ScheduleRepository();
            this._scheduleRepository.LoadAppointments();*/
            this._patientRepository = new PatientRepository();

        }

        //neka fuhnkcija 



    }
}
