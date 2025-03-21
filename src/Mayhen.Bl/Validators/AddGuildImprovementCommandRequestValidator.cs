using FluentValidation;
using Mayhem.Dal.Dto.Classes.Improvements;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables.Guilds;
using Mayhem.Messages;
using Mayhem.Util.Exceptions;
using Mayhen.Bl.Commands.AddGuildImprovement;
using Mayhen.Bl.Services.Interfaces;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Mayhen.Bl.Validators
{
    public class AddGuildImprovementCommandRequestValidator : BaseValidator<AddGuildImprovementCommandRequest>
    {
        private readonly ICostsValidationService costsValidationService;
        private readonly IMayhemDataContext mayhemDataContext;

        public AddGuildImprovementCommandRequestValidator(ICostsValidationService costsValidationService, IMayhemDataContext mayhemDataContext)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.costsValidationService = costsValidationService;
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
            VerifyGuildImprovementCost();
            VerifyGuildImprovement();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.GuildId).GreaterThanOrEqualTo(1);
            RuleFor(x => x.GuildImprovementTypeId).IsInEnum();
        }

        private void VerifyGuildImprovementCost()
        {
            RuleFor(x => new { x.GuildImprovementTypeId, x.UserId }).CustomAsync(async (request, context, cancellation) =>
            {
                Dictionary<ResourcesType, int> costs = GuildImprovementCostsDictionary.GetGuildImprovementCosts(request.GuildImprovementTypeId);

                List<ValidationMessage> result = await costsValidationService.ValidateGuildAsync(costs, request.UserId);

                if (result.Count > 0)
                {
                    context.AddFailure(FailureMessages.OwnFailure(result.First().FieldName, result.First().Message));
                }
            });
        }

        private void VerifyGuildImprovement()
        {
            RuleFor(x => x).CustomAsync(async (request, context, cancellation) =>
            {
                Guild guild = await mayhemDataContext.Guilds.SingleOrDefaultAsync(x => x.Id == request.GuildId, cancellationToken: cancellation);

                if (guild == null)
                {
                    context.AddFailure(FailureMessages.GuildWithIdDoesNotExistFailure(request.GuildId));
                    return;
                }
                else if (guild.OwnerId != request.UserId)
                {
                    context.AddFailure(FailureMessages.OnlyOwnerCanAddImprovementFailure());
                    return;
                }

                GuildImprovement existingGuildImprovement = await mayhemDataContext
                    .GuildImprovements
                    .SingleOrDefaultAsync(x => x.GuildId == request.GuildId
                    && x.GuildImprovementTypeId == request.GuildImprovementTypeId, cancellationToken: cancellation);

                if (existingGuildImprovement != null)
                {
                    context.AddFailure(FailureMessages.ImprovementWithGuildIdAndImprovementTypeIdAlreadyExistsFailure(request.GuildId, request.GuildImprovementTypeId.ToString()));
                }
            });
        }
    }
}
