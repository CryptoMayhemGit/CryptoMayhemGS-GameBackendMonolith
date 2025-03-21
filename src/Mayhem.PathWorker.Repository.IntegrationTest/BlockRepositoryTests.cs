using Dapper;
using FluentAssertions;
using Mayhem.Blockchain.Enums;
using Mayhem.PathWorker.Repository.IntegrationTest.Base;
using Mayhem.Worker.Dal.Dto;
using Mayhem.Workers.Dal.Repositories.Interfaces;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Mayhem.PathWorker.Repository.IntegrationTest
{
    internal class BlockRepositoryTests : BaseRepositoryTests
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
                long lastBlockValue = 12;
                BlocksType blockType = BlocksType.Item;
                string addBlock = $"insert into [dbo].[Block] (LastBlock, BlockTypeId) values ({lastBlockValue}, {(int)blockType}); SELECT CAST(SCOPE_IDENTITY() as int)";
                int blockId = await db.QuerySingleAsync<int>(addBlock);
                BlockDto lastBlock = await blockRepository.GetLastBlockAsync(blockType);

                string removeBlock = $"delete from [dbo].[Block] where id = {blockId}";
                await db.QueryAsync(removeBlock);

                string getBlocks = $"select * from [dbo].[Block]";
                IEnumerable<dynamic> blocksResult = await db.QueryAsync(getBlocks);

                lastBlock.LastBlock.Should().Be(lastBlockValue);
                lastBlock.BlockTypeId.Should().Be(blockType);
                blocksResult.Should().HaveCount(0);
            }
        }

        [Test]
        public async Task UpdateLastBlock_WhenBlockUpdated_ThenGetIt_Test()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                long lastBlockValue = 12;
                long newBlockValue = 23;
                BlocksType blockType = BlocksType.Item;
                string addBlock = $"insert into [dbo].[Block] (LastBlock, BlockTypeId) values ({lastBlockValue}, {(int)blockType}); SELECT CAST(SCOPE_IDENTITY() as int)";
                int blockId = await db.QuerySingleAsync<int>(addBlock);
                await blockRepository.UpdateLastBlockAsync(newBlockValue, blockType);
                string getBlockValue = $"select LastBlock from [dbo].[Block]";
                long blockValue = await db.QuerySingleAsync<long>(getBlockValue);

                string removeBlock = $"delete from [dbo].[Block] where id = {blockId}";
                await db.QueryAsync(removeBlock);

                string getBlocks = $"select * from [dbo].[Block]";
                IEnumerable<dynamic> blocksResult = await db.QueryAsync(getBlocks);

                blockValue.Should().Be(newBlockValue);
                blocksResult.Should().HaveCount(0);
            }
        }
    }
}
