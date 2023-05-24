﻿using Autofac;
using ZdravoCorp.Core.Models.SpecialistsRefferals;
using ZdravoCorp.Core.Repositories.UsersRepo;
using ZdravoCorp.Core.Services.DoctorServices;
using ZdravoCorp.Core.Services.EquipmentServices;
using ZdravoCorp.Core.Services.HospitalRefferalServices;
using ZdravoCorp.Core.Services.InventoryServices;
using ZdravoCorp.Core.Services.MedicalRecordServices;
using ZdravoCorp.Core.Services.NurseServices;
using ZdravoCorp.Core.Services.OrderServices;
using ZdravoCorp.Core.Services.PatientServices;
using ZdravoCorp.Core.Services.RoomServices;
using ZdravoCorp.Core.Services.ScheduleServices;
using ZdravoCorp.Core.Services.SpecialistsRefferalServices;
using ZdravoCorp.Core.Services.TransferServices;
using ZdravoCorp.Core.Services.UserServices;

namespace ZdravoCorp.Core.Utilities;

public class ServicesModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DoctorService>().As<IDoctorService>();
        builder.RegisterType<EquipmentService>().As<IEquipmentService>();
        builder.RegisterType<InventoryService>().As<IInventoryService>();
        builder.RegisterType<MedicalRecordService>().As<IMedicalRecordService>();
        // builder.RegisterType<NurseService>().As<INurseRepository>();
        builder.RegisterType<PatientService>().As<IPatientService>();
        builder.RegisterType<SpecialistsRefferalService>().As<ISpecialistsRefferalService>();
        builder.RegisterType<HospitalRefferalService>().As<IHospitalRefferalService>();
        builder.RegisterType<OrderService>().As<IOrderService>();
        builder.RegisterType<RoomService>().As<IRoomService>();
        builder.RegisterType<ScheduleService>().As<IScheduleService>();
        builder.RegisterType<TransferService>().As<ITransferService>();
        builder.RegisterType<UserService>().As<IUserService>();
        builder.RegisterType<PatientService>().As<IPatientService>();
    }
}