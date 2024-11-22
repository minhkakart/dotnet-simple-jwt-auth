using BaseAuth.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace BaseAuth.Controllers;

[ApiController]
[Route("api/[controller]")]
[ResponseWrapped]
public class AppController : ControllerBase;