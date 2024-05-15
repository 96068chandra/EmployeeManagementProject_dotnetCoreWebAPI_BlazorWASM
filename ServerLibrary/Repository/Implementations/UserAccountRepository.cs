using BaseLibrary.Dtos;
using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServerLibrary.Data;
using ServerLibrary.Helpers;
using ServerLibrary.Repository.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ServerLibrary.Repository.Implementations
{
    public class UserAccountRepository(IOptions<JwtSection> config,ApplicationDbContext applicationDbContext) : IUserAccount
    {
        public async Task<GeneralRespose> CreateAsync(Register user)
        {
            if(user == null)
            {

                return new GeneralRespose(false, "Model Is Empty");

            }
            var checkUser = await FindUserByEmail(user.Email);
            if(checkUser != null) 
            {
                return new GeneralRespose(false, "User Already Registered");
            }
            //Save user
            var ApplicationUser = await AddTODatabase(new ApplicationUser(){
                FullName = user.FullName,
                Email = user.Email,
                Password=BCrypt.Net.BCrypt.HashPassword(user.Password)   

            });

            //Check and Assign roles

            var checkAdminROle=await applicationDbContext.SystemRoles.FirstOrDefaultAsync(s => s.Name!.Equals(Constants.Admin));
            if(checkAdminROle is null)
            {
                var createAdminRole = await AddTODatabase(new SystemRole() { Name = Constants.Admin });
                await AddTODatabase(new UserRole() { RoleId=createAdminRole.Id,UserId=ApplicationUser.Id});
                return new GeneralRespose(true, "Account Created");

            }
            //check userrole
            var checkUserRole = await applicationDbContext.SystemRoles.FirstOrDefaultAsync(s => s.Name!.Equals(Constants.User));
            SystemRole systemRole = new();
            if(checkUserRole is null)
            {
                var createUserRole = await AddTODatabase(new SystemRole() { Name = Constants.User });
                await AddTODatabase(new UserRole() { RoleId=createUserRole.Id,UserId =ApplicationUser.Id});


            }
            else
            {
                await AddTODatabase(new UserRole() { RoleId= checkUserRole.Id,UserId=ApplicationUser.Id});

            }
            return new GeneralRespose(true, "Account is created");
        }
        
        private async Task<ApplicationUser> FindUserByEmail(string? email)=>
             await applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(x=>x.Email!.ToLower()!.Equals(email!.ToLower()));
        

        public async Task<LoginResponse> SingInAsync(Login user)
        {
            if(user is null)
            {
                return new LoginResponse(false, "Model is Empty");
            }

            var applicationUser = await FindUserByEmail(user.Email);
            if(applicationUser is null)
            {
                return new LoginResponse(false, "User Not Found");
            }
            //Verify password
            if(!BCrypt.Net.BCrypt.Verify(user.Password,applicationUser.Password))
            {
                return new LoginResponse(false,"Email/Password is not Valid");
            }
            var getUserRole = await FindUserRole(applicationUser.Id);
            if(getUserRole is null)
            {
                return new LoginResponse(false, "User role not found");
            }
            var getRoleName = await FindRoleName(getUserRole.RoleId);
            if(getRoleName is null)
            {
                return new LoginResponse(false, "User Role not found");
            }
            string jwtToken = GenerateToken(applicationUser, getRoleName!.Name!);
            string refreshToken = GenerateRefreshToken();
            var findUser = await applicationDbContext.RefreshTokenInfos.FirstOrDefaultAsync(t => t.UserId == applicationUser.Id);
            if(findUser is  not null)
            {
                findUser!.Token = refreshToken;
                await applicationDbContext.SaveChangesAsync();

            }
            else
            {
                await AddTODatabase(new RefreshTokenInfo()
                {
                    Token = refreshToken,
                    UserId = applicationUser.Id,
                });
            }
            return new LoginResponse(true,"Login Sucessfully",jwtToken,refreshToken);

        }

        private string GenerateToken(ApplicationUser user,string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Value.Key!));
            var credentals=new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.FullName!),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.Role,role),



            };
            var token = new JwtSecurityToken(
                issuer: config.Value.Issuer,
                audience: config.Value.Audience,
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentals
                );
            return new JwtSecurityTokenHandler().WriteToken(token);


        }

        private async Task<UserRole> FindUserRole(int userid) => await applicationDbContext.UserRoles.FirstOrDefaultAsync(p => p.UserId == userid);
        private async Task<SystemRole> FindRoleName(int roleid) => await applicationDbContext.SystemRoles.FirstOrDefaultAsync(p => p.Id == roleid);

            private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        private async Task<T> AddTODatabase<T>(T model)
        {
            var result = applicationDbContext.Add(model!);
            await applicationDbContext.SaveChangesAsync();
            return (T)result.Entity;
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshToken token)
        {
            if (token is null) return new LoginResponse(false,"Model is empty");

            var findToken=await applicationDbContext.RefreshTokenInfos.FirstOrDefaultAsync(t=>t.Token!.Equals(token.Token));
            if (findToken is null) return new LoginResponse(false, "Refresh token required");

            var user= await applicationDbContext.ApplicationUsers.FirstOrDefaultAsync(_=>_.Id==findToken.UserId);
            if (user is null) return new LoginResponse(false, "Refresh token could not be generated because user not found");

            var userRole =await FindUserRole(user.Id);
            var roleName=await FindRoleName(userRole.RoleId);
            string jwtToken =  GenerateToken(user, roleName.Name!);
            string refreshToken = GenerateRefreshToken();
            var updateRefreshToken=await applicationDbContext.RefreshTokenInfos.FirstOrDefaultAsync(t=>t.UserId==user.Id);
            if (updateRefreshToken is null) return new LoginResponse(false, "Refresh token could not be genertated because user is not logged in");
            updateRefreshToken.Token = refreshToken;
            await applicationDbContext.SaveChangesAsync();
            return new LoginResponse(true, "Token refreshed sucessfully",jwtToken,refreshToken);


        }
    }
}
