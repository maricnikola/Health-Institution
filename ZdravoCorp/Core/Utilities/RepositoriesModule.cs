using Autofac;
using ZdravoCorp.Core.Models.Users;
using ZdravoCorp.Core.Repositories.EquipmentRepo;
using ZdravoCorp.Core.Repositories.InventoryRepo;
using ZdravoCorp.Core.Repositories.MedicalRecordRepo;
using ZdravoCorp.Core.Repositories.OrderRepo;
using ZdravoCorp.Core.Repositories.RoomRepo;
using ZdravoCorp.Core.Repositories.ScheduleRepo;
using ZdravoCorp.Core.Repositories.TransfersRepo;
using ZdravoCorp.Core.Repositories.UsersRepo;

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
       
       builder.RegisterType<ScheduleRepository>().As<IScheduleRepository>().SingleInstance();
       builder.RegisterType<TransferRepository>().As<ITransferRepository>().SingleInstance();
       builder.RegisterType<UserRepository>().As<IUserRepository<User>>().SingleInstance();
       builder.RegisterType<DoctorRepository>().As<IDoctorRepository>().SingleInstance();
       builder.RegisterType<NurseRepository>().As<INurseRepository>().SingleInstance();
       builder.RegisterType<DirectorRepository>().As<IDirectorRepository>().SingleInstance();
       builder.RegisterType<PatientRepository>().As<IPatientRepository>().SingleInstance();
    }
}