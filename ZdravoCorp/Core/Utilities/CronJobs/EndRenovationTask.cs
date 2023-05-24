using System.Threading.Tasks;
using Quartz;
using ZdravoCorp.Core.Models.Renovation;
using ZdravoCorp.Core.Services.RenovationServices;

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
        if (_renovationDto.Split != null)
        {
            _manageRenovationService.EndWithSplit(_renovationDto);
        }else if (_renovationDto.Join != null)
        {
            _manageRenovationService.EndWithJoin(_renovationDto);
        }
        else
        {
            if (_manageRenovationService.End(_renovationDto.Room.Id))
            {
                _renovationService.UpdateStatus(_renovationDto.Id, Renovation.RenovationStatus.Finished);
            }
            else
            {
                _renovationService.UpdateStatus(_renovationDto.Id, Renovation.RenovationStatus.Failed);
            }
            return Task.CompletedTask;
        }
        
       
        
        return Task.CompletedTask;

    }
}