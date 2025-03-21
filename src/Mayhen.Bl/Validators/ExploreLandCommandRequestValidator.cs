using FluentValidation;
using Mayhem.Dal.Dto.Enums.Dictionaries;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.Messages;
using Mayhen.Bl.Commands.ExploreLand;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Mayhen.Bl.Validators
{
    public class ExploreLandCommandRequestValidator : BaseValidator<ExploreLandCommandRequest>
    {
        private readonly IMayhemDataContext mayhemDataContext;

        public ExploreLandCommandRequestValidator(IMayhemDataContext mayhemDataContext)
        {
            this.mayhemDataContext = mayhemDataContext;
            Validation();
        }

        private void Validation()
        {
            ValidateBase();
            VerifyLandsExistence();
            VerifyMissionExistance();
        }

        private void ValidateBase()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.LandId).GreaterThan(0);
        }

        private void VerifyLandsExistence()
        {
            RuleFor(x => new { x.LandId, x.UserId, x.NpcId }).CustomAsync(async (request, context, cancellation) =>
            {
                Land land = await mayhemDataContext
                    .Lands
                    .SingleOrDefaultAsync(x => x.Id == request.LandId, cancellation);

                if (land == null)
                {
                    context.AddFailure(FailureMessages.LandWithIdDoesNotExistFailure(request.LandId));
                    return;
                }

                List<Npc> userNpcs = await mayhemDataContext
                    .Npcs
                    .Where(x => x.UserId == request.UserId && x.LandId == request.LandId)
                    .ToListAsync(cancellationToken: cancellation);

                if (!userNpcs.Any(x => x.Id == request.NpcId))
                {
                    context.AddFailure(FailureMessages.LandWithIdDoesNotHaveAnyHeroFailure(request.LandId));
                    return;
                }

                UserLand userLand = await mayhemDataContext
                   .UserLands
                   .SingleOrDefaultAsync(x => x.LandId == request.LandId && x.UserId == request.UserId, cancellation);

                if (userLand == null)
                {
                    context.AddFailure(FailureMessages.LandWithIdMustBeDiscoveredFailure(request.LandId));
                    return;
                }

                if (userLand != null && userLand.Status == LandsStatus.Explored)
                {
                    context.AddFailure(FailureMessages.LandWithIdIsAlreadyExploredFailure(request.LandId));
                    return;
                }

                UserLand userLandOtherOwner = await mayhemDataContext
                    .UserLands
                    .SingleOrDefaultAsync(x => x.LandId == request.LandId && x.UserId != request.UserId && x.Owned, cancellationToken: cancellation);

                if (userLandOtherOwner != null)
                {
                    context.AddFailure(FailureMessages.LandWithIdBelongsToAnotherUserFailure(request.LandId));
                    return;
                }

                if (land.LandTypeId == LandsType.Water || land.LandTypeId == LandsType.Forest)
                {
                    context.AddFailure(FailureMessages.LandWithIdHasWrongTypeFailure(request.LandId));
                    return;
                }
            });
        }

        private void VerifyMissionExistance()
        {
            RuleFor(x => new { x.LandId, x.UserId, x.NpcId }).CustomAsync(async (request, context, cancellation) =>
            {
                Npc npc = await mayhemDataContext
                    .Npcs
                    .SingleOrDefaultAsync(x => x.Id == request.NpcId, cancellationToken: cancellation);

                if (npc.NpcStatusId != NpcsStatus.None)
                {
                    context.AddFailure(FailureMessages.NpcWithIdIsBusy(request.NpcId));
                    return;
                }

                bool existingMissionWithNpc = await mayhemDataContext
                    .ExploreMissions
                    .AnyAsync(x => x.LandId == request.LandId && x.UserId == request.UserId, cancellationToken: cancellation);

                if (existingMissionWithNpc)
                {
                    context.AddFailure(FailureMessages.AnotherNpcIsOnAMissionOnThisLand());
                    return;
                }
            });
        }
    }
}
