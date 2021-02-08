using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Persistence.Repositories.Interfaces;
using WebAPI.Dtos.UserDtos;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryFactory _repository;

        public UserController(IMapper mapper, IRepositoryFactory repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        // GET: User/
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = _repository.Users.GetAll(track:false);
            try
            {
                users = _repository.Users.Search(Request.Query, users);
                users = _repository.Users.Sort(Request.Query, users);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            var pagedUsers = await _repository.Users.GetPage(Request.Query, users);
            var usersDto = _mapper.Map<IEnumerable<UserReadDto>>(pagedUsers);

            Response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
            Response.Headers.Add("Pagination", JsonConvert.SerializeObject(pagedUsers.Metadata));

            return Ok(usersDto);
        }

        // GET: User/5
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _repository.Users.GetAsync(id, track:false);
            var userDto = _mapper.Map<UserReadDto>(user);
            
            return Ok(userDto);
        }


        // POST: Users
        [HttpPost]
        public async Task<IActionResult> Post(UserUpdateDto userDto)
        {
            if (userDto == null)
                return BadRequest();

            var user = _mapper.Map<User>(userDto);
            _repository.Users.Add(user);
            await _repository.SaveChangesAsync();

            return Created("created", _mapper.Map<UserReadDto>(user));
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put(int id, UserUpdateDto userDto)
        {
            if (id != userDto.Id)
                return BadRequest();

            if (_repository.Users.Exists(u => u.Username.Equals(userDto.Username) && u.Id != id))
            {
                ModelState.AddModelError("Username", "Username not available.");
                return ValidationFailed(ModelState);
            }

            var user = await _repository.Users.FindAsync(x => x.Id == id, track:true);
            _mapper.Map(userDto, user);

            await _repository.SaveChangesAsync();

            return Ok(_mapper.Map<UserReadDto>(user));
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> Patch(int id, JsonPatchDocument<UserUpdateDto> docPatch)
        {
            var storedUser = await _repository.Users.FindAsync(x => x.Id == id, track: true);
            var userToBePatched = _mapper.Map<UserUpdateDto>(storedUser);
            
            docPatch.ApplyTo(userToBePatched, ModelState);
            TryValidateModel(userToBePatched);

            if (!ModelState.IsValid)
                return ValidationFailed(ModelState);

            _mapper.Map(userToBePatched, storedUser);
            await _repository.SaveChangesAsync();

            return Ok(_mapper.Map<UserReadDto>(storedUser));
        }

        // DELETE: User/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _repository.Users.GetAsync(id);

            _repository.Users.Delete(user);
            await _repository.SaveChangesAsync();

            return Ok(_mapper.Map<UserReadDto>(user));
        }
    }
}
