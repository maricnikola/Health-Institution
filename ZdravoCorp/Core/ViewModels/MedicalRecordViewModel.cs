using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.Commands;
using ZdravoCorp.Core.Models.MedicalRecord;
using ZdravoCorp.Core.Repositories.MedicalRecord;

namespace ZdravoCorp.Core.ViewModels;

class MedicalRecordViewModel:ViewModelBase
{
    private readonly MedicalRecord _medicalRecord;
    public int PatientHeight => _medicalRecord.height;
    public int PatientWeight => _medicalRecord.weight;
    public string PatientName => _medicalRecord.user.FullName;
	public string PatientDeseaseHistory => _medicalRecord.DiseaseHistoryToString();
	private MedicalRecordRepository _medicalRecordRepository;
	
	public ICommand SaveCommand { get; }
    public MedicalRecordViewModel(MedicalRecord medicalRecord,MedicalRecordRepository medicalRecordRepository)
    {
		_medicalRecordRepository = medicalRecordRepository;
        _medicalRecord = medicalRecord;
		_height = _medicalRecord.height;
		_weight = _medicalRecord.weight;
		_diseaseHistory = medicalRecord.DiseaseHistoryToString();
		SaveCommand = new DelegateCommand(o => SaveChangesMedicalRecord());
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

	public void SaveChangesMedicalRecord()
	{

        try
        {
			int height = Height;
			int weight = Weight;
			List<String> diseasHistory = DiseaseHistory.Trim().Split(",").ToList();
			bool checkData = _medicalRecordRepository.CheckDataForChanges(weight,height,diseasHistory);
			if (checkData)
			{
				_medicalRecordRepository.ChangeRecord(_medicalRecord.user.Email, height, weight, diseasHistory);

			}else MessageBox.Show("Invalid Medical record", "Error", MessageBoxButton.OK);
        }
        catch (Exception)
        {
            MessageBox.Show("Invalid Medical record", "Error", MessageBoxButton.OK);
        }


    }
}


