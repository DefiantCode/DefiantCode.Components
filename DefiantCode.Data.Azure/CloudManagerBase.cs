using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace DefiantCode.Data.AzureStorage
{
    public class CloudManagerBase
    {
        private Tuple<QueueRequestOptions, bool> _queueRequestOptions;
        private Tuple<OperationContext, bool> _operationContext;

        protected QueueRequestOptions QueueRequestOptions
        {
            get { return _queueRequestOptions == null ? null : _queueRequestOptions.Item1; }
        }

        protected OperationContext OperationContext
        {
            get { return _operationContext == null ? null : _operationContext.Item1; }
        }

        /// <summary>
        /// Sets the QueueRequestOption for requests to the storage account.
        /// </summary>
        /// <param name="queueRequestOptions">The QueueRequestOption object to set</param>
        /// <param name="makeGlobal">Enables this QueueRequestOption globally for every request for the lifetime of the current CloudManager instance. If set to false, it will only be used for the next request, then cleared.</param>
        public virtual void SetRequestOptions(QueueRequestOptions queueRequestOptions, bool makeGlobal = false)
        {
            _queueRequestOptions = new Tuple<QueueRequestOptions, bool>(queueRequestOptions, makeGlobal);
        }

        /// <summary>
        /// Sets the OperationContext for requests to the storage account.
        /// </summary>
        /// <param name="operationContext">The OperationContext object to set</param>
        /// <param name="makeGlobal">Enables this OperationContext globally for every request for the lifetime of the current CloudManager instance. If set to false, it will only be used for the next request, then cleared.</param>
        public virtual void SetOperationContext(OperationContext operationContext, bool makeGlobal = false)
        {
            _operationContext = new Tuple<OperationContext, bool>(operationContext, makeGlobal);
        }

        public virtual void ResetOptionsAndContext()
        {
            if (_queueRequestOptions.Item2 == false)
                _queueRequestOptions = null;

            if (_operationContext.Item2 == false)
                _operationContext = null;
        }
    }
}