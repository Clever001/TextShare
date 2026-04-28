using DocShareApi.Attributes;
using DocShareApi.Dtos.Documents;
using DocShareApi.Extensions;
using DocShareApi.Mappers;
using DocShareApi.Models;
using DocShareApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocShareApi.Controllers;

[ValidateModelState]
[Route("api/documents")]
[ApiController]
public class DocumentController(
    IDocumentService docServ
) : ControllerBase {
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUpdateDocDto dto) {
        var callerId = User.GetId();
        if (callerId == null) // Never executed
            throw new ArgumentNullException(nameof(callerId));
        
        var result = await docServ.CreateDocument(callerId, dto);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        Document newDoc = result.Value;
        return Ok(newDoc.ToDto());
    }
}