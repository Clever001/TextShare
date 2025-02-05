using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TextShareApi.Data;

namespace TextShareApi.Controllers;

[Route("api/messages")]
[ApiController]
public class MessageController : ControllerBase {
    private AppDbContext _context;

    public MessageController(AppDbContext context) {
        _context = context;
    }
    
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll() {
        var messages = await _context.Messages.ToListAsync();
        return Ok(messages);
    }
}