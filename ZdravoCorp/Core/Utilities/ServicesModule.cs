using Autofac;
using ZdravoCorp.Core.HospitalAssets.Rooms.Services;
using ZdravoCorp.Core.HospitalAssets.Rooms.Services.Services;
using ZdravoCorp.Core.HospitalSystem.Analytics.Services;
using ZdravoCorp.Core.HospitalSystem.AnnualLeaves.Services;
using ZdravoCorp.Core.HospitalSystem.Notifications.Services;
using ZdravoCorp.Core.HospitalSystem.Users.Services;
using ZdravoCorp.Core.PatientFiles.MedicalRecords.Services;
using ZdravoCorp.Core.PatientFiles.Presriptions.Services;
using ZdravoCorp.Core.PatientFiles.Refferals.Services;
using ZdravoCorp.Core.Scheduling.Services;

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
        builder.RegisterType<SurveyService>().As<ISurveyService>().SingleInstance();
        builder.RegisterType<AnnualLeaveService>().As<IAnnualLeaveService>().SingleInstance();
        


    }
}