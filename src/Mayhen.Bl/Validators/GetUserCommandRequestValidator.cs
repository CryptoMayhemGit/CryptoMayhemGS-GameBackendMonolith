using AutoMapper;
using FluentValidation;
using Mayhem.Dal.Dto.Commands.GetUser;
using Mayhem.Dal.Interfaces.DataContext;
using Mayhem.Dal.Tables;
using Mayhem.Messages;
using Mayhen.Bl.Commands.GetUser;
using Mayhen.Bl.Validators.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Mayhen.Bl.Validators
{
    public class GetUserCommandRequestValidator : BaseValidator<GetUserCommandRequest>
    {
        private readonly IMayhemDataContext mayhemDataContext;
        private readonly IMapper mapper;

        public GetUserCommandRequestValidator(IMayhemDataContext mayhemDataContext, IMapper mapper)
        {
            this.mapper = mapper;
            this.mayhemDataContext = mayhemDataContext;
            Validation();
        }

        private void Validation()
        {
            VerifyUserExistence();
        }

        private void VerifyUserExistence()
        {
            RuleFor(x => x).CustomAsync(async (request, context, cancellation) =>
            {
                GetUserCommandRequestDto getUserCommandRequestDto = mapper.Map<GetUserCommandRequestDto>(request);

                IQueryable<GameUser> querableGameUser = mayhemDataContext
                    .GameUsers
                    .AsNoTracking()
                    .AsQueryable();

                GameUser gameUser = await querableGameUser.Where(x => x.Id == getUserCommandRequestDto.UserId).SingleOrDefaultAsync(cancellationToken: cancellation);

                if (gameUser == null)
                {
                    context.AddFailure(FailureMessages.UserWithIdDoesNotExistFailure(getUserCommandRequestDto.UserId));
                }
            });
        }
    }
}
