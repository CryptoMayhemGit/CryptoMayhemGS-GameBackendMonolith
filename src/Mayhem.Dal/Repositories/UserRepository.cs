using AutoMapper;
using Mayhem.Dal.Dto.Commands.GetUser;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhem.Dal.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMayhemDataContext mayhemDataContext;
        private readonly IMapper mapper;

        private static ICollection<UserResource> BasicResource =>
            Enum.GetValues(typeof(ResourcesType)).Cast<ResourcesType>().Select(type => new UserResource()
            {
                ResourceTypeId = type,
                Value = 0,
            }).ToList();

        public UserRepository(IMayhemDataContext mayhemDataContext, IMapper mapper)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.mapper = mapper;
        }

        public async Task<int?> CreateUserAsync(string walletAddress, string email)
        {
            using IDbContextTransaction ts = await mayhemDataContext.Database.BeginTransactionAsync();
            try
            {
                GameUser gameUser = new()
                {
                    Email = email,
                    WalletAddress = walletAddress,
                    UserResources = BasicResource,
                };

                EntityEntry<GameUser> newGameUser = await mayhemDataContext.GameUsers.AddAsync(gameUser);
                await mayhemDataContext.SaveChangesAsync();

                string guidId = Guid.NewGuid().ToString();
                ApplicationUser applicationUser = new()
                {
                    Id = guidId,
                    Email = email,
                    UserId = newGameUser.Entity.Id,
                    UserName = walletAddress,
                    NormalizedUserName = walletAddress.ToUpper(),
                    LockoutEnabled = true,
                };

                EntityEntry<ApplicationUser> newApplicationUser = await mayhemDataContext.ApplicationUsers.AddAsync(applicationUser);

                await mayhemDataContext.SaveChangesAsync();
                await ts.CommitAsync();

                return (newApplicationUser.Entity != null && newGameUser.Entity.Id > 0) ? applicationUser.UserId : null;
            }
            catch (Exception ex)
            {
                await ts.RollbackAsync();
                throw ExceptionMessages.TransactionException(ex, nameof(CreateUserAsync));
            }
        }

        public async Task LoginAsync(string walletAddress)
        {
            GameUser user = await mayhemDataContext
                .GameUsers
                .Where(x => x.WalletAddress.Equals(walletAddress))
                .SingleOrDefaultAsync();

            if (user != null)
            {
                user.LastLoginDate = DateTime.UtcNow;
                await mayhemDataContext.SaveChangesAsync();
            }
        }

        public async Task<GetUserCommandResponseDto> GetUserAsync(GetUserCommandRequestDto getUserRequest)
        {
            IQueryable<GameUser> querableGameUser = mayhemDataContext
                .GameUsers
                .AsNoTracking()
                .AsQueryable();

            if (getUserRequest.WithItems)
            {
                querableGameUser = querableGameUser.Include(x => x.Items);
            }
            if (getUserRequest.WithLands)
            {
                querableGameUser = querableGameUser.Include(x => x.UserLands);
            }
            if (getUserRequest.WithNpcs)
            {
                querableGameUser = querableGameUser.Include(x => x.Npcs);
            }
            if (getUserRequest.WithResources)
            {
                querableGameUser = querableGameUser.Include(x => x.UserResources);
            }

            GameUser gameUser = await querableGameUser.Where(x => x.Id == getUserRequest.UserId).SingleOrDefaultAsync();

            return new GetUserCommandResponseDto()
            {
                GameUser = mapper.Map<GameUserDto>(gameUser),
                Items = getUserRequest.WithItems ? gameUser.Items.Select(x => mapper.Map<ItemDto>(x)).ToList() : null,
                UserLands = getUserRequest.WithLands ? gameUser.UserLands.Select(x => mapper.Map<UserLandDto>(x)).ToList() : null,
                Npcs = getUserRequest.WithNpcs ? gameUser.Npcs.Select(x => mapper.Map<NpcDto>(x)).ToList() : null,
                UserResources = getUserRequest.WithResources ? gameUser.UserResources.Select(x => mapper.Map<UserResourceDto>(x)).ToList() : null,
            };
        }

        public async Task<ApplicationUserDto> GetApplicationUserByIdAsync(int userId) => await mayhemDataContext
            .ApplicationUsers
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => mapper.Map<ApplicationUserDto>(x))
            .SingleOrDefaultAsync();

        public async Task<ApplicationUserDto> GetApplicationUserByWalletAsync(string wallet) => await mayhemDataContext
            .ApplicationUsers
            .AsNoTracking()
            .Where(x => x.UserName == wallet)
            .Select(x => mapper.Map<ApplicationUserDto>(x))
            .SingleOrDefaultAsync();

        public async Task<bool> CheckEmailAsync(string email)
        {
            if (await mayhemDataContext.Notifications.SingleOrDefaultAsync(x => x.Email == email) != null)
            {
                return true;
            }

            if (await mayhemDataContext.GameUsers.SingleOrDefaultAsync(x => x.Email == email) != null)
            {
                return true;
            }

            return false;
        }
    }
}
