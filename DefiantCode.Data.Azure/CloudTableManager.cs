using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace DefiantCode.Data.AzureStorage
{
    public class CloudTableManager<T> where T : TableEntity, new()
    {
        public CloudTableClient TableClient { get; set; }
        public CloudTable Table { get; set; }

        public CloudTableManager(CloudStorageAccount storageAccount, string tableName)
            : this(storageAccount.CreateCloudTableClient(), tableName)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
        {
        }

        public CloudTableManager(CloudTableClient tableClient, string tableName)
        {
            TableClient = tableClient;
            Table = TableClient.GetTableReference(tableName);
            Table.CreateIfNotExists();
        }

        public CloudTableManager()
        {
            
        }

        public virtual TableResult Insert(T entity)
        {
            return Table.Execute(TableOperation.Insert(entity));
        }

        public virtual TableResult Replace(T entity)
        {
            if (String.IsNullOrEmpty(entity.ETag))
                entity.ETag = "*";

            return Table.Execute(TableOperation.Replace(entity));
        }

        public virtual IList<TableResult> InsertBatch(params T[] entities)
        {
            var batch = new TableBatchOperation();
            foreach (var entity in entities)
            {
                batch.Insert(entity);
            }

            return Table.ExecuteBatch(batch);
        }

        public virtual TableResult Retrieve(string partitionKey, string rowKey)
        {
            var retrieveOp = TableOperation.Retrieve<T>(partitionKey, rowKey);
            return Table.Execute(retrieveOp);
        }

        public virtual IEnumerable<T> ExecuteQuery(TableQuery<T> query)
        {
            return Table.ExecuteQuery(query);
        }

    }
}
