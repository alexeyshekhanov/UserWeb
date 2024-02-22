using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserWeb
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;

        public UserController(UserService _userService)
        {
            userService = _userService;
            
        }

        [HttpGet]
        [Route("/users")]
        public IActionResult GetAllUsers()
        {
            return Ok(userService.GetAll());
        }

        [HttpPost]
        [Route("/users")]
        public IActionResult Add(User user)
        {
            var result = userService.Add(user);
            return Ok(result);
        }

        [HttpPut]
        [Route("/users/{id}")]
        public IActionResult Update(int id, User user)
        {
            userService.Update(id, user);
            return Ok();
        }

        [HttpDelete]
        [Route("/users/{id}")]
        public IActionResult Delete(int id)
        {
            userService.Remove(id);
            return Ok();
        }

    }
}
