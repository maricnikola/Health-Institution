using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.SpecialistsRefferals;

namespace ZdravoCorp.Core.Services.SpecialistsRefferalServices;

public interface ISpecialistsRefferalService
{
    public List<SpecialistsRefferal>? GetAll();
    public SpecialistsRefferal? GetById(int id);
    public void AddRefferal(SpecialistsRefferal specialistsRefferal);
    public void Delete(int id);
}
