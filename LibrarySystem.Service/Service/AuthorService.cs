using LibrarySystem.Core.Entitties;
using LibrarySystem.Core.Repositories.Contract;
using LibrarySystem.Core.Services.Contract;
using LibrarySystem.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Service.Service
{
    public class AuthorService :IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Auther> ExistsAuthorAsync(string fullName)
        {
            return await _unitOfWork.Repository<Auther>().GetByEntitySpecAsync(new ExistsSpecification<Auther>(fullName));
        }
    }
}
