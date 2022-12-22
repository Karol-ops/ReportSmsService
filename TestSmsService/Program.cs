using serwersms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSmsService
{
    public class Program
    {
        static void Main(string[] args)
        {
            //try
            //{

                //    var serwerssms = new SerwerSMS("webapi_ReportSmsService", "ReportSmsService1!");
                //    var data = new Dictionary<string, string>();
                //    data.Add("test", "0");
                //    serwerssms.format = "json";
                //    var response = serwerssms.senders.index(data).ToString();
                //    /*
                //        serwerssms.format = "xml";
                //        XmlDocument response  = serwerssms.senders.index(data);
                //        XmlNodeList elemlist = response.GetElementsByTagName("message");
                //        string result = elemlist[0].InnerXml; 
                //        Console.WriteLine(result);
                //    */
                //    Console.WriteLine(response);

                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e.Message);
                //}
                //Console.ReadKey(true);
                try
                {

                    var serwerssms = new SerwerSMS("webapi_ReportSmsService", "qqqq");
                    var data = new Dictionary<string, string>();

                    // SMS FULL

                    String phone = "+48608000000";
                    String text = "test sms";
                    String sender = "INFORMACJA";
                    data.Add("details", "1");
                    var response = serwerssms.messages.sendSms(phone, text, sender, data).ToString();
                    Console.WriteLine(response);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                };
            }
    }
}
