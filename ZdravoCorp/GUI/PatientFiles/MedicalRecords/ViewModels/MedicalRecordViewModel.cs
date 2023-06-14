using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.Core.PatientFiles.MedicalRecords.Models;
using ZdravoCorp.Core.PatientFiles.MedicalRecords.Services;
using ZdravoCorp.GUI.Main;

namespace ZdravoCorp.GUI.PatientFiles.MedicalRecords.ViewModels;

internal class MedicalRecordViewModel : ViewModelBase
{
    private readonly MedicalRecord _medicalRecord;

    private string _diseaseHistory;

    private int _height;
    private readonly IMedicalRecordService _medicalRecordService;

    private int _weight;

    public MedicalRecordViewModel(MedicalRecord medicalRecord, IMedicalRecordService medicalRecordService)
    {
        _medicalRecordService = medicalRecordService;
        _medicalRecord = medicalRecord;
        _height = _medicalRecord.Height;
        _weight = _medicalRecord.Weight;
        _diseaseHistory = medicalRecord.DiseaseHistoryToString();
        SaveCommand = new DelegateCommand(o => SaveChangesMedicalRecord());
        CloseCommand = new DelegateCommand(o => CloseWindow());
    }

    public int PatientHeight => _medicalRecord.Height;
    public int PatientWeight => _medicalRecord.Weight;
    public string PatientName => _medicalRecord.Patient.FullName;
    public string PatientDeseaseHistory => _medicalRecord.DiseaseHistoryToString();

    public ICommand SaveCommand { get; }
    public ICommand CloseCommand { get; }

    public int Height
    {
        get => _height;
        set
        {
            _height = value;
            OnPropertyChanged();
        }
    }

    public int Weight
    {
        get => _weight;
        set
        {
            _weight = value;
            OnPropertyChanged();
        }
    }

    public string DiseaseHistory
    {
        get => _diseaseHistory;
        set
        {
            _diseaseHistory = value;
            OnPropertyChanged();
        }
    }

    private void CloseWindow()
    {
        var activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        activeWindow?.Close();
    }

    public void SaveChangesMedicalRecord()
    {
        try
        {
            var height = Height;
            var weight = Weight;
            var diseasHistory = DiseaseHistory.Trim().Split(",").ToList();
            var checkData = _medicalRecordService.CheckDataForChanges(weight, height, diseasHistory);
            if (checkData)
            {
                _medicalRecordService.ChangeRecord(_medicalRecord.Patient.Email, height, weight, diseasHistory);
                CloseWindow();
            }
            else
            {
                MessageBox.Show("Invalid Medical record", "Error", MessageBoxButton.OK);
            }
        }
        catch (Exception)
        {
            MessageBox.Show("Invalid Medical record", "Error", MessageBoxButton.OK);
        }
    }
}