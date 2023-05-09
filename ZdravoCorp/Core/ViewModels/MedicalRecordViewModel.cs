using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.MedicalRecord;

namespace ZdravoCorp.Core.ViewModels;

class MedicalRecordViewModel : ViewModelBase
{
    private readonly MedicalRecord _medicalRecord;
    public int PatientHeight => _medicalRecord.height;
    public int PatientWeight => _medicalRecord.height;
    public string PatientName => _medicalRecord.user.FullName;

    public MedicalRecordViewModel(MedicalRecord medicalRecord)
    {
        _medicalRecord = medicalRecord;
    }
}