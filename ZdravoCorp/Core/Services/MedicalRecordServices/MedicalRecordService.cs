using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.MedicalRecords;
using ZdravoCorp.Core.Repositories.MedicalRecordRepo;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Services.MedicalRecordServices
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private IMedicalRecordRepository _medicalRecordRepository;

        public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository)
        {
            this._medicalRecordRepository = medicalRecordRepository;
        }

        public void ChangeRecord(string patientEmail, int newHeight, int newWeight, List<string> newDeseaseHistory)
        {
            var medicalRecordToBeChanged = this._medicalRecordRepository.GetById(patientEmail);

            if (medicalRecordToBeChanged != null)
            {
                _medicalRecordRepository.Delete(medicalRecordToBeChanged);
                medicalRecordToBeChanged.Weight = newWeight;
                medicalRecordToBeChanged.Height = newHeight;
                medicalRecordToBeChanged.DiseaseHistory = newDeseaseHistory;
                _medicalRecordRepository.Insert(medicalRecordToBeChanged);
                
            }
        }

        public bool CheckDataForChanges(int newWeight, int newHeight, List<string> newDeseaseHistory)
        {
            if (newWeight < 30 && newWeight > 250) return false;
            if (newHeight > 300 && newHeight < 50) return false;
            foreach (var desease in newDeseaseHistory)
                if (desease.Trim().Length < 5)
                    return false;
            return true;
        }

        public void Delete(MedicalRecord entity)
        {
            _medicalRecordRepository.Delete(entity);
        }

        public void Delete(string id)
        {
            MedicalRecord medicalRecordToBeDeleted = _medicalRecordRepository.GetById(id);
            if (medicalRecordToBeDeleted != null)
            {
                _medicalRecordRepository.Delete(medicalRecordToBeDeleted);
            }
        }

        public IEnumerable<MedicalRecord> GetAll()
        {
            return _medicalRecordRepository.GetAll();
        }

        public MedicalRecord? GetById(string id)
        {
            return _medicalRecordRepository.GetById(id);
        }

        public void Insert(MedicalRecord newMedicalRecord)
        {
            _medicalRecordRepository.Insert(newMedicalRecord);  
        }
    }
}
