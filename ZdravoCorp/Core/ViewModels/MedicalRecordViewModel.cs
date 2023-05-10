using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Core.Models.MedicalRecord;

namespace ZdravoCorp.Core.ViewModels;

class MedicalRecordViewModel:ViewModelBase
{
    private readonly MedicalRecord _medicalRecord;
    public int PatientHeight => _medicalRecord.height;
    public int PatientWeight => _medicalRecord.weight;
    public string PatientName => _medicalRecord.user.FullName;
	public string PatientDeseaseHistory => _medicalRecord.DiseaseHistoryToString();
	
	public ICommand SaveCommand { get; }
    public MedicalRecordViewModel(MedicalRecord medicalRecord)
    {
        _medicalRecord = medicalRecord;
		_height = _medicalRecord.height;
		_weight = _medicalRecord.weight;
		_diseaseHistory = "";
    }

	private int _height;
	public int Height
	{
		get
		{
			return _height;
		}
		set
		{
			_height = value;
			OnPropertyChanged(nameof(Height));
		}
	}

	private int _weight;
	public int Weight
	{
		get
		{
			return _weight;
		}
		set
		{
			_weight = value;
			OnPropertyChanged(nameof(Weight));
		}
	}

	private string _diseaseHistory;
	public string DiseaseHistory
	{
		get
		{
			return _diseaseHistory;
		}
		set
		{
			_diseaseHistory = value;
			OnPropertyChanged(nameof(DiseaseHistory));
		}
	}
}


