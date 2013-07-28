using System.Net;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace Vertesaur.Site
{

    [Route("/docs", "GET")]
    [Route("/docs/{Slug}", "GET")]
    public class WikiDocsSlugRequest
    {
        public string Slug { get; set; }
    }

    public class WikiDocsService : Service
    {
        public IHttpResult Any(WikiDocsSlugRequest request) {
            var response = WikiDocs.GetWikiContent(request.Slug);
            if(response == null)
                throw new HttpError(HttpStatusCode.NotFound, "Content not found.");

            return new HttpResult(response) {
                View = "_WikiDocsLayout"
            };
        }
    }
}