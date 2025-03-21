using FluentValidation;
using Mayhem.Dal.Dto.Classes.GuildBuildings;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Messages;
using Mayhem.Util.Exceptions;
using Mayhen.Bl.Commands.UpgradeGuildBuilding;
using Mayhen.Bl.Services.Interfaces;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Mayhen.Bl.Validators
{
    public class UpgradeGuildBuildingCommandRequestValidator : BaseValidator<UpgradeGuildBuildingCommandRequest>
    {
        private readonly IMayhemDataContext mayhemDataContext;
        private readonly IGuildBuildingRepository guildBuildingRepository;
        private readonly ICostsValidationService costsValidationService;
        private readonly IGuildImprovementValidationService guildImprovementValidationService;
        private readonly IImprovementRepository improvementRepository;

        public UpgradeGuildBuildingCommandRequestValidator(IMayhemDataContext mayhemDataContext,
            IGuildBuildingRepository guildBuildingRepository, ICostsValidationService costsValidationService,
            IGuildImprovementValidationService guildImprovementValidationService,
            IImprovementRepository improvementRepository)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.guildBuildingRepository = guildBuildingRepository;
            this.costsValidationService = costsValidationService;
            this.guildImprovementValidationService = guildImprovementValidationService;
            this.improvementRepository = improvementRepository;
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
            VerifyBuilding();
            VerifyUserResource();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.GuildBuildingId).GreaterThanOrEqualTo(1);
        }

        private void VerifyBuilding()
        {
            RuleFor(x => new { x.GuildBuildingId, x.UserId }).CustomAsync(async (request, context, cancellation) =>
            {

                GuildBuildingDto building = await guildBuildingRepository.GetGuildBuildingByIdAsync(request.GuildBuildingId);
                if (building == null)
                {
                    context.AddFailure(FailureMessages.GuildBuildingWithIdDoesNotExistFailure(request.GuildBuildingId));
                    return;
                }

                Dictionary<ResourcesType, int> costs = GuildBuildingCostsDictionary.GetGuildBuildingCosts(building.GuildBuildingTypeId, building.Level + 1);

                List<ValidationMessage> result = await costsValidationService.ValidateGuildAsync(costs, request.UserId);
                if (result.Count > 0)
                {
                    context.AddFailure(FailureMessages.OwnFailure(result.First().FieldName, result.First().Message));
                    return;
                }

                IEnumerable<GuildImprovementDto> improvements = await improvementRepository.GetGuildImprovementsByGuildIdAsync(building.GuildId);
                if (!guildImprovementValidationService.ValidateImprovement(building.Level + 1, building.GuildBuildingTypeId, improvements.Select(x => x.GuildImprovementTypeId)))
                {
                    context.AddFailure(FailureMessages.GuildBuildingCannotBeUpgradedDueTolackOfImprovementFailure());
                }
            });
        }

        private void VerifyUserResource()
        {
            RuleFor(x => new { x.GuildBuildingId, x.UserId }).CustomAsync(async (request, context, cancellation) =>
            {
                GuildBuilding guildBuilding = await mayhemDataContext
                .GuildBuildings
                .Include(x => x.Guild)
                .SingleOrDefaultAsync(x => x.Id == request.GuildBuildingId, cancellationToken: cancellation);

                if (guildBuilding == null)
                {
                    context.AddFailure(FailureMessages.GuildBuildingWithIdDoesNotExistFailure(request.GuildBuildingId));
                    return;
                }

                if (guildBuilding.Guild.OwnerId != request.UserId)
                {
                    context.AddFailure(FailureMessages.OnlyOwnerCanUpgradeBuildingFailure());
                }

                Dictionary<ResourcesType, int> costs = GuildBuildingCostsDictionary.GetGuildBuildingCosts(guildBuilding.GuildBuildingTypeId, guildBuilding.Level + 1);

                List<GuildResource> resources = await mayhemDataContext
                    .GuildResources
                    .Include(x => x.Guild)
                    .Where(x => x.GuildId == guildBuilding.Guild.Id)
                    .ToListAsync(cancellationToken: cancellation);

                if (resources.Count == 0)
                {
                    context.AddFailure(FailureMessages.GuildResourcesCouldNotBeFetchFailure());
                }
            });
        }
    }
}
