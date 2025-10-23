using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;


namespace Podcast.Controller
{
    public class S3PodcastsService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3PodcastsService(IAmazonS3 s3Client, string bucketName)
        {
            _s3Client = s3Client;
            _bucketName = bucketName;
        }

        // Create/Upload
        public async Task<string> UploadAsync(string key, Stream content, string contentType = "application/octet-stream") {
            var request = new PutObjectRequest { 
                BucketName = _bucketName,
                Key = key,
                ContentType = contentType,
                InputStream = content
            };
            var response = await _s3Client.PutObjectAsync(request);
            return response.ETag;
        }

        //Read/Download
        public async Task<Stream> DownloadAsync(string key) {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };
            var response = await _s3Client.GetObjectAsync(request);
            return response.ResponseStream;
        }

        //Update
        public Task<string> UpdateAsunc(string key, Stream content, string contentType = "application/octet-stream") => UploadAsync(key, content, contentType);

        //Delete
        public async Task DeleteAsync(string key) {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };
            await _s3Client.DeleteObjectAsync(request);
        }

        //List All
        public async Task<List<string>> ListAsync(string prefix = "") {
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = prefix
            };
            var response = await _s3Client.ListObjectsV2Async(request);
            return response.S3Objects.Select(obj => obj.Key).ToList();
        }
    }
}
