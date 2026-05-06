using DocShareApi.Attributes;
using DocShareApi.Dtos.Documents;
using DocShareApi.Dtos.Exception;
using DocShareApi.Dtos.QueryOptions;
using DocShareApi.Dtos.QueryOptions.Filters;
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
[Produces("application/json")]
[ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status400BadRequest)]
public class DocumentController(
    IDocumentService docServ
) : ControllerBase {

    [Authorize]
    [HttpPost(Name = "CreateDocument")]
    [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(FullDocumentDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateUpdateDocDto dto) {
        var callerId = this.GetUserId();
        var result = await docServ.CreateDocument(callerId, dto);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        Document newDoc = result.Value;
        return CreatedAtAction(nameof(GetById),
            new { docId = result.Value.Id },
            result.Value.ToFullDto());
    }

    [HttpGet("{docId}", Name = "GetDocumentById")]
    [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(FullDocumentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromRoute] string docId) {
        var result = await docServ.GetDocumentInfo(docId);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        Document doc = result.Value;
        return Ok(doc.ToFullDto());
    }

    [HttpGet(Name = "SearchDocuments")]
    [ProducesResponseType(typeof(PaginatedResponseDto<ShortDocumentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search(
        [FromQuery] SortDto sortDto, [FromQuery] PaginationDto paginationDto,
        [FromQuery] DocumentFilterDto filterDto
    ) {
        var result = await docServ.SearchDocuments(sortDto, paginationDto, filterDto);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        PaginatedResponseDto<Document> documents = result.Value;
        return Ok(documents.Convert(d => d.ToShortDto()));
    }

    [Authorize]
    [HttpPut("{docId}", Name = "UpdateDocument")]
    [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(FullDocumentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(
        [FromRoute] string docId, [FromBody] CreateUpdateDocDto dto
    ) {
        var callerId = this.GetUserId();
        var result = await docServ.UpdateDocumentInfo(callerId, docId, dto);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        Document updatedDoc = result.Value;
        return Ok(updatedDoc.ToFullDto());
    }

    [Authorize]
    [HttpDelete("{docId}", Name = "DeleteDocumentById")]
    [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteById([FromRoute] string docId) {
        var callerId = this.GetUserId();
        var result = await docServ.DeleteDocument(callerId, docId);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        return NoContent();
    }
}
