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

        TimeSlot time2 = new TimeSlot(new DateTime(2023, 8, 11, 6, 0, 0), new DateTime(2023, 8, 11, 6, 15, 0));
        Patient p2 = new Patient("sreten.pejovic@gmail.com", "Sreten", "Pejovic");
        MedicalRecord mr2 = new MedicalRecord(p, 175, 72);
        Doctor d2 = new Doctor("savo.oroz@gmail.com", "Savo", "Oroz", Doctor.SpecializationType.Surgeon);
        int id2 = random.Next(0, 100000);
        Appointment appointment2 = new Appointment(id2, time2, d2, mr2);

        TimeSlot time3 = new TimeSlot(new DateTime(2023, 6, 23, 6, 0, 0), new DateTime(2023, 6, 23, 6, 15, 0));
        Patient p3 = new Patient("sreten.pejovic@gmail.com", "Sreten", "Pejovic");
        MedicalRecord mr3 = new MedicalRecord(p, 175, 72);
        Doctor d3 = new Doctor("savo.oroz@gmail.com", "Savo", "Oroz", Doctor.SpecializationType.Surgeon);
        int id3 = random.Next(0, 100000);
        Appointment appointment3 = new Appointment(id3, time3, d3, mr3);

        TimeSlot time4 = new TimeSlot(new DateTime(2023, 12, 12, 8, 0, 0), new DateTime(2023, 12, 12, 8, 15, 0));
        Patient p4 = new Patient("sreten.pejovic@gmail.com", "Sreten", "Pejovic");
        MedicalRecord mr4 = new MedicalRecord(p, 175, 72);
        Doctor d4 = new Doctor("savo.oroz@gmail.com", "Savo", "Oroz", Doctor.SpecializationType.Surgeon);
        int id4 = random.Next(0, 100000);
        Appointment appointment4 = new Appointment(id4, time4, d4, mr4);

        schedule.AddAppointment(appointment);
        schedule.AddAppointment(appointment1);
        schedule.AddAppointment(appointment2);
        schedule.AddAppointment(appointment3);
        schedule.AddAppointment(appointment4);



    }
}