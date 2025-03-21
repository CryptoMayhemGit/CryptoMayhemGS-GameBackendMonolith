using FluentValidation;
using Mayhem.Dal.Dto.Classes.Buildings;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Messages;
using Mayhem.Util.Exceptions;
using Mayhen.Bl.Commands.AddBuildingToLand;
using Mayhen.Bl.Services.Interfaces;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Mayhen.Bl.Validators
{
    public class AddBuildingToLandCommandRequestValidator : BaseValidator<AddBuildingToLandCommandRequest>
    {
        private readonly ICostsValidationService costsValidationService;
        private readonly IMayhemDataContext mayhemDataContext;

        public AddBuildingToLandCommandRequestValidator(ICostsValidationService costsValidationService, IMayhemDataContext mayhemDataContext)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.costsValidationService = costsValidationService;
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
            VerifyUserBuildingCosts();
            VerifyLand();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.BuildingTypeId).IsInEnum();
            RuleFor(x => x.LandId).GreaterThanOrEqualTo(1);
        }

        private void VerifyUserBuildingCosts()
        {
            RuleFor(x => new { x.BuildingTypeId, x.UserId }).CustomAsync(async (request, context, cancellation) =>
            {
                Dictionary<ResourcesType, int> costs = BuildingCostsDictionary.GetBuildingCosts(request.BuildingTypeId, 1);

                List<ValidationMessage> result = await costsValidationService.ValidateUserAsync(costs, request.UserId);
                if (result.Count > 0)
                {
                    context.AddFailure(FailureMessages.OwnFailure(result.First().FieldName, result.First().Message));
                }
            });
        }

        private void VerifyLand()
        {
            RuleFor(x => new { x.LandId, x.UserId, x.BuildingTypeId }).CustomAsync(async (request, context, cancellation) =>
            {
                UserLand userLand = await mayhemDataContext
                    .UserLands
                    .Include(x => x.Land)
                    .ThenInclude(x => x.Buildings)
                    .Include(x => x.Land)
                    .ThenInclude(x => x.Npcs)
                    .SingleOrDefaultAsync(x => x.LandId == request.LandId && x.UserId == request.UserId, cancellationToken: cancellation);

                if (userLand == null)
                {
                    context.AddFailure(FailureMessages.UserLandWithIdDoesNotExistFailure(request.LandId));
                    return;
                }
                if (userLand.Status != LandsStatus.Explored)
                {
                    context.AddFailure(FailureMessages.LandWithIdMustBeDiscoveredFailure(request.LandId));
                    return;
                }
                if (userLand.Land.Buildings.Count > 0)
                {
                    context.AddFailure(FailureMessages.LandWithIdAlreadyHasBuildingFailure(request.LandId));
                    return;
                }
                if (!BuildingLandValidationDictionary.BuildingsPerSlot[userLand.Land.LandTypeId].Contains(request.BuildingTypeId))
                {
                    context.AddFailure(FailureMessages.BuildingCannotBeBuildOnLandFailure(request.BuildingTypeId.ToString(), userLand.Land.LandTypeId.ToString()));
                    return;
                }
                if (!userLand.Land.Npcs.Any(x => x.UserId == request.UserId))
                {
                    context.AddFailure(FailureMessages.BuildingCannotBeBuildWithoutNpcAvatarFailure(request.BuildingTypeId.ToString()));
                    return;
                }
            });
        }
    }
}
