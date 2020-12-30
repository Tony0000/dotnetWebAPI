using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories.Interfaces;
using Domain.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebAPI.ActionFilters;
using WebAPI.Dtos.UserDtos;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryFactory _repository;

        public UserController(IRepositoryFactory repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: User/
        [HttpGet]
        public IActionResult Get()
        {
            var users = _repository.Users.GetAll(track:false);
            try
            {
                users = _repository.Users.Search(Request.Query, users);
                users = _repository.Users.Sort(Request.Query, users);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new { error = e.GetType().ToString(), message = e.Message});
            }

            var usersDto = _mapper.Map<IEnumerable<UserReadDto>>(users);

            return Ok(usersDto);
        }

        // GET: User/5
        [HttpGet]
        [Route("{id}")]
        [ServiceFilter(typeof(NotFoundAttribute<User>))]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _repository.Users.GetAsync(id, track:false);
            var userDto = _mapper.Map<UserReadDto>(user);
            
            return Ok(userDto);
        }


        // POST: Users
        [HttpPost]
        [ServiceFilter(typeof(ModelValidationAttribute))]
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
        [ServiceFilter(typeof(NotFoundAttribute<User>))]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public IActionResult Put(int id, UserUpdateDto userDto)
        {
            if (id != userDto.Id)
                return BadRequest();

            if (_repository.Users.Exists(u => u.Username.Equals(userDto.Username) && u.Id != id))
            {
                ModelState.AddModelError("Username", "Username not available.");
                return ValidationFailed(ModelState);
            }

            var user = _repository.Users.Find(id);
            _mapper.Map(userDto, user);

            _repository.Users.SaveChanges();

            return Ok(_mapper.Map<UserReadDto>(user));
        }

        [HttpPatch]
        [Route("{id}")]
        [ServiceFilter(typeof(NotFoundAttribute<User>))]
        public IActionResult Patch(int id, JsonPatchDocument<UserUpdateDto> docPatch)
        {
            var storedUser = _repository.Users.Find(id);
            var userToBePatched = _mapper.Map<UserUpdateDto>(storedUser);
            
            docPatch.ApplyTo(userToBePatched, ModelState);
            TryValidateModel(userToBePatched);

            if (!ModelState.IsValid)
                return ValidationFailed(ModelState);

            _mapper.Map(userToBePatched, storedUser);
            _repository.Users.SaveChanges();

            return Ok(_mapper.Map<UserReadDto>(storedUser));
        }

        // DELETE: User/5
        [HttpDelete]
        [Route("{id}")]
        [ServiceFilter(typeof(NotFoundAttribute<User>))]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _repository.Users.GetAsync(id);

            _repository.Users.Delete(user);
            await _repository.SaveChangesAsync();

            return Ok(_mapper.Map<UserReadDto>(user));
        }
    }
}
