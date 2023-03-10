using DocStorage.Application.Adapters;
using FluentMediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocStorage.Api.Controllers.Documents
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IMediator _mediator;
        const string folderName = "files";
        readonly string folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        public DocumentController(IMediator meditator)
        {
            _mediator = meditator;
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Upload([FromForm] FileMetaData fileMetaData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var file = fileMetaData.File;

            var document = new AddDocumentCommand(fileMetaData);
            var fileExtension = file.FileName.Split('.').Last();
            document.Filename = Path.Combine(folderPath, $"{document.Id}.{fileExtension}");

            using (var stream = new FileStream(document.Filename, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var newDocument = await _mediator.SendAsync<Document>(document);
            return Ok(newDocument);
        }

        [Authorize(Roles = "Admin,Manager,Regular")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Download(Guid id)
        {
            var document = await _mediator.SendAsync<Document>(new GetDocumentRequest { Id = id });
            var filePath = Path.Combine(folderPath, document.Filename);

            return File(System.IO.File.ReadAllBytes(filePath), "application/octet-stream", document.Filename);
        }

        [HttpPost("{id}/access/user")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddUser([FromRoute] Guid id, ManageAccessDocumentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _mediator.PublishAsync(new AddUserAccessDocumentCommand
            {
                Id = id,
                UserId = request.Id
            });
            return Ok();
        }

        [HttpDelete("{id}/access/user")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id, ManageAccessDocumentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _mediator.PublishAsync(new RemoveUserAccessDocumentCommand
            {
                Id = id,
                UserId = request.Id
            });
            return Ok();
        }

        [HttpPost("{id}/access/group")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddGroup([FromRoute] Guid id, ManageAccessDocumentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _mediator.PublishAsync(new AddGroupAccessDocumentCommand
            {
                Id = id,
                GroupId = request.Id
            });
            return Ok();
        }

        [HttpDelete("{id}/access/group")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteGroup([FromRoute] Guid id, ManageAccessDocumentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _mediator.PublishAsync(new RemoveGroupAccessDocumentCommand
            {
                Id = id,
                GroupId = request.Id
            });
            return Ok();
        }
    }
}
