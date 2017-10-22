using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Riverside.Utilities.Data;

namespace Riverside.Cms.Elements.CodeSnippets
{
    public interface ICodeSnippetRepository
    {
        void Create(CodeSnippetSettings settings, IUnitOfWork unitOfWork = null);
        void Copy(long sourceTenantId, long sourceElementId, long destTenantId, long destElementId, IUnitOfWork unitOfWork = null);
        void Read(CodeSnippetSettings settings, IUnitOfWork unitOfWork = null);
        void Update(CodeSnippetSettings settings, IUnitOfWork unitOfWork = null);
        void Delete(long tenantId, long elementId, IUnitOfWork unitOfWork = null);
    }
}
