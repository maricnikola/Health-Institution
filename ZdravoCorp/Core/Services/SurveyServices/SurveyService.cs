﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.Surveys;
using ZdravoCorp.Core.Repositories.SurveysRepo;

namespace ZdravoCorp.Core.Services.ServayServices;

public class SurveyService : ISurveyService
{
    private readonly IDoctorSurveyRepository _doctorSurveyRepository;
    private readonly IHospitalSurveyRepository _hospitalSurveyRepository;

    public SurveyService(IDoctorSurveyRepository doctorSurveyRepository, IHospitalSurveyRepository hospitalSurveyRepository)
    {
        _doctorSurveyRepository = doctorSurveyRepository;
        _hospitalSurveyRepository = hospitalSurveyRepository;
    }

    public IEnumerable<DoctorSurvey>? GetAllDoctorSurveys()
    {
        return _doctorSurveyRepository.GetAll() as List<DoctorSurvey>;
    }

    public IEnumerable<HospitalSurvey>? GetAllHospitalSurveys()
    {
        return _hospitalSurveyRepository.GetAll() as List<HospitalSurvey>;
    }

    public DoctorSurvey? FindExistingDoctorSurvey(string doctorEmail, string patientEmail)
    {
        return GetAllDoctorSurveys().FirstOrDefault(survey => survey.DoctorEmail == doctorEmail && survey.PatientEmail == patientEmail);
    }

    public void AddDoctorSuvay(DoctorSurveyDTO doctorSurvey)
    {
        var survey = FindExistingDoctorSurvey(doctorSurvey.DoctorEmail, doctorSurvey.PatientEmail);
        if (survey != null)
        {
            _doctorSurveyRepository.Delete(survey);
            _doctorSurveyRepository.Insert(new DoctorSurvey(doctorSurvey));
        }
        else 
            _doctorSurveyRepository.Insert(new DoctorSurvey(doctorSurvey));

    }

    public void AddHospitalSurvey(HospitalSurveyDTO hospitalSurvey)
    {
        var survey = FindHospitalSurveyForPatient(hospitalSurvey.PatientEmail);
        if (survey != null)
        {
            _hospitalSurveyRepository.Delete(survey);
            _hospitalSurveyRepository.Insert(new HospitalSurvey(hospitalSurvey));
        }
        else
            _hospitalSurveyRepository.Insert(new HospitalSurvey(hospitalSurvey));
    }

    public List<DoctorSurvey> FindSurveysForDoctor(string doctorEmail)
    {
        return GetAllDoctorSurveys().Where(survey => survey.DoctorEmail == doctorEmail).ToList();
    }

    public double FindAverageGradeForDoctor(string doctorEmail)
    {
        var doctorsSturvays = FindSurveysForDoctor(doctorEmail);
        double count = doctorsSturvays.Count;
        if (count == 0)
            return 5;
        double sum = doctorsSturvays.Sum(survey => survey.Grade);
        double avg = sum / count;
        return avg;
    }

    public HospitalSurvey? FindHospitalSurveyForPatient(string patientEmail)
    {
        return GetAllHospitalSurveys()!.FirstOrDefault(survey => survey.PatientEmail == patientEmail);
    }
}