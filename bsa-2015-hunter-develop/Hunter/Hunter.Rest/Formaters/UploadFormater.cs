using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Hunter.Services;
using Newtonsoft.Json;

namespace Hunter.Rest.Formaters
{
    public class UploadFormater : MediaTypeFormatter
    {
        public UploadFormater()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/form-data"));
        }

        public override bool CanReadType(Type type)
        {
            if (type == typeof (FileDto))
            {
                return true;
            }
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return false;
        }

        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content,
            IFormatterLogger formatterLogger,
            CancellationToken cancellationToken)

        {
            if (!content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var fileDto = new FileDto();
            var parts = await content.ReadAsMultipartAsync(cancellationToken);
            //file content
            var fileContent = parts.Contents.FirstOrDefault(x => x.Headers.ContentDisposition.Name == "\"file\"");
            //file information
            var data = parts.Contents.FirstOrDefault(x => x.Headers.ContentDisposition.Name == "\"data\"");
            var dataStr = await data.ReadAsStringAsync();
            fileDto = JsonConvert.DeserializeObject<FileDto>(dataStr);

            fileDto.File = await fileContent.ReadAsStreamAsync();

            return fileDto;

        }
    }
}