using BaseLibrary.Dtos;
using BaseLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Repository.Contracts
{
    public interface IUserAccount
    {

        Task<GeneralRespose> CreateAsync(Register user);
        Task<LoginResponse> SingInAsync(Login user);


        Task<LoginResponse> RefreshTokenAsync(RefreshToken token);

    }
}
