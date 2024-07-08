using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
[Authorize]
public class UserController (IUserRepository userRepository, IMapper mapper): BaseAPIController
{


    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
    {
        var users = await userRepository.GetMemberAsync();

       
        return Ok(users);
    }

    
    [HttpGet("{username}")]
    public  async Task<ActionResult<MemberDTO>> GetUser(string username)
    {
        var user = await userRepository.GetMemberAsync(username);
        if (user == null)
            return NotFound();
        return  user;
        
    }
    [HttpPut]
    public async Task<ActionResult> EditUser(MemberUpdateDto memberUpdateDto)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (username == null) 
            return BadRequest("No username found");
        
        var user = await userRepository.GetUsersByUsernamedAsync(username);

        if(user == null)
            return BadRequest("Couldn't find username");
        
        mapper.Map(memberUpdateDto, user);

        if(await userRepository.SaveAllAsync()) 
            return NoContent();
        
        return BadRequest("Failed to update the user");

    }
}
