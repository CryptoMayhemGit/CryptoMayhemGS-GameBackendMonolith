using Mayhem.Consumer.Dal.Interfaces.Wrapers;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;

namespace Mayhem.Consumer.Test.Common.Builder
{
    public static class DapperWrapperMockBuilder
    {
        public static Mock<IDapperWrapper> Create()
        {
            return new Mock<IDapperWrapper>();
        }

        public static Mock<IDapperWrapper> WithQueryFirstOrDefaultAsync<T>(this Mock<IDapperWrapper> dapperWrapperMock, T result)
        {
            dapperWrapperMock
                .Setup(x => x.QueryFirstOrDefaultAsync<T>(It.IsAny<IDbConnection>(), It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(result);

            return dapperWrapperMock;
        }

        public static Mock<IDapperWrapper> WithQueryFirstOrDefaultAsyncThrowError<T>(this Mock<IDapperWrapper> dapperWrapperMock)
        {
            dapperWrapperMock
                .Setup(x => x.QueryFirstOrDefaultAsync<T>(It.IsAny<IDbConnection>(), It.IsAny<string>(), It.IsAny<object>()))
                .ThrowsAsync(new Exception());

            return dapperWrapperMock;
        }

        public static Mock<IDapperWrapper> WithQueryFirstAsync<T>(this Mock<IDapperWrapper> dapperWrapperMock, T result)
        {
            dapperWrapperMock
                .Setup(x => x.QueryFirstAsync<T>(It.IsAny<IDbConnection>(), It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(result);

            return dapperWrapperMock;
        }

        public static Mock<IDapperWrapper> WithQueryAsync(this Mock<IDapperWrapper> dapperWrapperMock)
        {
            dapperWrapperMock
                .Setup(x => x.QueryAsync(It.IsAny<IDbConnection>(), It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(It.IsAny<IEnumerable<dynamic>>());

            return dapperWrapperMock;
        }

        public static Mock<IDapperWrapper> WithQueryAsyncThrowError(this Mock<IDapperWrapper> dapperWrapperMock)
        {
            dapperWrapperMock
                .Setup(x => x.QueryAsync(It.IsAny<IDbConnection>(), It.IsAny<string>(), It.IsAny<object>()))
                .ThrowsAsync(new Exception());

            return dapperWrapperMock;
        }

        public static IDapperWrapper Build(this Mock<IDapperWrapper> logger)
        {
            return logger.Object;
        }
    }
}
