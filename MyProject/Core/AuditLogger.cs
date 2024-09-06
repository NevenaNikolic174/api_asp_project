using MyProject.DataAccess;
using MyProject.Domain.Entities;

namespace MyProject.Api.Core
{
    public class AuditLogger
    {
        private readonly MyDBContext _dbCon;

        public AuditLogger(MyDBContext dbContext)
        {
            _dbCon = dbContext;
        }

        public void LogAudit(string username, string useCaseName, string action)
        {
            var auditLog = new AuditLog
            {
                UserName = username,
                UseCaseName = useCaseName,
                ActionStatus = action,
                ExecutedAt = DateTime.UtcNow
            };

            _dbCon.AuditLogs.Add(auditLog);
            _dbCon.SaveChanges();
        }
    }
}
