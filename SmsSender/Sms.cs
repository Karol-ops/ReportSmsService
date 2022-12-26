using SerwerSMS;
using System;
using System.Collections.Generic;

namespace SmsSender
{
    public class Sms
    {
        private string _smsAccountLogin;
        private string _smsAccountPassword;
        private string _receiverNumber;
        private string _senderName;

        public Sms(SmsParams smsParams)
        {
            _smsAccountLogin = smsParams.SmsAccountLogin;
            _smsAccountPassword = smsParams.SmsAccountPassword;
            _receiverNumber = smsParams.ReceiverNumber;
            _senderName = smsParams.SenderName;
        }

        public string Send(string smsBody)
        {
            try
            {
                var serwerssms = new SerwerSms(_smsAccountLogin, _smsAccountPassword);
                var data = new Dictionary<string, string> {{ "details", "1" }};
                var response = serwerssms.Messages.SendSms(_receiverNumber, smsBody, _senderName, data).ToString();

                return response;
            }
            catch (Exception ex)
            {
                return ex.Message;
            };
        }
    }
}
