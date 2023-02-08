using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationJWT.Controllers
{
	//[Authorize]// applies to all methods except constructior.
	[Route("api/[controller]")]
	[ApiController]
	public class NameController : ControllerBase
	{
		private readonly IJWTAuthenticationManager jWTAuthenticationManager;
		public NameController(IJWTAuthenticationManager jWTAuthenticationManager)
		{
			this.jWTAuthenticationManager = jWTAuthenticationManager;
		}

		// GET: api/Name
		[HttpGet]
		[Authorize] // This allows access only with valid Token. It requires to add middleware (app.UseAuthorization()).
		public IEnumerable<string> Get()
		{
			return new string[] { "New York", "New Jersey" };
		}

		// GET: api/Name/5
		[HttpGet("{id}", Name = "Get")]
		public string Get(int id)
		{
			return "value";
		}

		[AllowAnonymous] // this allows without any token or authorization.
		[HttpPost("authenticate")]
		public IActionResult Authenticate([FromBody] UserCred userCred)
		{
			var token = jWTAuthenticationManager.Authenticate(userCred.UserName, userCred.Password);
			if (token==null)
			{
				return Unauthorized();
			}
			return Ok(token);
		}

		
	}
}
