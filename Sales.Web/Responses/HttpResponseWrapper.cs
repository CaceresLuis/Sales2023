using System.Net;

namespace Sales.Web.Responses
{
    public class HttpResponseWrapper<T>
    {
        public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
        {
            Error = error;
            Response = response;
            HttpResponseMessage = httpResponseMessage;
        }

        public bool Error { get; set; }

        public T? Response { get; set; }

        public HttpResponseMessage HttpResponseMessage { get; set; }

        public async Task<string?> GetErrorMessageAsync()
        {
            if (!Error)
            {
                return null;
            }

            var statusCode = HttpResponseMessage.StatusCode;
            switch (statusCode)
            {
                case HttpStatusCode.NotFound:
                    return "Recurso no encontrado";
                case HttpStatusCode.BadRequest:
                    return await HttpResponseMessage.Content.ReadAsStringAsync();
                case HttpStatusCode.Unauthorized:
                    return "Tienes que logearte para hacer esta operación";
                case HttpStatusCode.Forbidden:
                    return "No tienes permisos para hacer esta operación";
                case HttpStatusCode.InternalServerError:
                    return await HttpResponseMessage.Content.ReadAsStringAsync();
                default:
                    return "Ha ocurrido un error inesperado";
            }
        }
    }

}
