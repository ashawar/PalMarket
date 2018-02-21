using PalMarket.Common.Helpers;
using PalMarket.Domain.Contracts;
using PalMarket.Domain.Contracts.Repositories;
using PalMarket.Domain.Contracts.Services;
using PalMarket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Domain
{
    public class NotificationService : INotificationService
    {
        public NotificationService()
        {

        }

        #region INotificationService Members

        public void SendNotification(List<Device> devices, string message)
        {
            foreach (var device in devices)
            {
                //if (device.Type == DeviceType.Android)
                //{
                //    NotificationSend.SendPushNotificationForBadgeByFCM(device.RegisterID, title, message);
                //}
                //else
                //{
                NotificationSend.APNSSendPushNotificationForBadgeByPushSharp(device.RegisterID, message);
                //}
            }
        }

        #endregion
    }
}
