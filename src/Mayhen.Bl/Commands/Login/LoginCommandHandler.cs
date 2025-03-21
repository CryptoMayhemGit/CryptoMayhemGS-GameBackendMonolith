using AutoMapper;
using Mayhem.Dal.Dto.Classes.AuditLogs;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhem.Messages;
using Mayhem.Util.Classes;
using Mayhen.Bl.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mayhen.Bl.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommandRequest, LoginCommandResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IAuthService authService;
        private readonly IAuditLogRepository auditLogRepository;
        private readonly IMapper mapper;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IAuthService authService,
            IAuditLogRepository auditLogRepository,
            IMapper mapper)
        {
            this.userRepository = userRepository;
            this.authService = authService;
            this.auditLogRepository = auditLogRepository;
            this.mapper = mapper;
        }

        public async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
        {
            string token = await authService.CreateTokenAsync(request.Wallet);

            if (!string.IsNullOrEmpty(token))
            {
                await userRepository.LoginAsync(request.Wallet);

                await auditLogRepository.AddAuditLogAsync(mapper.Map<AuditLogDto>(request, opts =>
                {
                    opts.Items[MappingParamConstants.LoginActionParam] = AuditLogNames.Login;
                }));

                return new LoginCommandResponse()
                {
                    Token = token,
                };
            }

            throw ExceptionMessages.CannotGenerateTokenException(request.Wallet);
        }
    }
}
