using LibrarySystem.Core.Entitties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.Services.Contract
{
    public interface IAuthorService
    {
        Task<Auther> ExistsAuthorAsync(string fullName);
        
    }
}
