using FluentAssertions;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.UnitTest.Base;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayhem.UnitTest.Repositories
{
    public class JobRepositoryTests : UnitTestBase
    {
        private IJobRepository jobRepository;
        private IMayhemDataContext mayhemDataContext;

        [OneTimeSetUp]
        public void SetUp()
        {
            jobRepository = GetService<IJobRepository>();
            mayhemDataContext = GetService<IMayhemDataContext>();
        }

        [Test]
        public async Task GetJobsByLandId_WhenJobsExists_ThenGetThem_Test()
        {
            EntityEntry<Land> newLand = await mayhemDataContext.Lands.AddAsync(new Land());

            await mayhemDataContext.Jobs.AddAsync(new Job() { LandId = newLand.Entity.Id });
            await mayhemDataContext.Jobs.AddAsync(new Job() { LandId = newLand.Entity.Id });

            await mayhemDataContext.SaveChangesAsync();

            IEnumerable<JobDto> jobs = await jobRepository.GetJobsByLandIdAsync(newLand.Entity.Id);

            jobs.Should().NotBeNull();
            jobs.Should().HaveCount(2);
        }
    }
}
