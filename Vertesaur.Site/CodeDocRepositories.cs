using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;
using DandyDoc.CRef;
using DandyDoc.CodeDoc;
using DandyDoc.XmlDoc;

namespace Vertesaur.Site
{
    public class CodeDocRepositories
    {

        static CodeDocRepositories() {
            // this is needed so reflection only load can find the other assemblies
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += ReflectionOnlyResolveEventHandler;
        }

        private static Assembly ReflectionOnlyResolveEventHandler(object sender, ResolveEventArgs args) {
            var assemblyName = new AssemblyName(args.Name);
            var binPath = HostingEnvironment.MapPath(String.Format("~/bin/{0}.dll", assemblyName.Name));
            if (File.Exists(binPath))
                return Assembly.ReflectionOnlyLoadFrom(binPath);
            return Assembly.ReflectionOnlyLoad(args.Name);
        }

        public CodeDocRepositories() {

            SupportingRepository = new CodeDocRepositoryFailureProtectionWrapper(new MsdnCodeDocMemberRepository(), new TimeSpan(0, 0, 10));
            TargetRepository = new ReflectionCodeDocMemberRepository(
                new ReflectionCRefLookup(
                    typeof(Point2).Assembly,//Assembly.ReflectionOnlyLoadFrom(HostingEnvironment.MapPath("~/bin/Vertesaur.Core.dll")),
                    typeof(Generation.Expressions.AbsExpression).Assembly //Assembly.ReflectionOnlyLoadFrom(HostingEnvironment.MapPath("~/bin/Vertesaur.Generation.dll"))
                ),
                new XmlAssemblyDocument(HostingEnvironment.MapPath("~/bin/bin/Vertesaur.Core.XML")),
                new XmlAssemblyDocument(HostingEnvironment.MapPath("~/bin/bin/Vertesaur.Generation.XML"))
            );
        }

        public ICodeDocMemberRepository TargetRepository { get; private set; }

        public ICodeDocMemberRepository SupportingRepository { get; private set; }

        private IEnumerable<ICodeDocMemberRepository> GetAllRepositories() {
            return new[] {TargetRepository, SupportingRepository}.Where(x => x != null);
        }

        public ICodeDocMember GetModelFromTarget(CRefIdentifier cRef, CodeDocMemberDetailLevel detailLevel = CodeDocMemberDetailLevel.Full) {
            if (cRef == null) throw new ArgumentNullException("cRef");
            Contract.EndContractBlock();
            return new CodeDocRepositorySearchContext(GetAllRepositories(), detailLevel)
                .CloneWithOneUnvisited(TargetRepository)
                .Search(cRef);
        }

        public ICodeDocMember GetModelFromAny(CRefIdentifier cRef, CodeDocMemberDetailLevel detailLevel = CodeDocMemberDetailLevel.Full) {
            if (cRef == null) throw new ArgumentNullException("cRef");
            Contract.EndContractBlock();
            return new CodeDocRepositorySearchContext(GetAllRepositories(), detailLevel).Search(cRef);
        }


    }
}