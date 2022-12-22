using Cipher;
using NLog;
using ReportSmsService.Core.Repositories;
using SmsSender;
using System;
using System.Configuration;
using System.ServiceProcess;
using System.Timers;

namespace ReportSmsService
{
    public partial class ReportSmsService : ServiceBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private Timer _timer;
        private int _intervalInMinutes;
        private Sms _sms;
        private StringCipher _stringCipher = new StringCipher("3D4E8871-46CF-4312-9F6C-F9FF2C897CE1");
        private const string NotEncryptedPasswordPrefix = "encrypt:";
        private ErrorRepository _errorRepository = new ErrorRepository();

        public ReportSmsService()
        {
            Logger.Info("1");
            InitializeComponent();
            Logger.Info("2");
            ConfigureReportSmsService();
            Logger.Info("3");
        }

        private void ConfigureReportSmsService()
        {
            try
            {
                _sms = new Sms(new SmsParams
                {
                    SenderName = ConfigurationManager.AppSettings["SenderName"],
                    ReceiverNumber = ConfigurationManager.AppSettings["ReceiverNumber"],
                    SmsAccountLogin = ConfigurationManager.AppSettings["SmsAccountLogin"],
                    SmsAccountPassword = DecryptSmsAccountPassword()
                });

                _intervalInMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalInMinutes"]);
                _timer = new Timer(_intervalInMinutes * 60000);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private string DecryptSmsAccountPassword()
        {
            var encryptedPassword = ConfigurationManager.AppSettings["SenderSmsPassword"];

            if (encryptedPassword.StartsWith(NotEncryptedPasswordPrefix))
            {
                encryptedPassword = _stringCipher.Encrypt(encryptedPassword.Replace(NotEncryptedPasswordPrefix, string.Empty));

                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configFile.AppSettings.Settings["SenderEmailPassword"].Value = encryptedPassword;
                configFile.Save();
            }

            return _stringCipher.Decrypt(encryptedPassword);
        }

        protected override void OnStart(string[] args)
        {
            _timer.Elapsed += DoWork;
            _timer.Start();
            Logger.Info("Service started...");
        }

        private void DoWork(object sender, ElapsedEventArgs e)
        {
            try
            {
                SendError();

                Logger.Info("Error sent.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private void SendError()
        {
            Logger.Info("Sending error sms.");

            var errors = _errorRepository.GetLastErrors();

            foreach (var error in errors)
            {
                var smsBody = $"{error.Id} {error.Date.Date.ToString("dd-MM-yy")} {error.Message}";
                var response = _sms.Send(smsBody);

                Logger.Info($"Sms body: {smsBody}");
                Logger.Info($"SmsServer response: {response}");
            }
        }

        protected override void OnStop()
        {
            Logger.Info("Service stopped...");
        }
    }
}
