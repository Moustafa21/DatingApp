using System.Security.Cryptography;
using System.Text;
using API.Controllers;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService, IMapper mapper) : BaseAPIController
{
 
    [HttpPost("register")] 
    public async Task<ActionResult<UserDTO>> Register([FromBody]RegisterDTO registerDTO)
    {
        if (await UserExist(registerDTO.Username) )
            return BadRequest("User is taken");
        
        using var hmac = new HMACSHA512();
        var user = mapper.Map<AppUser>(registerDTO);

        user.UserName = registerDTO.Username.ToLower();
        user.PasswordHash =hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.password));
        user.PasswordSalt = hmac.Key;

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new UserDTO
        {
            UserName = user.UserName,
            TokenKey = tokenService.CreateToken(user),
            KnownAs = user.KnownAs,
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
    {
        var user = await context.Users
        .Include(p => p.Photos)
            .FirstOrDefaultAsync(x=>
                x.UserName == loginDTO.Username.ToLower());
        if (user == null)
            return Unauthorized("Invalid Username");
        
        using var hmac = new HMACSHA512(user.PasswordSalt);

        var ComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.password));
        for(int i = 0; i < ComputedHash.Length; i++)
        {
            if(ComputedHash[i] != user.PasswordHash[i])
                return Unauthorized("Invalid Password");
            
        }
        return new UserDTO{
            UserName = user.UserName,
            TokenKey = tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            KnownAs = user.KnownAs,
        };
    }



    private async Task<bool> UserExist(string username){
        return  await context.Users.AnyAsync(x=>x.UserName.ToLower() == username.ToLower());
    }

}
