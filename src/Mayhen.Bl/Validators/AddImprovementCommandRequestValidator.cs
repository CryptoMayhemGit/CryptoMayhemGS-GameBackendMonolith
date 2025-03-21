using FluentValidation;
using Mayhem.Dal.Dto.Classes.Improvements;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.Messages;
using Mayhem.Util.Exceptions;
using Mayhen.Bl.Commands.AddImprovement;
using Mayhen.Bl.Services.Interfaces;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Mayhen.Bl.Validators
{
    public class AddImprovementCommandRequestValidator : BaseValidator<AddImprovementCommandRequest>
    {
        private readonly ICostsValidationService costsValidationService;
        private readonly IMayhemDataContext mayhemDataContext;

        public AddImprovementCommandRequestValidator(ICostsValidationService costsValidationService, IMayhemDataContext mayhemDataContext)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.costsValidationService = costsValidationService;
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
            VerifyImprovementCost();
            VerifyLand();
            VerifyExistingImprovement();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.LandId).GreaterThanOrEqualTo(1);
            RuleFor(x => x.ImprovementTypeId).IsInEnum();
        }

        private void VerifyImprovementCost()
        {
            RuleFor(x => new { x.ImprovementTypeId, x.UserId }).CustomAsync(async (request, context, cancellation) =>
            {
                Dictionary<ResourcesType, int> costs = ImprovementCostsDictionary.GetImprovementCosts(request.ImprovementTypeId);

                List<ValidationMessage> result = await costsValidationService.ValidateUserAsync(costs, request.UserId);
                if (result.Count > 0)
                {
                    context.AddFailure(FailureMessages.OwnFailure(result.First().FieldName, result.First().Message));
                }
            });
        }

        private void VerifyLand()
        {
            RuleFor(x => new { x.ImprovementTypeId, x.LandId }).CustomAsync(async (request, context, cancellation) =>
            {
                Land land = await mayhemDataContext.Lands.SingleOrDefaultAsync(x => x.Id == request.LandId, cancellationToken: cancellation);

                if (land == null)
                {
                    context.AddFailure(FailureMessages.LandWithIdDoesNotExistFailure(request.LandId));
                }
            });
        }

        private void VerifyExistingImprovement()
        {
            RuleFor(x => new { x.ImprovementTypeId, x.LandId }).CustomAsync(async (request, context, cancellation) =>
            {
                Improvement existingImprovement = await mayhemDataContext
                    .Improvements
                    .SingleOrDefaultAsync(x => x.LandId == request.LandId
                    && x.ImprovementTypeId == request.ImprovementTypeId, cancellationToken: cancellation);

                if (existingImprovement != null)
                {
                    context.AddFailure(FailureMessages.ImprovementWithLandIdAndImprovementTypeIdAlreadyExistsFailure(request.LandId, request.ImprovementTypeId.ToString()));
                }
            });
        }
    }
}
