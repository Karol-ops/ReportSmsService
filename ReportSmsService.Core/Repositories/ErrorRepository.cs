using ReportSmsService.Core.Domains;
using System;
using System.Collections.Generic;

namespace ReportSmsService.Core.Repositories
{
    public class ErrorRepository
    {
        public List<Error> GetLastErrors()
        {
            return new List<Error>
            {
                new Error(){ Id = 7, Message = "System Down", Date = DateTime.Now},
                new Error(){ Id = 8, Message = "Power off", Date = DateTime.Now.AddMinutes(15)}
            };
        }
    }
}
