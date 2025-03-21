using Mayhem.Blockchain.Interfaces.Services;
using Mayhem.Configuration.Interfaces;
using Mayhen.Bl.Commands.Login;
using Mayhen.Bl.Validators.Base;

namespace Mayhem.Dal.Dto.Validations
{
    public class LoginCommandRequestValidator : BaseWalletValidator<LoginCommandRequest, LoginCommandResponse>
    {
        private readonly IMayhemConfigurationService mayhemConfigurationService;
        private readonly IBlockchainService blockchainService;

        public LoginCommandRequestValidator(IMayhemConfigurationService mayhemConfigurationService, IBlockchainService blockchainService)
        {
            this.mayhemConfigurationService = mayhemConfigurationService;
            this.blockchainService = blockchainService;
            Validation();
        }

        private void Validation()
        {
            ValidateWallet(mayhemConfigurationService.MayhemConfiguration.CommonConfiguration, blockchainService);
        }
    }
}