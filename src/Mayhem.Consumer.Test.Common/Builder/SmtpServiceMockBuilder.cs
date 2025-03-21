using Mayhem.SmtpBase.Dto;
using Mayhem.SmtpBase.Services.Interfaces;
using Moq;
using System;


namespace Mayhem.Consumer.Test.Common.Builder
{
    public static class SmtpServiceMockBuilder
    {
        public static Mock<ISmtpService> Create()
        {
            return new Mock<ISmtpService>();
        }

        public static Mock<ISmtpService> WithSendAsync(this Mock<ISmtpService> smtpService, bool result)
        {
            smtpService
                .Setup(x => x.SendAsync(It.IsAny<EmailRequestDto>()))
                .ReturnsAsync(result);

            return smtpService;
        }

        public static Mock<ISmtpService> WithSendAsyncThrow(this Mock<ISmtpService> smtpService)
        {
            smtpService
                .Setup(x => x.SendAsync(It.IsAny<EmailRequestDto>()))
                .ThrowsAsync(new Exception());

            return smtpService;
        }

        public static ISmtpService Build(this Mock<ISmtpService> smtpService)
        {
            return smtpService.Object;
        }
    }
}
