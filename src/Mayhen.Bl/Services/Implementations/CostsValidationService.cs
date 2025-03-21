using Mayhem.Dal.Dto.Commands.GetUser;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Messages;
using Mayhem.Util.Exceptions;
using Mayhen.Bl.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mayhen.Bl.Services.Implementations
{
    public class CostsValidationService : ICostsValidationService
    {
        private readonly IUserRepository userRepository;
        private readonly IGuildRepository guildRepository;

        public CostsValidationService(IUserRepository userRepository, IGuildRepository guildRepository)
        {
            this.userRepository = userRepository;
            this.guildRepository = guildRepository;
        }

        public async Task<List<ValidationMessage>> ValidateUserAsync(Dictionary<ResourcesType, int> costs, int userId)
        {
            List<ValidationMessage> messages = new();

            GetUserCommandResponseDto user = await userRepository.GetUserAsync(new GetUserCommandRequestDto()
            {
                UserId = userId,
                WithResources = true,
            });

            if (user == null)
            {
                messages.Add(ValidationMessages.UserDoesNotExistMessage(userId));
                return messages;
            }

            foreach (KeyValuePair<ResourcesType, int> cost in costs)
            {
                UserResourceDto userResource = user.UserResources.SingleOrDefault(x => x.ResourceTypeId == cost.Key);
                if (userResource.Value < cost.Value)
                {
                    messages.Add(ValidationMessages.UserDoesNotHaveEnoughResourceExistMessage(cost.Key.ToString()));

                }
            }

            return messages;
        }


        public async Task<List<ValidationMessage>> ValidateGuildAsync(Dictionary<ResourcesType, int> costs, int userId)
        {
            List<ValidationMessage> messages = new();

            GuildDto guild = await guildRepository.GetGuildWithResourcesByOwnerIdAsync(userId);

            if (guild == null)
            {
                messages.Add(ValidationMessages.GuildDoesNotExistMessage());
                return messages;
            }

            foreach (KeyValuePair<ResourcesType, int> cost in costs)
            {
                GuildResourceDto userResource = guild.GuildResources.SingleOrDefault(x => x.ResourceTypeId == cost.Key);
                if (userResource.Value < cost.Value)
                {
                    messages.Add(ValidationMessages.GuildDoesNotHaveEnoughResourceMessage());
                }
            }

            return messages;
        }
    }
}
