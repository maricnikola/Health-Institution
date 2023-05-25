using System.Threading.Tasks;
using Quartz;
using ZdravoCorp.Core.Models.Renovation;
using ZdravoCorp.Core.Services.RenovationServices;

namespace ZdravoCorp.Core.Utilities.CronJobs;

public class StartRenovationTask : IJob
{
    private IRenovationService _renovationService;
    private IManageRenovationService _manageRenovationService;
    private RenovationDTO _renovationDto;
    public Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        _renovationDto = (RenovationDTO)dataMap["renovation"];
        _renovationService = (IRenovationService)dataMap["renser"];
        _manageRenovationService = (IManageRenovationService)dataMap["renman"];
        if (_manageRenovationService.StartRenovation(_renovationDto.Room.Id))
        {
            if (_renovationDto.Join != null)
                _manageRenovationService.StartRenovation(_renovationDto.Join.Id);
            _renovationService.UpdateStatus(_renovationDto.Id, Renovation.RenovationStatus.InProgress);
        }
        else
        {
            _renovationService.UpdateStatus(_renovationDto.Id, Renovation.RenovationStatus.Failed);
        }
        
        
        return Task.CompletedTask;

    }
}