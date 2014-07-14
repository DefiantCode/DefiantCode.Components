using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace DefiantCode.Data.AzureStorage
{
    public class CloudBlobManager
    {
        public virtual CloudBlobClient BlobClient { get; private set; }
        public virtual CloudBlobContainer Container { get; private set; }

        public CloudBlobManager(CloudStorageAccount storageAccount, string containerName, BlobContainerPublicAccessType containerAccess = BlobContainerPublicAccessType.Off)
            : this(storageAccount.CreateCloudBlobClient(), containerName, containerAccess)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
        {
        }

        public CloudBlobManager(CloudBlobClient blobClient, string containerName, BlobContainerPublicAccessType containerAccess = BlobContainerPublicAccessType.Off)
        {
            BlobClient = blobClient;
            var container = blobClient.GetContainerReference(containerName);
            container.CreateIfNotExists();
            Container = container;
            SetPublicAccess(containerAccess);
        }

        public virtual void SetPublicAccess(BlobContainerPublicAccessType access)
        {
            Container.SetPermissions(new BlobContainerPermissions{ PublicAccess = access });   
        }

        public virtual CloudBlockBlob GetBlockBlobReference(string blobName)
        {
            return Container.GetBlockBlobReference(blobName);
        }

        public virtual void UploadBlockBlob(string blobName, Stream streamToUpload, string contentType, AccessCondition accessCondition = null)
        {
            var blockBlob = GetBlockBlobReference(blobName);
            blockBlob.Properties.ContentType = contentType;
            
            blockBlob.UploadFromStream(streamToUpload, accessCondition);
        }

        public virtual void UploadBlockBlob(CloudBlockBlob blockBlob, Stream streamToUpload, AccessCondition accessCondition = null)
        {
            blockBlob.UploadFromStream(streamToUpload, accessCondition);
        }

    }
}
