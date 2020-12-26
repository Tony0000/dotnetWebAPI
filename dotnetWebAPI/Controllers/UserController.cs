using System.Collections.Generic;
using AutoMapper;
using Data.Repositories.Interfaces;
using Domain.Model;
using dotnetWebAPI.Dtos.UserDtos;
using Microsoft.AspNetCore.Mvc;

namespace dotnetWebAPI.Controllers
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
        public ActionResult Get()
        {
            var users = _repository.Users.GetAll();

            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(users));
        }

        // GET: User/5
        [HttpGet]
        [Route("{id}")]
        public ActionResult Get(int id)
        {
            var user = _repository.Users.Find(id);

            return Ok(_mapper.Map<UserReadDto>(user));
        }


        // POST: Users
        [HttpPost]
        public ActionResult Post(UserUpdateDto userDto)
        {
            if (userDto == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var user = _mapper.Map<User>(userDto);
            _repository.Users.Add(user);

            return Created("created", _mapper.Map<UserReadDto>(user));
        }

        // DELETE: User/5
        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete(int id)
        {
            var user = _repository.Users.Find(id);
            if (user == null)
                return NotFound();

            _repository.Users.Delete(user);

            return Ok(_mapper.Map<UserReadDto>(user));
        }
    }
}
