using AutoMapper;
using Data.Repositories.Interfaces;
using Domain.Model;
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

            return Ok(users);
        }

        // GET: User/5
        [HttpGet]
        [Route("{id}")]
        public ActionResult Get(int id)
        {
            var user = _repository.Users.Find(id);

            return Ok(user);
        }


        // POST: Users
        [HttpPost]
        public ActionResult Post(User user)
        {
            if (user == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            _repository.Users.Add(user);

            return Created("created", user);
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

            return Ok(user);
        }
    }
}
