using Autofac;
using ZdravoCorp.Core.HospitalAssets.Equipment.Repositories;
using ZdravoCorp.Core.HospitalAssets.Rooms.Repositories;
using ZdravoCorp.Core.HospitalSystem.Analytics.Repositories;
using ZdravoCorp.Core.HospitalSystem.AnnualLeaves.Repositories;
using ZdravoCorp.Core.HospitalSystem.Notifications.Repositories;
using ZdravoCorp.Core.HospitalSystem.Users.Models;
using ZdravoCorp.Core.HospitalSystem.Users.Repositories;
using ZdravoCorp.Core.PatientFiles.MedicalRecords.Repositories;
using ZdravoCorp.Core.PatientFiles.Presriptions.Repositories;
using ZdravoCorp.Core.PatientFiles.Refferals.Repositories;
using ZdravoCorp.Core.Scheduling.Repositories;

namespace ZdravoCorp.Core.Utilities;

public class RepositoriesModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<EquipmentRepository>().As<IEquipmentRepository>().SingleInstance();
        builder.RegisterType<RoomRepository>().As<IRoomRepository>().SingleInstance();
        builder.RegisterType<InventoryRepository>().As<IInventoryRepository>().SingleInstance();
        builder.RegisterType<MedicalRecordRepository>().As<IMedicalRecordRepository>().SingleInstance();
        builder.RegisterType<OrderRepository>().As<IOrderRepository>().SingleInstance();
        builder.RegisterType<RenovationRepository>().As<IRenovationRepository>().SingleInstance();
        builder.RegisterType<SpecialistsRefferalRepository>().As<ISpecialistsRefferalRepository>().SingleInstance();
        builder.RegisterType<HospitalRefferalRepository>().As<IHospitalRefferalRepository>().SingleInstance();
        builder.RegisterType<MedicamentRepository>().As<IMedicamentRepository>().SingleInstance();

        builder.RegisterType<ScheduleRepository>().As<IScheduleRepository>().SingleInstance();
        builder.RegisterType<TransferRepository>().As<ITransferRepository>().SingleInstance();
        builder.RegisterType<UserRepository>().As<IUserRepository<User>>().SingleInstance();
        builder.RegisterType<DoctorRepository>().As<IDoctorRepository>().SingleInstance();
        builder.RegisterType<NurseRepository>().As<INurseRepository>().SingleInstance();
        builder.RegisterType<DirectorRepository>().As<IDirectorRepository>().SingleInstance();
        builder.RegisterType<PatientRepository>().As<IPatientRepository>().SingleInstance();
        builder.RegisterType<NotificationRepository>().As<INotificationRepository>().SingleInstance();
        builder.RegisterType<DoctorSurveyRepository>().As<IDoctorSurveyRepository>().SingleInstance();
        builder.RegisterType<HospitalSurveyRepository>().As<IHospitalSurveyRepository>().SingleInstance();
        builder.RegisterType<AnnualLeaveRepository>().As<IAnnualLeaveRepository>().SingleInstance();
        

    }
}