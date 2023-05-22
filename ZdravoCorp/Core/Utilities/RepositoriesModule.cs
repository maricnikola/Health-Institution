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
       builder.RegisterType<EquipmentRepository>().As<IEquipmentRepository>();
       builder.RegisterType<InventoryRepository>().As<IInventoryRepository>();
       builder.RegisterType<MedicalRecordRepository>().As<IMedicalRecordRepository>();
       builder.RegisterType<OrderRepository>().As<IOrderRepository>();
       builder.RegisterType<RoomRepository>().As<IRoomRepository>();
       builder.RegisterType<ScheduleRepository>().As<IScheduleRepository>();
       builder.RegisterType<TransferRepository>().As<ITransferRepository>();
       builder.RegisterType<UserRepository>().As<IUserRepository<User>>();
       builder.RegisterType<DoctorRepository>().As<IDoctorRepository>();
       builder.RegisterType<NurseRepository>().As<INurseRepository>();
       builder.RegisterType<DirectorRepository>().As<IDirectorRepository>();
       builder.RegisterType<PatientRepository>().As<IPatientRepository>();
    }
}