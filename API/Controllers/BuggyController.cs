using System;
using System.Runtime.InteropServices;
using API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController(DataContext context) : BaseApiController
{
        [Authorize]
        [HttpGet("auth")]

        public ActionResult<string> GetAuth()
        {
            return "Secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<string> GetNotFound()
        {
            return NotFound();
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var result = context.Users.Find(-1) ?? 
            throw new ArgumentException("Server error ocurred");
            return "random text";
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("Bad request happened");
        }
}