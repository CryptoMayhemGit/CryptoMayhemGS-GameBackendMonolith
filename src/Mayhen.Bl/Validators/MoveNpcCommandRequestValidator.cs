using FluentValidation;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables.Nfts;
using Mayhem.Messages;
using Mayhen.Bl.Commands.MoveNpc;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Mayhen.Bl.Validators
{
    public class MoveNpcCommandRequestValidator : BaseValidator<MoveNpcCommandRequest>
    {
        private readonly IMayhemDataContext mayhemDataContext;

        public MoveNpcCommandRequestValidator(IMayhemDataContext mayhemDataContext)
        {
            this.mayhemDataContext = mayhemDataContext;
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
            VerifyNpcAndLands();
            VerifyMissions();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.NpcId).GreaterThanOrEqualTo(1);
            RuleFor(x => x.LandToId).GreaterThanOrEqualTo(1);
        }

        private void VerifyNpcAndLands()
        {
            RuleFor(x => new { x.NpcId, x.UserId, x.LandToId }).CustomAsync(async (request, context, cancellation) =>
            {
                bool travelWithNpcExist = await mayhemDataContext.Travels.Where(x => x.NpcId == request.NpcId).AnyAsync(cancellation);

                if (travelWithNpcExist)
                {
                    context.AddFailure(FailureMessages.TravelWithNpcWithIdExist(request.NpcId));
                    return;
                }

                Npc npc = await mayhemDataContext
                    .Npcs
                    .SingleOrDefaultAsync(x => x.Id == request.NpcId && x.UserId == request.UserId, cancellationToken: cancellation);

                if (npc == null)
                {
                    context.AddFailure(FailureMessages.NpcWithIdDoesNotExistFailure(request.NpcId));
                    return;
                }
                if (npc.LandId == null)
                {
                    context.AddFailure(FailureMessages.NpcIsNotInAnyLand(request.NpcId));
                    return;
                }
                if (npc.LandId == request.LandToId)
                {
                    context.AddFailure(BaseMessages.LandFromMustBeDifferentThanLandToBaseMessage);
                    return;
                }

                Land landFrom = await mayhemDataContext
                    .Lands
                    .Include(x => x.UserLands)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == npc.LandId.Value, cancellationToken: cancellation);

                if (landFrom == null)
                {
                    context.AddFailure(FailureMessages.LandWithIdDoesNotExistFailure(npc.LandId.Value));
                    return;
                }
                if (landFrom?.Id != npc?.LandId)
                {
                    context.AddFailure(FailureMessages.LandFromMustBeTheSameAsNpcLandFailure());
                    return;
                }

                Land landTo = await mayhemDataContext
                    .Lands
                    .Include(x => x.UserLands)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == request.LandToId, cancellationToken: cancellation);

                if (landTo == null)
                {
                    context.AddFailure(FailureMessages.LandWithIdDoesNotExistFailure(request.LandToId));
                    return;
                }
                if (landFrom.LandInstanceId != landTo.LandInstanceId)
                {
                    context.AddFailure(FailureMessages.LandFromAndLandToMustBelongToTheSameLandInstanceFailure());
                    return;
                }
            });
        }

        private void VerifyMissions()
        {
            RuleFor(x => new { x.NpcId, x.UserId, x.LandToId }).CustomAsync(async (request, context, cancellation) =>
            {
                bool npcOnDiscoveryMission = await mayhemDataContext.DiscoveryMissions.AnyAsync(x => x.NpcId == request.NpcId, cancellationToken: cancellation);
                if (npcOnDiscoveryMission)
                {
                    context.AddFailure(FailureMessages.NpcWithIdIsBusy(request.NpcId));
                    return;
                }
                bool npcOnExploreMission = await mayhemDataContext.ExploreMissions.AnyAsync(x => x.NpcId == request.NpcId, cancellationToken: cancellation);
                if (npcOnExploreMission)
                {
                    context.AddFailure(FailureMessages.NpcWithIdIsBusy(request.NpcId));
                    return;
                }
            });
        }
    }
}
