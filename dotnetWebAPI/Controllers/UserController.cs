using System.Collections.Generic;
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
    public class UserController : Controller
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
            var users = _repository.Users.GetAll();
            
            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(users));
        }

        // GET: User/5
        [HttpGet]
        [Route("{id}")]
        [ServiceFilter(typeof(NotFoundAttribute<User>))]
        public IActionResult Get(int id)
        {
            var user = _repository.Users.Find(id);

            return Ok(_mapper.Map<UserReadDto>(user));
        }


        // POST: Users
        [HttpPost]
        [ServiceFilter(typeof(ModelValidationAttribute))]
        public IActionResult Post(UserUpdateDto userDto)
        {
            if (userDto == null)
                return BadRequest();

            var user = _mapper.Map<User>(userDto);
            _repository.Users.Add(user);

            return Created("created", _mapper.Map<UserReadDto>(user));
        }

        // DELETE: User/5
        [HttpDelete]
        [Route("{id}")]
        [ServiceFilter(typeof(NotFoundAttribute<User>))]
        public IActionResult Delete(int id)
        {
            var user = _repository.Users.Find(id);
            if (user == null)
                return NotFound();

            _repository.Users.Delete(user);

            return Ok(_mapper.Map<UserReadDto>(user));
        }
    }
}
