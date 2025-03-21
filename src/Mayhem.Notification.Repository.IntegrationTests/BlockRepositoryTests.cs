using Dapper;
using FluentAssertions;
using Mayhem.Blockchain.Enums;
using Mayhem.Notification.Repository.IntegrationTests.Base;
using Mayhem.Worker.Dal.Dto;
using Mayhem.Workers.Dal.Repositories.Interfaces;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Mayhem.Notification.Repository.IntegrationTests
{
    internal class BlockRepositoryTests : RepositoryBaseTest
    {
        private IBlockRepository blockRepository;

        [OneTimeSetUp]
        public void Setup()
        {
            blockRepository = GetBlockRepository();
        }

        [Test]
        public async Task GetLastBlock_WhenBlockExist_ThenGetIt_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string addBlock = "insert into dbo.Block (LastBlock, BlockTypeId, CreationDate, LastModificationDate) values (100, 1, GETUTCDATE(), null); SELECT CAST(SCOPE_IDENTITY() as int)";
                int blockId = await db.QuerySingleAsync<int>(addBlock);
                BlockDto block = await blockRepository.GetLastBlockAsync(BlocksType.Npc);
                string removeBlock = $"delete from dbo.Block where id = {blockId}";
                await db.QueryAsync(removeBlock);
                string getBlock = $"select * from dbo.Block where id = {blockId}";
                IEnumerable<dynamic> result = await db.QueryAsync(getBlock);

                block.LastBlock.Should().Be(100);
                result.Should().HaveCount(0);
            }
        }

        [Test]
        public async Task UpdateLastBlock_WhenBlockUpdated_ThenGetIt_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                string addBlock = "insert into dbo.Block (LastBlock, BlockTypeId, CreationDate, LastModificationDate) values (100, 1, GETUTCDATE(), null); SELECT CAST(SCOPE_IDENTITY() as int)";
                int blockId = await db.QuerySingleAsync<int>(addBlock);
                await blockRepository.UpdateLastBlockAsync(200, BlocksType.Npc);
                BlockDto block = await blockRepository.GetLastBlockAsync(BlocksType.Npc);
                string removeBlock = $"delete from dbo.Block where id = {blockId}";
                await db.QueryAsync(removeBlock);
                string getBlock = $"select * from dbo.Block where id = {blockId}";
                IEnumerable<dynamic> result = await db.QueryAsync(getBlock);

                block.LastBlock.Should().Be(200);
                result.Should().HaveCount(0);
            }
        }
    }
}
