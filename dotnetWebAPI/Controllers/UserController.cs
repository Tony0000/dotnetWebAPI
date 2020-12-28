using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Data.Repositories.Interfaces;
using Domain.Model;
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
        public async Task<IActionResult> Get()
        {
            var users = await _repository.Users.GetAllAsync(track:false);

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
