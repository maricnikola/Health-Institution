using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ZdravoCorp.Core.HospitalSystem.Notifications.Models;
using ZdravoCorp.Core.Utilities;

namespace ZdravoCorp.Core.HospitalSystem.Notifications.Repositories;

public class NotificationRepository : ISerializable, INotificationRepository
{
    private readonly string _fileName = @".\..\..\..\..\Data\notifications.json";
    private List<Notification>? _notifications;

    public NotificationRepository()
    {
        _notifications = new List<Notification>();
        Serializer.Load(this);
    }
    public IEnumerable<Notification> GetAll()
    {
        return _notifications;
    }

    public void Insert(Notification notification)
    {
        _notifications.Add(notification);
        Serializer.Save(this);
    }

    public void Delete(Notification notification)
    {
        _notifications.Remove(notification);
        Serializer.Save(this);
    }

    public Notification GetById(int id)
    {
        return _notifications.FirstOrDefault(notification=> notification.Id == id);
    }

    public void UpdateStatus(int id, Notification.NotificationStatus status)
    {
        GetById(id).Status = status;
        Serializer.Save(this);
    }

    public void UpdateTime(int id, DateTime when)
    {
        GetById(id).When = when;
        Serializer.Save(this);
    }

    public string FileName()
    {
        return _fileName;
    }

    public IEnumerable<object>? GetList()
    {
        return _notifications;
    }

    public void Import(JToken token)
    {
        _notifications = token.ToObject<List<Notification>>();
    }
}