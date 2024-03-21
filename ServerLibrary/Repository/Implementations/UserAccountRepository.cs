using BaseLibrary.Dtos;
using BaseLibrary.Responses;
using ServerLibrary.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Repository.Implementations
{
    internal class UserAccountRepository : IUserAccount
    {
        public Task<GeneralRespose> CreateAsync(Register user)
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponse> SingInAsync(Login user)
        {
            throw new NotImplementedException();
        }
    }
}
