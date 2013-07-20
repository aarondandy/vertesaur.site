using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using MarkdownSharp;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace Vertesaur.Site
{

    [Route("/docs/{Slug}", "GET")]
    public class WikiDocsSlugRequest
    {
        public string Slug { get; set; }
    }

    public class WikiDocsSlugResponse
    {
        public string Title { get; set; }
        public HtmlString Body { get; set; }
    }

    public class WikiDocsService : Service
    {

        public IHttpResult Any(WikiDocsSlugRequest request) {
            var directory = new DirectoryInfo(HostingEnvironment.MapPath("~/docs/wiki/Content"));
            var fileName = request.Slug;
            if (!fileName.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
                fileName += ".md";

            var fileInfo = directory.GetFiles(fileName).FirstOrDefault(x => String.Equals(x.Name, fileName, StringComparison.OrdinalIgnoreCase));
            if (fileInfo == null)
                throw new HttpError(HttpStatusCode.NotFound, "Content not found.");

            var fullMarkdownPath = fileInfo.FullName;
            var markdown = new Markdown();
            var html = markdown.Transform(File.ReadAllText(fullMarkdownPath));

            var response = new WikiDocsSlugResponse();
            response.Body = new HtmlString(html);
            response.Title = Path.GetFileNameWithoutExtension(fileInfo.Name).Replace('-',' ');

            return new HttpResult(response) {
                View = "_WikiDocsLayout"
            };
        }

    }
}