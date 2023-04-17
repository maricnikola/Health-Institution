using System;
using System.Collections.Generic;
using ZdravoCorp.Core.Models.Appointment;
using ZdravoCorp.Core.Models.MedicalRecord;
using ZdravoCorp.Core.Models.User;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.TimeSlots;

namespace ZdravoCorp.Core.Loader;

public class LoadFunctions
{
    /*public static void LoadUsers(UserRepository userRepository)
    {
        User u1 = new User("12",1, "a", "Miso", "Misic","Nurse", "blocked");
        User u2 = new User("adadad", 1, "adfada", "Miso", "Misic", "Doctor", "blocked");
        User u3 = new User("adadad", 1, "adfada", "Miso", "Misic", "Doctor", "blocked");
        User u4 = new User("adadad", 1, "adfada", "Miso", "Misic", "Doctor", "blocked");

        userRepository.AddUser(u1);
        userRepository.AddUser(u2);
        userRepository.AddUser(u3);
        userRepository.AddUser(u4);
    }*/
    public static void LoadAppointments(ScheduleRepository schedule)
    {
        Random random = new Random();
        TimeSlot time = new TimeSlot(new DateTime(2023, 10, 10, 9, 0, 0), new DateTime(2023, 10, 10, 9, 15, 0));
        Patient p = new Patient("sreten.pejovic@gmail.com", "Seten", "Pejovic");
        MedicalRecord mr = new MedicalRecord(p, 175, 72);
        Doctor d = new Doctor("savo.oroz@gmail.com", "Savo", "Oroz", Doctor.SpecializationType.Surgeon);
        int id = random.Next(0,100000);
        Appointment appointment = new Appointment(id, time, d, mr);

        TimeSlot time1 = new TimeSlot(new DateTime(2023, 11, 10, 6, 0, 0), new DateTime(2023, 11, 10, 6, 15, 0));
        Patient p1 = new Patient("sreten.pejovic@gmail.com", "Sreten", "Pejovic");
        MedicalRecord mr1 = new MedicalRecord(p, 175, 72);
        Doctor d1 = new Doctor("savo.oroz@gmail.com", "Savo", "Oroz", Doctor.SpecializationType.Surgeon);
        int id1 = random.Next(0, 100000);
        Appointment appointment1 = new Appointment(id1, time1, d1, mr1);

        schedule.AddAppointment(appointment);
        schedule.AddAppointment(appointment1);

    }
}