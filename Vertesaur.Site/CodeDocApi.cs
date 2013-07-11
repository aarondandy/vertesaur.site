using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Web;
using DandyDoc.CRef;
using DandyDoc.CodeDoc;
using ServiceStack.Common.Web;

namespace Vertesaur.Site
{

    public class CodeDocCRefRequest
    {
        public string CRef { get; set; }
    }

    public class CodeDocApi
    {

        public CodeDocRepositories Repositories { get; set; }

        public CodeDocSimpleMember Any(CodeDocCRefRequest request) {
            if (request == null) throw new ArgumentNullException("request");
            if (String.IsNullOrEmpty(request.CRef)) throw new ArgumentException("Code reference (CRef) not provided.", "request");
            Contract.EndContractBlock();

            var model = Repositories.GetModel(new CRefIdentifier(request.CRef));
            if (model == null)
                throw new HttpError(HttpStatusCode.NotFound, "Documentation model could not be found.");
            return model;
        }

    }
}