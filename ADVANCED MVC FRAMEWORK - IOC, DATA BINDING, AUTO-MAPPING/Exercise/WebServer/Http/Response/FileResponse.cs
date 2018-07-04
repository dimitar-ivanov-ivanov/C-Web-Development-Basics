using WebServer.Enums;
using WebServer.Exceptions;

namespace WebServer.Http.Response
{
    public class FileResponse : HttpResponse
    {
        public FileResponse(HttpStatusCode statusCode, byte[] fileData, string mimeType)
        {
            this.EnsureValidStatusCode(statusCode);

            this.FileData = fileData;
            this.StatusCode = statusCode;

            this.Headers.Add(HttpHeader.ContentLength, this.FileData.Length.ToString());
            this.Headers.Add(HttpHeader.ContentDisposition, "attachment");
            this.Headers.Add(HttpHeader.ContentType, mimeType);

        }

        public byte[] FileData { get; }

        private void EnsureValidStatusCode(HttpStatusCode statusCode)
        {
            int statusCodeNumber = (int)statusCode;
            bool isValidResponseCode = statusCodeNumber >= 200 && statusCodeNumber < 400;

            if (!isValidResponseCode)
            {
                throw new InvalidResponseException("File response need a redirect-type status code.");
            }
        }
    }
}
