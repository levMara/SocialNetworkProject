using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System.Diagnostics;

namespace WebApplication4.BL
{
    public class AmazonBucketClient
    {
        private const string bucketName = "saedambucket1";

        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast2;

        string hostUrl = "https://s3.us-east-2.amazonaws.com/";

        private static IAmazonS3 s3Client=null;

        public AmazonBucketClient()
        {
            s3Client = new AmazonS3Client(bucketRegion);
        }
        
        public string UploadFile(Stream inputStream, string FileKey)
        {
            using (s3Client)
            {
                var request = new PutObjectRequest();
                request.BucketName = bucketName;
                request.Key = FileKey;
                request.CannedACL = S3CannedACL.PublicReadWrite;

                request.InputStream = inputStream;
                s3Client.PutObject(request);

                return Path.Combine(hostUrl, bucketName, FileKey);

            }
        }
    }
}