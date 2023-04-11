using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ZdravoCorp.Core.Appointments.Model;

namespace ZdravoCorp.Core.User.Repository;

public class DoctorRepository
{
    private JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {

    };
    public List<Doctor> Doctors;

    public String _fileName;

    public void SaveDoctors()
    {
        var doctors = JsonSerializer.Serialize(this.Doctors, _serializerOptions);
        File.WriteAllText(this._fileName, doctors);
    }
    public void LoadDoctors()
    {
        string text = File.ReadAllText(_fileName);
        var doctors = JsonSerializer.Deserialize<List<Doctor>>(text);
        doctors.ForEach(doctor => Doctors.Add(doctor));
    }
    public Doctor GetDoctorById(int id)
    {
        foreach(Doctor doctor in Doctors)
        {
            if (doctor.Id == id)
            {
                return doctor;
            }
        }
        return null;
    }
    
}
