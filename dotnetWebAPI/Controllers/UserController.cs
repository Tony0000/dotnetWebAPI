using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Persistence.Repositories.Interfaces;
using WebAPI.ActionFilter;
using WebAPI.Dtos.UserDtos;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ServiceFilter(typeof(NotFoundFilter<User>))]
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
        public async Task<IActionResult> GetAllAsync()
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
        public async Task<IActionResult> GetAsync(int id)
        {
            var user = await _repository.Users.GetAsync(id, track:false);
            var userDto = _mapper.Map<UserReadDto>(user);
            
            return Ok(userDto);
        }

        // POST: Users
        [HttpPost]
        public async Task<IActionResult> PostAsync(UserCreateDto userDto, 
            CancellationToken cancellationToken = default)
        {
            if (userDto == null)
                return BadRequest();

            var user = _mapper.Map<User>(userDto);
            _repository.Users.Add(user);
            await _repository.SaveChangesAsync(cancellationToken);

            return CreatedAtAction("Get", new {id = user.Id}, _mapper.Map<UserReadDto>(user));
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PutAsync(int id, 
            UserUpdateDto userDto, 
            CancellationToken cancellationToken = default)
        {
            if (id != userDto.Id)
                return BadRequest();

            if (_repository.Users.Exists(u => u.Username.Equals(userDto.Username) && u.Id != id))
            {
                ModelState.AddModelError("Username", "Username not available.");
                return ValidationFailed(ModelState);
            }

            var user = await _repository.Users.GetAsync(id, track:true);
            _mapper.Map(userDto, user);

            await _repository.SaveChangesAsync(cancellationToken);

            return Ok(_mapper.Map<UserReadDto>(user));
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> PatchAsync(int id, 
            JsonPatchDocument<UserUpdateDto> docPatch,
            CancellationToken cancellationToken = default)
        {
            var storedUser = await _repository.Users.GetAsync(id, track: true);
            var userToBePatched = _mapper.Map<UserUpdateDto>(storedUser);
            
            docPatch.ApplyTo(userToBePatched, ModelState);
            TryValidateModel(userToBePatched);

            if (!ModelState.IsValid)
                return ValidationFailed(ModelState);

            _mapper.Map(userToBePatched, storedUser);
            await _repository.SaveChangesAsync(cancellationToken);

            return Ok(_mapper.Map<UserReadDto>(storedUser));
        }

        // DELETE: User/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await _repository.Users.GetAsync(id);

            _repository.Users.Delete(user);
            await _repository.SaveChangesAsync(cancellationToken);

            return Ok(_mapper.Map<UserReadDto>(user));
        }
    }
}
