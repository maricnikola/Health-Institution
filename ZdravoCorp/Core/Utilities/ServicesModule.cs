using Autofac;
using ZdravoCorp.Core.Repositories.RenovationRepo;
using ZdravoCorp.Core.Repositories.UsersRepo;
using ZdravoCorp.Core.Services.DoctorServices;
using ZdravoCorp.Core.Services.EquipmentServices;
using ZdravoCorp.Core.Services.HospitalRefferalServices;
using ZdravoCorp.Core.Services.InventoryServices;
using ZdravoCorp.Core.Services.MedicalRecordServices;
using ZdravoCorp.Core.Services.MedicamentServices;
using ZdravoCorp.Core.Services.NotificationServices;
using ZdravoCorp.Core.Services.NurseServices;
using ZdravoCorp.Core.Services.OrderServices;
using ZdravoCorp.Core.Services.PatientServices;
using ZdravoCorp.Core.Services.RenovationServices;
using ZdravoCorp.Core.Services.RoomServices;
using ZdravoCorp.Core.Services.ScheduleServices;
using ZdravoCorp.Core.Services.ServayServices;
using ZdravoCorp.Core.Services.SpecialistsRefferalServices;
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
        builder.RegisterType<PatientService>().As<IPatientService>().SingleInstance();
        builder.RegisterType<SpecialistsRefferalService>().As<ISpecialistsRefferalService>().SingleInstance();
        builder.RegisterType<HospitalRefferalService>().As<IHospitalRefferalService>().SingleInstance();
        builder.RegisterType<MedicamentService>().As<IMedicamentService>().SingleInstance();
        builder.RegisterType<NurseService>().As<INurseService>().SingleInstance();
        builder.RegisterType<OrderService>().As<IOrderService>().SingleInstance();
        builder.RegisterType<RoomService>().As<IRoomService>().SingleInstance();
        builder.RegisterType<ScheduleService>().As<IScheduleService>().SingleInstance();
        builder.RegisterType<TransferService>().As<ITransferService>().SingleInstance();
        builder.RegisterType<UserService>().As<IUserService>().SingleInstance();
        builder.RegisterType<PatientService>().As<IPatientService>().SingleInstance();
        builder.RegisterType<RenovationService>().As<IRenovationService>().SingleInstance();
        builder.RegisterType<ManageRenovationService>().As<IManageRenovationService>().SingleInstance();
        builder.RegisterType<NotificationService>().As<INotificationService>().SingleInstance();
        builder.RegisterType<SurvayService>().As<ISurvayService>().SingleInstance();


    }
}