using System;
using ZdravoCorp.Core.Repositories.Equipment;
using ZdravoCorp.Core.Repositories.Inventory;
using ZdravoCorp.Core.Repositories.MedicalRecord;
using ZdravoCorp.Core.Repositories.Order;
using ZdravoCorp.Core.Repositories.Room;
using ZdravoCorp.Core.Repositories.Schedule;
using ZdravoCorp.Core.Repositories.Transfers;
using ZdravoCorp.Core.Repositories.User;

namespace ZdravoCorp.Core.Repositories;

public class RepositoryManager
{
    private UserRepository _userRepository;
    private DirectorRepository _directorRepository;
    private PatientRepository _patientRepository;
    private NurseRepository _nurseRepository;
    private DoctorRepository _doctorRepository;
    private EquipmentRepository _equipmentRepository;
    private RoomRepository _roomRepository;
    private InventoryRepository _inventoryRepository;
    private MedicalRecordRepository _medicalRecordRepository;
    private ScheduleRepository _scheduleRepository;
    private OrderRepository _orderRepository;
    private TransferRepository _transferRepository;

    public UserRepository UserRepository
    {
        get => _userRepository;
        set => _userRepository = value ?? throw new ArgumentNullException(nameof(value));
    }

    public DirectorRepository DirectorRepository
    {
        get => _directorRepository;
        set => _directorRepository = value ?? throw new ArgumentNullException(nameof(value));
    }

    public PatientRepository PatientRepository
    {
        get => _patientRepository;
        set => _patientRepository = value ?? throw new ArgumentNullException(nameof(value));
    }

    public NurseRepository NurseRepository
    {
        get => _nurseRepository;
        set => _nurseRepository = value ?? throw new ArgumentNullException(nameof(value));
    }

    public DoctorRepository DoctorRepository
    {
        get => _doctorRepository;
        set => _doctorRepository = value ?? throw new ArgumentNullException(nameof(value));
    }

    public EquipmentRepository EquipmentRepository
    {
        get => _equipmentRepository;
        set => _equipmentRepository = value ?? throw new ArgumentNullException(nameof(value));
    }

    public RoomRepository RoomRepository
    {
        get => _roomRepository;
        set => _roomRepository = value ?? throw new ArgumentNullException(nameof(value));
    }

    public InventoryRepository InventoryRepository
    {
        get => _inventoryRepository;
        set => _inventoryRepository = value ?? throw new ArgumentNullException(nameof(value));
    }

    public MedicalRecordRepository MedicalRecordRepository
    {
        get => _medicalRecordRepository;
        set => _medicalRecordRepository = value ?? throw new ArgumentNullException(nameof(value));
    }

    public ScheduleRepository ScheduleRepository
    {
        get => _scheduleRepository;
        set => _scheduleRepository = value ?? throw new ArgumentNullException(nameof(value));
    }

    public OrderRepository OrderRepository
    {
        get => _orderRepository;
        set => _orderRepository = value ?? throw new ArgumentNullException(nameof(value));
    }

    public TransferRepository TransferRepository
    {
        get => _transferRepository;
        set => _transferRepository = value ?? throw new ArgumentNullException(nameof(value));
    }

    public RepositoryManager()
    {
        _userRepository = new UserRepository();
        _directorRepository = new DirectorRepository();
        _patientRepository = new PatientRepository();
        _nurseRepository = new NurseRepository();
        _doctorRepository = new DoctorRepository();
        _equipmentRepository = new EquipmentRepository();
         _roomRepository = new RoomRepository();
        _inventoryRepository = new InventoryRepository(_roomRepository, _equipmentRepository);
        _medicalRecordRepository =  new MedicalRecordRepository();
        _scheduleRepository = new ScheduleRepository();
        _orderRepository = new OrderRepository();
        _transferRepository = new TransferRepository();
    }
}