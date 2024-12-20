using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sonrysocialsapi.Infrastructure;
using sonrysocialsapi.Models;

namespace sonrysocialsapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserHandler _userHandler;

        public UserController(IUserHandler userHandler)
        {
            _userHandler = userHandler;
        }
        
        [HttpGet("{pretoken}")]
        public async Task<IActionResult> GetUser(string pretoken)
        {
            string username = await _userHandler.GetUserByPretoken(pretoken);
            return username != null ? (IActionResult)Ok(username) : NotFound();
        }
        
    }
}
