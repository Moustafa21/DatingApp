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
public class UserController (IUserRepository userRepository, IMapper mapper, IPhotoService photoService): BaseAPIController
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
        var user = await userRepository.GetUsersByUsernamedAsync(User.GetUsername());

        if(user == null)
            return BadRequest("Couldn't find username");
        
        mapper.Map(memberUpdateDto, user);

        if(await userRepository.SaveAllAsync()) 
            return NoContent();
        
        return BadRequest("Failed to update the user");

    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var user = await userRepository.GetUsersByUsernamedAsync(User.GetUsername());
        
        if (user == null) return BadRequest("Cannot Get User");

        var result = await photoService.AddPhotoAsync(file);
        
        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo{
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        if(user.Photos.Count == 0)
            photo.IsMain = true;

        user.Photos.Add(photo);
        if (await userRepository.SaveAllAsync()) 
        {
            return CreatedAtAction(nameof(GetUser),
             new {username = user.UserName}, 
              mapper.Map<PhotoDto>(photo)
            );

        }        
        return BadRequest("Problem while Adding Image");
    }

    [HttpPut("set-main-photo/{photoId:int}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user = await userRepository.GetUsersByUsernamedAsync(User.GetUsername());

        if(user == null) return BadRequest("Could not find user");

        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

        if (photo == null || photo.IsMain) return BadRequest("Cannot use this as main photo");

        var currentMain = user.Photos.FirstOrDefault(p => p.IsMain);
        if(currentMain != null) 
            currentMain.IsMain = false ;
        
        photo.IsMain = true ;

        if(await userRepository.SaveAllAsync())
            return NoContent();
        return BadRequest("Problem Setting phto");
    }

    [HttpDelete("delete-photo/{photoId:int}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await userRepository.GetUsersByUsernamedAsync(User.GetUsername());

        if(user == null) return BadRequest("Could not find user");

        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

        if(photo == null || photo.IsMain)  return BadRequest("Cannot Delete this photo");

        if(photo.PublicId != null){
            var result = await photoService.DeletehotoAsync(photo.PublicId);
            if(result.Error != null) return BadRequest(result.Error.Message);
        }
        user.Photos.Remove(photo);

        if(await userRepository.SaveAllAsync()) return Ok();
        return BadRequest("Problem Setting phto");

    }
}
