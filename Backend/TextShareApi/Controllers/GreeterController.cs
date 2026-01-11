using Auth;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using TextShareApi.Attributes;
using TextShareApi.Dtos.Greeter;

namespace TextShareApi.Controllers;

[ValidateModelState]
[Route("api/[controller]")]
[ApiController]
public class GreeterController : ControllerBase {
    private Greeter.GreeterClient _greeterClient;
    public GreeterController(Greeter.GreeterClient greeterClient) {
        _greeterClient = greeterClient;
    }

    [HttpPost]
    public async Task<IActionResult> RequestGreeting([FromBody] GreetingRequest request) {
        var reply = await _greeterClient.SayHelloAsync(new HelloRequest() {
            Name = request.Name
        });
        return Ok(reply);
    }
}
