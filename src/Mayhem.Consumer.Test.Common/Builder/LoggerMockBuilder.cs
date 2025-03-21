using Microsoft.Extensions.Logging;
using Moq;
using System;

namespace Mayhem.Consumer.Test.Common.Builder
{
    public static class LoggerMockBuilder
    {
        public static Mock<ILogger<T>> Create<T>()
        {
            return new Mock<ILogger<T>>();
        }

        public static ILogger<T> Build<T>(this Mock<ILogger<T>> logger)
        {
            return logger.Object;
        }

        public static void VerifyResult<T>(this Mock<ILogger<T>> logger, Times times)
        {
            logger.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), times);
        }

        public static void VerifyResult<T>(this Mock<ILogger<T>> logger, string failMessage)
        {
            Func<object, Type, bool> state = (v, t) => v.ToString().CompareTo(failMessage) == 0;

            logger.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => state(v, t)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }
    }
}
