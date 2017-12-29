using Jil;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebBoardAuth.Api.Formatters
{
    public class JilInputFormatter : IInputFormatter
    {
        public bool CanRead(InputFormatterContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var contentType = context.HttpContext.Request.ContentType;
            if (contentType == null
                || contentType == "application/json")
                return true;
            return false;
        }
        public Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            return InputFormatterResult.SuccessAsync(null);
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var request = context.HttpContext.Request;
            if (request.ContentLength == 0)
            {
                if (context.ModelType.GetTypeInfo().IsValueType)
                    return InputFormatterResult.SuccessAsync(Activator.CreateInstance(context.ModelType));
                else return InputFormatterResult.SuccessAsync(null);
            }
            var encoding = Encoding.UTF8;//do we need to get this from the request im not sure yet
            using (var reader = new StreamReader(context.HttpContext.Request.Body))
            {
                var model = Jil.JSON.Deserialize(reader, context.ModelType, new Options(
                    prettyPrint: false,
                    serializationNameFormat: SerializationNameFormat.CamelCase
                ));
                return InputFormatterResult.SuccessAsync(model);
            }
        }
    }
}
