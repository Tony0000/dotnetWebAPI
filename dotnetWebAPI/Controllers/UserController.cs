using System.Collections.Generic;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnetWebAPI.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private List<string> _users = new List<string>
        {
            "John Doe", "Jane Doe", "Mark Doe", "Clark Doe"
        };

        private readonly IMapper _mapper;

        public UserController(IMapper mapper)
        {
            _mapper = mapper;
        }

        // GET: User/
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_users);
        }

        // GET: User/5
        [HttpGet]
        [Route("{id}")]
        public ActionResult Get(int id)
        {
            return Ok(_users[id]);
        }


        // POST: Users
        [HttpPost]
        public ActionResult Post(User user)
        {
            return Created("created", user);
        }

        // DELETE: User/5
        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete(int id)
        {
            return Ok(_users[id]);
        }
    }
}
