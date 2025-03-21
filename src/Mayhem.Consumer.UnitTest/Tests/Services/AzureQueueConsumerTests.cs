using FluentAssertions;
using Mayhem.Consumer.Test.Common.Builder;
using Mayhem.Consumer.UnitTest.Base;
using Mayhem.Item.QueueConsumer;
using Mayhem.Land.QueueConsumer;
using Mayhem.Notification.QueueConsumer;
using Mayhem.Npc.QueueConsumer;
using Mayhem.Queue.Consumer.Base.Services;
using Mayhem.Queue.Dto;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mayhem.Consumer.UnitTest.Tests.Services
{
    public class AzureQueueConsumerTests
    {
        [TestCaseSource(nameof(GetTestCases))]
        public void FromMessage_WhenMessageBodyExist_ThenGetObject_Test<T, E>(T instance, E fromMessageTestInstance)
        {
            string body = GetExampleBody();
            string expectedFirstErrorMessage = $"Getting body for message.";
            string expectedSecondErrorMessage = $"Message body {body}.";

            Mock<ILogger<AzureQueueConsumer<T>>> azureQueueConsumerLoggerMock = LoggerMockBuilder
                .Create<AzureQueueConsumer<T>>();

            IQueueClient queueClient = QueueClientMockBuilder
                .Create()
                .Build();

            byte[] bytes = GetExampleBytes(body);
            BaseAzureQueueConsumerTests<T> home = new(azureQueueConsumerLoggerMock.Object, queueClient);

            E result = home.FromMessageTest<E>(new Message() { Body = bytes });

            azureQueueConsumerLoggerMock.VerifyResult(Times.Exactly(2));
            azureQueueConsumerLoggerMock.VerifyResult(expectedFirstErrorMessage);
            azureQueueConsumerLoggerMock.VerifyResult(expectedSecondErrorMessage);
            result.Should().NotBeNull();
        }

        [TestCaseSource(nameof(GetTestCases))]
        public void FromMessage_WhenMessageBodyNotExist_ThenGetThrowArgumentException_Test<T, E>(T instance, E fromMessageTestInstance)
        {
            string expectedErrorMessage = $"Message has null or empty body.";

            Mock<ILogger<AzureQueueConsumer<T>>> azureQueueConsumerLoggerMock = LoggerMockBuilder
                .Create<AzureQueueConsumer<T>>();

            IQueueClient queueClient = QueueClientMockBuilder
                .Create()
                .Build();

            BaseAzureQueueConsumerTests<T> home = new(azureQueueConsumerLoggerMock.Object, queueClient);

            Action act = () => home.FromMessageTest<E>(new Message());

            act.Should().Throw<ArgumentException>().WithMessage(expectedErrorMessage);

            azureQueueConsumerLoggerMock.VerifyResult(Times.Once());
            azureQueueConsumerLoggerMock.VerifyResult(expectedErrorMessage);
        }

        private string GetExampleBody()
        {
            const string expectedFrom = "frog@AdriaGames.com";
            const string expectedTo = "to@AdriaGames.com";
            const string expectedValue = "Test";

            TransferQueueMessage transferQueueMessage = new()
            {
                Dtos = new List<TransferQueueDto>()
            };
            transferQueueMessage.Dtos.Add(new TransferQueueDto() { From = expectedFrom, To = expectedTo, Value = expectedValue });
            return JsonConvert.SerializeObject(transferQueueMessage);
        }

        private byte[] GetExampleBytes(string body)
        {
            return Encoding.ASCII.GetBytes(body);
        }

        private static IEnumerable<TestCaseData> GetTestCases()
        {
            yield return new TestCaseData(new ItemQueueConsumer(null, null, null), new TransferQueueMessage());
            yield return new TestCaseData(new LandQueueConsumer(null, null, null), new TransferQueueMessage());
            yield return new TestCaseData(new NpcQueueConsumer(null, null, null), new TransferQueueMessage());
            yield return new TestCaseData(new NotificationQueueConsumer(null, null, null, null), new TransferQueueMessage());
        }
    }
}