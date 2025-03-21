using FluentValidation;
using Mayhem.Dal.Dto.Classes.Buildings;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Buildings;
using Mayhem.Messages;
using Mayhem.Util.Exceptions;
using Mayhen.Bl.Commands.UpgradeBuilding;
using Mayhen.Bl.Services.Interfaces;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Mayhen.Bl.Validators
{
    public class UpgradeBuildingCommandRequestValidator : BaseValidator<UpgradeBuildingCommandRequest>
    {
        private readonly IBuildingRepository buildingRepository;
        private readonly IMayhemDataContext mayhemDataContext;
        private readonly ICostsValidationService costsValidationService;
        private readonly IImprovementRepository improvementRepository;
        private readonly IImprovementValidationService improvementValidationService;

        public UpgradeBuildingCommandRequestValidator(IMayhemDataContext mayhemDataContext, IBuildingRepository buildingRepository, ICostsValidationService costsValidationService, IImprovementRepository improvementRepository, IImprovementValidationService improvementValidationService)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.buildingRepository = buildingRepository;
            this.costsValidationService = costsValidationService;
            this.improvementRepository = improvementRepository;
            this.improvementValidationService = improvementValidationService;
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
            RuleFor(x => x.BuildingId).GreaterThanOrEqualTo(1);
        }

        private void VerifyBuilding()
        {
            RuleFor(x => new { x.BuildingId, x.UserId }).CustomAsync(async (request, context, cancellation) =>
            {
                BuildingDto building = await buildingRepository.GetBuildingByIdAsync(request.BuildingId);
                if (building == null)
                {
                    context.AddFailure(FailureMessages.BuildingWithIdDoesNotExistFailure(request.BuildingId));
                    return;
                }

                Dictionary<ResourcesType, int> costs = BuildingCostsDictionary.GetBuildingCosts(building.BuildingTypeId, building.Level + 1);
                List<ValidationMessage> result = await costsValidationService.ValidateUserAsync(costs, request.UserId);
                if (result.Count > 0)
                {
                    context.AddFailure(FailureMessages.OwnFailure(result.First().FieldName, result.First().Message));
                    return;
                }

                LandDto land = await buildingRepository.GetLandByBuildingIdAsync(building.Id);
                IEnumerable<ImprovementDto> improvements = await improvementRepository.GetImprovementsByLandIdAsync(land.Id);
                if (!improvementValidationService.ValidateImprovement(building.Level + 1, building.BuildingTypeId, improvements.Select(x => x.ImprovementTypeId)))
                {
                    context.AddFailure(FailureMessages.BuildingCannotBeUpgradedDueTolackOfImprovementFailure());
                }
            });
        }

        private void VerifyUserResource()
        {
            RuleFor(x => new { x.BuildingId, x.UserId }).CustomAsync(async (request, context, cancellation) =>
            {
                Building building = await mayhemDataContext
                .Buildings
                .Include(x => x.Land)
                .SingleOrDefaultAsync(x => x.Id == request.BuildingId, cancellationToken: cancellation);

                if (building == null)
                {
                    return;
                }

                Dictionary<ResourcesType, int> costs = BuildingCostsDictionary.GetBuildingCosts(building.BuildingTypeId, building.Level + 1);
                List<UserResource> resources = await mayhemDataContext
                    .UserResources
                    .Where(x => x.UserId == request.UserId)
                    .ToListAsync(cancellationToken: cancellation);

                if (resources.Count == 0)
                {
                    context.AddFailure(FailureMessages.UserResourcesCouldNotBeFetchedFailure());
                }
            });
        }
    }
}
