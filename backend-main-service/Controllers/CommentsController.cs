using DocShareApi.Attributes;
using DocShareApi.Dtos.Comments;
using DocShareApi.Dtos.Exception;
using DocShareApi.Dtos.QueryOptions;
using DocShareApi.Dtos.QueryOptions.Filters;
using DocShareApi.Extensions;
using DocShareApi.Mappers;
using DocShareApi.Models;
using DocShareApi.Repositories;
using DocShareApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocShareApi.Controllers;

[ValidateModelState]
[Route("api/comments")]
[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status400BadRequest)]
public class CommentsController(
    ICommentService commentsServ
) : ControllerBase {

    [Authorize]
    [HttpPost(Name = "CreateComment")]
    [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] CreateCommentDto dto) {
        var callerId = this.GetUserId();
        var result = await commentsServ.CreateComment(callerId, dto);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        Comment newComment = result.Value;
        return CreatedAtAction(nameof(GetById),
            new { commentId = result.Value.Id },
            result.Value.ToDto());
    }

    [HttpGet("{commentId}", Name = "GetCommentById")]
    [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById([FromRoute] long commentId) {
        var result = await commentsServ.GetComment(commentId);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        Comment comment = result.Value;
        return Ok(comment.ToDto());
    }

    [HttpGet(Name = "SearchComments")]
    [ProducesResponseType(typeof(PaginatedResponseDto<CommentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search(
        [FromQuery] PaginationDto paginationDto,
        [FromQuery] CommentFilterDto filterDto
    ) {
        var result = await commentsServ.SearchComments(
            paginationDto, filterDto
        );
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        PaginatedResponseDto<Comment> comments = result.Value;
        return Ok(comments.Convert(c => c.ToDto()));
    }

    [Authorize]
    [HttpPut("{commentId}", Name = "UpdateComment")]
    [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(
        [FromRoute] long commentId, [FromBody] UpdateCommentDto dto
    ) {
        var callerId = this.GetUserId();
        var result = await commentsServ.UpdateComment(
            callerId, commentId, dto
        );
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        Comment updatedComment = result.Value;
        return Ok(updatedComment.ToDto());
    }

    [Authorize]
    [HttpDelete("{commentId}", Name = "ClearComment")]
    [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ClearById([FromRoute] long commentId) {
        var callerId = this.GetUserId();
        var result = await commentsServ.ClearComment(callerId, commentId);
        if (!result.IsSuccess) return this.ToActionResult(result.Exception);

        return NoContent();
    }
}
