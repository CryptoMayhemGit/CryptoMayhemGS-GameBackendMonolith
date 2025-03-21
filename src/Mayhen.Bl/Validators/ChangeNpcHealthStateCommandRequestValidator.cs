using FluentValidation;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Messages;
using Mayhen.Bl.Commands.ChangeNpcHealthState;
using Mayhen.Bl.Validators.Base;

namespace Mayhen.Bl.Validators
{
    public class ChangeNpcHealthStateCommandRequestValidator : BaseValidator<ChangeNpcHealthStateCommandRequest>
    {
        private readonly INpcRepository npcRepository;

        public ChangeNpcHealthStateCommandRequestValidator(INpcRepository npcRepository)
        {
            this.npcRepository = npcRepository;
            Validation();
        }

        private void Validation()
        {
            VerifyBasicData();
            VerifyUserExistence();
        }

        private void VerifyBasicData()
        {
            RuleFor(x => x.NpcHealthState).IsInEnum();
            RuleFor(x => x.NpcId).GreaterThanOrEqualTo(1);
        }

        private void VerifyUserExistence()
        {
            RuleFor(x => new { x.NpcId, x.UserId }).CustomAsync(async (request, context, cancellation) =>
            {
                NpcDto npc = await npcRepository.GetUserNpcByIdAsync(request.NpcId, request.UserId);
                if (npc == null)
                {
                    context.AddFailure(FailureMessages.NpcWithIdDoesNotExistFailure(request.NpcId));
                }
            });
        }
    }
}
