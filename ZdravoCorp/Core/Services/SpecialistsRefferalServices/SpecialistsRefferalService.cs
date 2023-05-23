﻿using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Core.Models.SpecialistsRefferals;
using ZdravoCorp.Core.Repositories.SpecialistsRefferalsRepo;

namespace ZdravoCorp.Core.Services.SpecialistsRefferalServices;

public class SpecialistsRefferalService: ISpecialistsRefferalService
{
    private ISpecialistsRefferalRepository _specialistsRefferalRepository;

    public SpecialistsRefferalService(ISpecialistsRefferalRepository specialistsRefferalRepository)
    {
        _specialistsRefferalRepository = specialistsRefferalRepository;
    }

    public List<SpecialistsRefferal>? GetAll()
    {
        return _specialistsRefferalRepository.GetAll() as List<SpecialistsRefferal>;
    }

    public SpecialistsRefferal? GetById(int id)
    {
        return _specialistsRefferalRepository.GetById(id);
    }

    public void AddRefferal(SpecialistsRefferal specialistsRefferal)
    {
        _specialistsRefferalRepository.Insert(specialistsRefferal);
    }

    public void Delete(int id)
    {
        _specialistsRefferalRepository.Delete(GetById(id));
    }
}
