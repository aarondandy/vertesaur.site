using System;
using System.Diagnostics.Contracts;
using System.Net;
using DandyDoc.CRef;
using DandyDoc.CodeDoc;
using ServiceStack.Common.Web;
using ServiceStack.Html;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace Vertesaur.Site
{

    [Api("Provides documentation models.")]
    [Route("/docs/api")]
    public class CodeDocCRefRequest
    {
        public string CRef { get; set; }
    }

    public class CodeDocApi : Service
    {

        public CodeDocRepositories Repositories { get; set; }

        public IHttpResult Any(CodeDocCRefRequest request) {
            if (request == null) throw new ArgumentNullException("request");
            Contract.EndContractBlock();

            if (String.IsNullOrEmpty(request.CRef))
                return this.Redirect(new UrlHelper().Content("~/docs/api/namespaces"));

            var model = Repositories.GetModelFromTarget(new CRefIdentifier(request.CRef)) as CodeDocSimpleMember;
            if (model == null)
                throw new HttpError(HttpStatusCode.NotFound, "Documentation model could not be found.");

            return new HttpResult(model);
        }

    }
}