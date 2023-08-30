using System.Net;

namespace Sales.Frontend.Repositories
{
    public class HttpResponseWrapper<T>
    {
        public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
        {
            Error = error;
            Response = response;
            HttpResponseMessage = httpResponseMessage;
        }

        public bool Error { get; private set; }

        public T? Response { get; private set; }
        
        public HttpResponseMessage HttpResponseMessage { get; set; }

        public async Task<string?> GetErrorMessageAsync()
        {
            if(!Error) 
            { 
                return null;
            }
             
            var statusCode= HttpResponseMessage.StatusCode;
            if(statusCode == HttpStatusCode.NotFound)
            {
                return "recurso no encontrado";
            }
            else if (statusCode==HttpStatusCode.BadRequest)
            {
                return await HttpResponseMessage.Content.ReadAsStringAsync();
            }
            else if(statusCode == HttpStatusCode.Unauthorized) 
            {
                return "tienes que loguearte para hacer esta operacion";
            }
            else if (statusCode == HttpStatusCode.Forbidden)
            {
                return "no tienes permisos para hacer esta operacion";
            }
            return "ha ocurrido un error inesperado";
        }

    }
}
