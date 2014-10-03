using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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

        public async virtual void UploadBlockBlobAsync(CloudBlockBlob blockBlob, Stream streamToUpload, AccessCondition accessCondition = null)
        {
            await blockBlob.UploadFromStreamAsync(streamToUpload, accessCondition, null, null);
        }

        public async virtual void UploadBlockBlobAsync(string blobName, Stream streamToUpload, string contentType, AccessCondition accessCondition = null)
        {
            var blockBlob = GetBlockBlobReference(blobName);
            blockBlob.Properties.ContentType = contentType;

            await blockBlob.UploadFromStreamAsync(streamToUpload, accessCondition, null, null);
        }

        public virtual string DownloadAsString(string blobName, Encoding encoding = null)
        {
            var blockBlob = GetBlockBlobReference(blobName);
            return blockBlob.DownloadText(encoding ?? Encoding.UTF8);
        }

        public async virtual Task<String> DownloadAsStringAsync(string blobName)
        {
            var blockBlob = GetBlockBlobReference(blobName);
            return await blockBlob.DownloadTextAsync();
        }

        public virtual byte[] DownloadAsBytes(string blobName)
        {
            var blockBlob = GetBlockBlobReference(blobName);
            blockBlob.FetchAttributes();
            var fileByteLength = blockBlob.Properties.Length;
            var fileContent = new byte[fileByteLength];
            blockBlob.DownloadToByteArray(fileContent, 0);
            return fileContent;
        }

        public async virtual Task<byte[]> DownloadAsBytesAsync(string blobName)
        {
            var blockBlob = GetBlockBlobReference(blobName);
            await blockBlob.FetchAttributesAsync();
            var fileByteLength = blockBlob.Properties.Length;
            var fileContent = new byte[fileByteLength];
            await blockBlob.DownloadToByteArrayAsync(fileContent, 0);
            return fileContent;
        }

        public virtual void DownloadAsStream(string blobName, Stream targetStream)
        {
            var blockBlob = GetBlockBlobReference(blobName);
            blockBlob.DownloadToStream(targetStream);
        }

        public async virtual Task DownloadAsStreamAsync(string blobName, Stream targetStream)
        {
            var blockBlob = GetBlockBlobReference(blobName);
            await blockBlob.DownloadToStreamAsync(targetStream);
        }

    }
}
