using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using PushSharp.Core;
//using PushSharp.Google;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace PalMarket.Common.Helpers
{
    public static class NotificationSend
    {
        public static void SendPushNotificationForBadgeByFCM(string deviceId, string message)
        {
            try
            {
                //https://firebase.google.com/docs/cloud-messaging/send-message


                //For local and QA environment
                string ServerKey = "AAAAECaG8a4:APA91bFBVk00OJtZU1GqindMptxwwJWPwP2j0w-eUavpooTehWKgffAYDK1tmjxOSZGiNyqxbVF5qyloizYTgDVDwYJMmDlltX15bqswgq3PEItJNpjIu-ZHW4TZg0WzDNV629FFgHM4";

                //////////////////////////////////////////////////////////////////////////

                ////For online environment
                //string ServerKey = "AAAAVqskYPM:APA91bFevN9I913sXRZd1D6RZbVf98xiEw36T-mu4do5ZYZ3dQhUgpaoOk9sWYna1F9kbjyE_Vd-c7g15DPgtnob-nJSB4je9FvwYJ2F0R9AfKCuAN01x_8HyrmFEzLMkx7mdDEr7e8J";

                //////////////////////////////////////////////////////////////////////////

                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var jsonData = new
                {
                    data = new
                    {
                        count = 5
                    },
                    to = deviceId
                };
                var json = JsonConvert.SerializeObject(jsonData);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", ServerKey));
                //tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;

                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                WebResponse tResponse = tRequest.GetResponse();
                Stream dataStreamResponse = tResponse.GetResponseStream();
                StreamReader tReader = new StreamReader(dataStreamResponse);
                String sResponseFromServer = tReader.ReadToEnd();
                string str = sResponseFromServer;

                //using (Stream dataStream = tRequest.GetRequestStream())
                //{
                //    dataStream.Write(byteArray, 0, byteArray.Length);
                //    using (WebResponse tResponse = tRequest.GetResponse())
                //    {
                //        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                //        {
                //            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                //            {
                //                String sResponseFromServer = tReader.ReadToEnd();
                //                string str = sResponseFromServer;
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }

        public static void APNSSendPushNotificationForBadgeByPushSharp(string deviceId, string message)
        {
            //For local and QA environment
            string pathToFiles = HttpContext.Current.Server.MapPath("~/Certifications/PalMarClientCertPushNotification.p12");
            var appleCert = File.ReadAllBytes(pathToFiles);

            // Configuration (NOTE: .pfx can also be used here)
            var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Sandbox,
                appleCert, "PalMar.PUSH.NOTIFICATION?"); //For local and QA environment

            //////////////////////////////////////////////////////////////////////////////

            ////For Online environment
            //string pathToFiles = HttpContext.Current.Server.MapPath("~/Certifications/ss-prod.p12");
            //var appleCert = File.ReadAllBytes(pathToFiles);

            //// Configuration (NOTE: .pfx can also be used here)
            //var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Production,
            //    appleCert, "ss123"); //For online environment

            //////////////////////////////////////////////////////////////////////////


            // Create a new broker
            var apnsBroker = new ApnsServiceBroker(config);

            // Wire up events
            apnsBroker.OnNotificationFailed += (notification, aggregateEx) =>
            {

                aggregateEx.Handle(ex =>
                {

                    // See what kind of exception it was to further diagnose
                    if (ex is ApnsNotificationException)
                    {
                        var notificationException = (ApnsNotificationException)ex;

                        // Deal with the failed notification
                        var apnsNotification = notificationException.Notification;
                        var statusCode = notificationException.ErrorStatusCode;

                        //Console.WriteLine($"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}");

                    }
                    else
                    {
                        // Inner exception might hold more useful information like an ApnsConnectionException			
                        //Console.WriteLine($"Apple Notification Failed for some unknown reason : {ex.InnerException}");
                    }

                    // Mark it as handled
                    return true;
                });
            };

            apnsBroker.OnNotificationSucceeded += (notification) =>
            {
                //Console.WriteLine("Apple Notification Sent!");
            };

            // Start the broker
            apnsBroker.Start();

            var data = new
            {
                aps = new
                {
                    alert = message,
                    sound = "default",
                    link_url = ""
                }
            };

            //var data = new
            //{
            //    aps = new
            //    {
            //        badge = count,
            //    }
            //};
            var json = JsonConvert.SerializeObject(data);

            //foreach (var deviceToken in MY_DEVICE_TOKENS)
            //{
            // Queue a notification to send
            apnsBroker.QueueNotification(new ApnsNotification
            {
                DeviceToken = deviceId,
                //Payload = JObject.Parse("{\"aps\":{\"badge\":7}}")
                Payload = JObject.Parse(json)
            });
            //}

            // Stop the broker, wait for it to finish   
            // This isn't done after every message, but after you're
            // done with the broker
            apnsBroker.Stop();
        }

        //public static void GCMSendPushNotificationForBadgeByPushSharp(string deviceId)
        //{
        //    // Configuration
        //    var config = new GcmConfiguration("GCM-SENDER-ID", "AUTH-TOKEN", null);

        //    // Create a new broker
        //    var gcmBroker = new GcmServiceBroker(config);

        //    // Wire up events
        //    gcmBroker.OnNotificationFailed += (notification, aggregateEx) =>
        //    {

        //        aggregateEx.Handle(ex =>
        //        {

        //            // See what kind of exception it was to further diagnose
        //            if (ex is GcmNotificationException)
        //            {
        //                var notificationException = (GcmNotificationException)ex;

        //                // Deal with the failed notification
        //                var gcmNotification = notificationException.Notification;
        //                var description = notificationException.Description;

        //                Console.WriteLine($"GCM Notification Failed: ID={gcmNotification.MessageId}, Desc={description}");
        //            }
        //            else if (ex is GcmMulticastResultException)
        //            {
        //                var multicastException = (GcmMulticastResultException)ex;

        //                foreach (var succeededNotification in multicastException.Succeeded)
        //                {
        //                    Console.WriteLine($"GCM Notification Succeeded: ID={succeededNotification.MessageId}");
        //                }

        //                foreach (var failedKvp in multicastException.Failed)
        //                {
        //                    var n = failedKvp.Key;
        //                    var e = failedKvp.Value;

        //                    Console.WriteLine($"GCM Notification Failed: ID={n.MessageId}, Desc={e.Description}");
        //                }

        //            }
        //            else if (ex is DeviceSubscriptionExpiredException)
        //            {
        //                var expiredException = (DeviceSubscriptionExpiredException)ex;

        //                var oldId = expiredException.OldSubscriptionId;
        //                var newId = expiredException.NewSubscriptionId;

        //                Console.WriteLine($"Device RegistrationId Expired: {oldId}");

        //                if (!string.IsNullOrWhiteSpace(newId))
        //                {
        //                    // If this value isn't null, our subscription changed and we should update our database
        //                    Console.WriteLine($"Device RegistrationId Changed To: {newId}");
        //                }
        //            }
        //            else if (ex is RetryAfterException)
        //            {
        //                var retryException = (RetryAfterException)ex;
        //                // If you get rate limited, you should stop sending messages until after the RetryAfterUtc date
        //                Console.WriteLine($"GCM Rate Limited, don't send more until after {retryException.RetryAfterUtc}");
        //            }
        //            else
        //            {
        //                Console.WriteLine("GCM Notification Failed for some unknown reason");
        //            }

        //            // Mark it as handled
        //            return true;
        //        });
        //    };

        //    gcmBroker.OnNotificationSucceeded += (notification) =>
        //    {
        //        Console.WriteLine("GCM Notification Sent!");
        //    };

        //    // Start the broker
        //    gcmBroker.Start();

        //    //    foreach (var regId in MY_REGISTRATION_IDS)
        //    //    {
        //    //        // Queue a notification to send
        //    //        gcmBroker.QueueNotification(new GcmNotification
        //    //        {
        //    //            RegistrationIds = new List<string> {
        //    //    regId
        //    //},
        //    //            Data = JObject.Parse("{ \"somekey\" : \"somevalue\" }")
        //    //        });
        //    //    }

        //    // Stop the broker, wait for it to finish   
        //    // This isn't done after every message, but after you're
        //    // done with the broker
        //    gcmBroker.Stop();
        //}
    }
}
