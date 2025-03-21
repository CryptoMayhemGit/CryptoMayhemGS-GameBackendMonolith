using FluentValidation;
using Mayhem.Dal.Dto.Classes.GuildBuildings;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Messages;
using Mayhem.Util.Exceptions;
using Mayhen.Bl.Commands.AddGuildBuilding;
using Mayhen.Bl.Services.Interfaces;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mayhen.Bl.Validators
{
    public class AddGuildBuildingCommandRequestValidator : BaseValidator<AddGuildBuildingCommandRequest>
    {
        private readonly ICostsValidationService costsValidationService;
        private readonly IMayhemDataContext mayhemDataContext;

        public AddGuildBuildingCommandRequestValidator(ICostsValidationService costsValidationService, IMayhemDataContext mayhemDataContext)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.costsValidationService = costsValidationService;
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
            VerifyGuild();
            VerifyGuildBuildingCosts();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.GuildBuildingTypeId).IsInEnum();
            RuleFor(x => x.GuildId).GreaterThanOrEqualTo(1);
        }

        private void VerifyGuildBuildingCosts()
        {
            RuleFor(x => new { x.GuildBuildingTypeId, x.UserId }).CustomAsync(async (request, context, cancellation) =>
            {
                Dictionary<ResourcesType, int> costs = GuildBuildingCostsDictionary.GetGuildBuildingCosts(request.GuildBuildingTypeId, 1);

                List<ValidationMessage> result = await costsValidationService.ValidateGuildAsync(costs, request.UserId);

                if (result.Count > 0)
                {
                    context.AddFailure(FailureMessages.OwnFailure(result.First().FieldName, result.First().Message));
                }
            });
        }

        private void VerifyGuild()
        {
            RuleFor(x => new { x.GuildId, x.UserId, x.GuildBuildingTypeId }).CustomAsync(async (request, context, cancellation) =>
            {

                Guild guild = await mayhemDataContext
                    .Guilds
                    .Include(x => x.GuildBuildings)
                    .SingleOrDefaultAsync(x => x.Id == request.GuildId, cancellationToken: cancellation);

                if (guild == null)
                {
                    context.AddFailure(FailureMessages.GuildWithIdDoesNotExistFailure(request.GuildId));
                    return;
                }
                else if (guild.OwnerId != request.UserId)
                {
                    context.AddFailure(FailureMessages.OnlyOwnerCanAddBuildingFailure());
                }
                else if (guild.GuildBuildings.Select(x => x.GuildBuildingTypeId).Contains(request.GuildBuildingTypeId))
                {
                    context.AddFailure(FailureMessages.BuildingWithTypeAlreadyExistsFailure(request.GuildBuildingTypeId.ToString()));
                }
            });
        }
    }
}
