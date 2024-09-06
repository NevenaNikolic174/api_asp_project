using MyProject.Application.Actor;
using MyProject.Application.UseCases;
using MyProject.DataAccess;
using MyProject.Domain.Entities;
using MyProject.Implementation.UseCases;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Implementation.Logging
{
    public class DBUseCaseLogger : EfUseCase, IUseCaseLogger
    {
        public DBUseCaseLogger(MyDBContext context) : base(context)
        {
        }

        public void Log(IUseCase useCase, IApplicationActor actor, object useCaseData)
        {
            AuditLog log = new AuditLog
            {
                UserName = actor.Username,
                UseCaseName = useCase.Name,
                ActionStatus = JsonConvert.SerializeObject(useCaseData),
                ExecutedAt = DateTime.UtcNow,
            };

            Context.AuditLogs.Add(log);
            Context.SaveChanges();
        }
    }

}
