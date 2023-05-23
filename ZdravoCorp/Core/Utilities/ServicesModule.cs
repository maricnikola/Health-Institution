﻿using Autofac;
using ZdravoCorp.Core.Repositories.UsersRepo;
using ZdravoCorp.Core.Services.DoctorServices;
using ZdravoCorp.Core.Services.EquipmentServices;
using ZdravoCorp.Core.Services.InventoryServices;
using ZdravoCorp.Core.Services.MedicalRecordServices;
using ZdravoCorp.Core.Services.NurseServices;
using ZdravoCorp.Core.Services.OrderServices;
using ZdravoCorp.Core.Services.RoomServices;
using ZdravoCorp.Core.Services.ScheduleServices;
using ZdravoCorp.Core.Services.TransferServices;
using ZdravoCorp.Core.Services.UserServices;

namespace ZdravoCorp.Core.Utilities;

public class ServicesModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DoctorService>().As<IDoctorService>().SingleInstance();
        builder.RegisterType<EquipmentService>().As<IEquipmentService>().SingleInstance();
        builder.RegisterType<InventoryService>().As<IInventoryService>().SingleInstance();
        builder.RegisterType<MedicalRecordService>().As<IMedicalRecordService>().SingleInstance();
       // builder.RegisterType<NurseService>().As<INurseRepository>().SingleInstance();
        builder.RegisterType<OrderService>().As<IOrderService>().SingleInstance().SingleInstance();
        builder.RegisterType<RoomService>().As<IRoomService>().SingleInstance();
        builder.RegisterType<ScheduleService>().As<IScheduleService>().SingleInstance();
        builder.RegisterType<TransferService>().As<ITransferService>().SingleInstance();
        builder.RegisterType<UserService>().As<IUserService>().SingleInstance();
    }
}