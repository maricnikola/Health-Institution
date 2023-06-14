using System.Threading.Tasks;
using Quartz;
using ZdravoCorp.Core.HospitalAssets.Rooms.Models;
using ZdravoCorp.Core.HospitalAssets.Rooms.Services;

namespace ZdravoCorp.Core.Utilities.CronJobs;

public class EndRenovationTask : IJob
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
        bool success = true;
        if (_renovationDto.Split != null)
        {
            success = _manageRenovationService.EndWithSplit(_renovationDto);
        }else if (_renovationDto.Join != null)
        {
            success = _manageRenovationService.EndWithJoin(_renovationDto);
        }
        else
        {
            success = _manageRenovationService.End(_renovationDto.Room.Id);
        }

        if (success)
        {
            _renovationService.UpdateStatus(_renovationDto.Id, Renovation.RenovationStatus.Finished);
        }
        else
        {
            _renovationService.UpdateStatus(_renovationDto.Id, Renovation.RenovationStatus.Failed);
        }
       
        
        return Task.CompletedTask;

    }
}