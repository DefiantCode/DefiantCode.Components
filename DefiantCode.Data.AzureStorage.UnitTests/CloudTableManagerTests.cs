using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Table;
using NSubstitute;

namespace DefiantCode.Data.AzureStorage.UnitTests
{
    [TestClass]
    public class CloudTableManagerTests
    {
        [TestMethod]
        public void CloudTableShouldSuppportMocking()
        {
            var mock = Substitute.For<CloudTableManager<MyEntity>>();
            var entity = new MyEntity();
            var resultStub = new TableResult();
            resultStub.HttpStatusCode = 200;
            mock.Insert(entity).Returns(resultStub);
            var svc = new MyTestService(mock);
            svc.DoTableInsert(entity);
            mock.Received().Insert(entity);
        }
    }

    public class MyTestService
    {
        private readonly CloudTableManager<MyEntity> _tableManager;

        public MyTestService(CloudTableManager<MyEntity> tableManager)
        {
            _tableManager = tableManager;
        }

        public void DoTableInsert(MyEntity entity)
        {
            var result = _tableManager.Insert(entity);
            if(result.HttpStatusCode != 200)
                throw new InvalidOperationException("Expected 200 status code");
        }
    }

    public class MyEntity : TableEntity
    {
        
    }
}
