using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Queue.Protocol;

namespace DefiantCode.Data.AzureStorage
{
    public class CloudQueueManager : CloudManagerBase
    {
        private readonly TimeSpan? _defaultVisibilityTimeout;
        public virtual CloudQueueClient QueueClient { get; private set; }


        public CloudQueueManager(CloudStorageAccount storageAccount, TimeSpan? defaultVisibilityTimeout)
            : this(storageAccount.CreateCloudQueueClient(), defaultVisibilityTimeout)
        {
        }

        public CloudQueueManager(CloudQueueClient queueClient, TimeSpan? defaultVisibilityTimeout)
        {
            _defaultVisibilityTimeout = defaultVisibilityTimeout;
            QueueClient = queueClient;
        }

        private CloudQueue GetQueueAndCreateIfNotExists(string queueName, bool createIfNotExists = false)
        {
            var queue = QueueClient.GetQueueReference(queueName);
            if (!createIfNotExists) return queue;

            queue.CreateIfNotExists(QueueRequestOptions, OperationContext);
            return queue;
        }

        public virtual CloudQueue GetQueue(string queueName, bool createIfNotExists = false)
        {
            try
            {
                var queue = GetQueueAndCreateIfNotExists(queueName, createIfNotExists);
                return queue;
            }
            finally
            {
                ResetOptionsAndContext();
            }
        }

        public virtual CloudQueue CreateQueue(string queueName, QueuePermissions permissions = null)
        {
            try
            {
                var queue = GetQueueAndCreateIfNotExists(queueName, true);
                if(permissions != null)
                    queue.SetPermissions(permissions, QueueRequestOptions, OperationContext);
                //TODO: add some transaction support--if the SetPermissions call fails should we delete the queue?
                return queue;
            }
            finally
            {
                ResetOptionsAndContext();
            }
        }

        public virtual void AddMessage(string queueName, string message, TimeSpan? timeToLive = null, TimeSpan? initialVisibiltyDelay = null, bool createIfNotExists = false)
        {
            AddMessage(queueName, new CloudQueueMessage(message), timeToLive, initialVisibiltyDelay, createIfNotExists);
        }

        public virtual void AddMessage(string queueName, byte[] message, TimeSpan? timeToLive = null, TimeSpan? initialVisibiltyDelay = null, bool createIfNotExists = false)
        {
            AddMessage(queueName, new CloudQueueMessage(message), timeToLive, initialVisibiltyDelay, createIfNotExists);
        }

        public virtual void AddMessage(string queueName, CloudQueueMessage message, TimeSpan? timeToLive = null, TimeSpan? initialVisibiltyDelay = null, bool createIfNotExists = false)
        {
            try
            {
                var queue = GetQueueAndCreateIfNotExists(queueName, createIfNotExists);
                queue.AddMessage(message, timeToLive, initialVisibiltyDelay, QueueRequestOptions, OperationContext);
            }
            finally
            {
                ResetOptionsAndContext();
            }
        }

        public virtual CloudQueueMessage PeekNextMessage(string queueName, bool createIfNotExists = false)
        {
            try
            {
                var queue = GetQueueAndCreateIfNotExists(queueName, createIfNotExists);
                var message = queue.PeekMessage(QueueRequestOptions, OperationContext);

                return message;
            }
            finally
            {
                ResetOptionsAndContext();
            }
        }

        public virtual CloudQueueMessage DequeueNextMessage(string queueName, bool createIfNotExists = false, TimeSpan? visibiltyTimeout = null)
        {
            try
            {
                var queue = GetQueueAndCreateIfNotExists(queueName, createIfNotExists);
                var message = queue.GetMessage(visibiltyTimeout ?? _defaultVisibilityTimeout, QueueRequestOptions, OperationContext);
                queue.DeleteMessage(message, QueueRequestOptions, OperationContext);
                return message;
            }
            finally
            {
                ResetOptionsAndContext();
            }
        }

        public virtual int? GetApproximateMessageCount(string queueName, bool createIfNotExists = false)
        {
            try
            {
                var queue = GetQueueAndCreateIfNotExists(queueName, createIfNotExists);
                queue.FetchAttributes();
                return queue.ApproximateMessageCount;
            }
            finally
            {
                ResetOptionsAndContext();
            }

        }

        public virtual bool DeleteQueue(string queueName)
        {
            try
            {
                var queue = GetQueueAndCreateIfNotExists(queueName);
                return queue.DeleteIfExists(QueueRequestOptions, OperationContext);
            }
            finally
            {
                ResetOptionsAndContext();
            }
        }
    }
}
