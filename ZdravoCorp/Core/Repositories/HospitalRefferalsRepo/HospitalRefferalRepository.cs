using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Core.Models.HospitalRefferals;
using ZdravoCorp.Core.Models.SpecialistsRefferals;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.Repositories.HospitalRefferalsRepo;

public class HospitalRefferalRepository: ISerializable, IHospitalRefferalRepository
{
    private readonly string _fileName = @".\..\..\..\..\Data\hospitalRefferal.json";
    private List<HospitalRefferal>? _hospitalRefferals;

    public HospitalRefferalRepository()
    {
        _hospitalRefferals = new List<HospitalRefferal>();
        Serializer.Load(this);
    }

    public void Insert(HospitalRefferal newHospitalRefferal)
    {
        _hospitalRefferals.Add(newHospitalRefferal);

        Serializer.Save(this);
    }

    string ISerializable.FileName()
    {
        return _fileName;
    }

    IEnumerable<object>? ISerializable.GetList()
    {
        return _hospitalRefferals;
    }

    void ISerializable.Import(JToken token)
    {
        _hospitalRefferals = token.ToObject<List<HospitalRefferal>>();
    }
    public void SaveChanges()
    {
        Serializer.Save(this);
    }

    public List<HospitalRefferal> GetAll()
    {
        return _hospitalRefferals;
    }

    public void Delete(HospitalRefferal entity)
    {
        _hospitalRefferals.Remove(entity);
        Serializer.Save(this);
    }

    public HospitalRefferal GetById(int id)
    {
        return _hospitalRefferals.FirstOrDefault(refferal => refferal.Id == id);
    }
}
