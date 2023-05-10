using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Repositories.Schedule;

namespace ZdravoCorp.Core.Repositories.MedicalRecord;

using System.Windows;
using ZdravoCorp.Core.Models.MedicalRecord;

public class MedicalRecordRepository
{
    private List<MedicalRecord> _records { get; set; }

    public MedicalRecordRepository(List<Appointment> appointments)
    {
        _records = new List<MedicalRecord>();
        foreach(Appointment ap in appointments)
        {
            MedicalRecord mr = ap.MedicalRecord;
            _records.Add(mr);
        }
            
    }
    public void AddRecord(Models.MedicalRecord.MedicalRecord? newMedicalRecord)
    {
        _records.Add(newMedicalRecord);
    }
    public MedicalRecord? GetById(string id)
    {   
        MedicalRecord mr = _records.FirstOrDefault(record => record.user.Email == id);
        //foreach(MedicalRecord m in _records)
        //{
        //    MessageBox.Show(m.user.Email, "Error", MessageBoxButton.OK);
        //}
        return mr;
    }
    public void RemoveById(int id)
    {

    }
}