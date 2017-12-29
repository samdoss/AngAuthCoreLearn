using Jil;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBoardAuth.Api.Formatters
{
    public class JilOutputFormatter : IOutputFormatter
    {
        public bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (context.ContentType == null
                || context.ContentType.ToString() == "application/json")
                return true;
            return false;
        }
        public async Task WriteAsync(OutputFormatterWriteContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";
            using (var writer = context.WriterFactory(response.Body, Encoding.UTF8))
            {
                Jil.JSON.Serialize(context.Object, writer, new Options(
                    prettyPrint: false,
                    excludeNulls: true,
                    serializationNameFormat: SerializationNameFormat.CamelCase
                ));
                await writer.FlushAsync();
            }
        }
    }
}
