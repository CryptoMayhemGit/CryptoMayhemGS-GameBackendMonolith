using FluentAssertions;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables.AuditLogs;
using Mayhem.UnitTest.Base;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Repositories
{
    public class AuditLogRepositoryTests : UnitTestBase
    {
        private IAuditLogRepository auditLogRepository;
        private IMayhemDataContext mayhemDataContext;

        [OneTimeSetUp]
        public void SetUp()
        {
            auditLogRepository = GetService<IAuditLogRepository>();
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test]
        public async Task AddAuditLog_WhenAuditLogAdded_ThenGetIt_Test()
        {
            AuditLogDto auditLog = await auditLogRepository.AddAuditLogAsync(new AuditLogDto()
            {
                Message = "message",
                Nonce = 1234213,
                SignedMessage = "signed message",
                Wallet = "wallet address",
                Action = "UnitTest",
            });

            AuditLog auditLogDb = await mayhemDataContext.AuditLogs.SingleOrDefaultAsync(x => x.Id == auditLog.Id);

            auditLog.Should().NotBeNull();
            auditLogDb.Should().NotBeNull();
            auditLogDb.Message.Should().Be(auditLog.Message);
            auditLogDb.Nonce.Should().Be(auditLog.Nonce);
            auditLogDb.SignedMessage.Should().Be(auditLog.SignedMessage);
            auditLogDb.Wallet.Should().Be(auditLog.Wallet);
            auditLogDb.Action.Should().Be(auditLog.Action);
        }
    }
}
