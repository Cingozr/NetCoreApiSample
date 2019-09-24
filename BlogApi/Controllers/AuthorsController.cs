using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogApi.Entities;
using BlogApi.Models;
using BlogApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        public AuthorsController(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }



        [HttpGet("GetAuthors")]
        public IActionResult GetAuthors()
        {
            ApiResponseMessage<List<AuthorDTO>> apiResponseMessage = new ApiResponseMessage<List<AuthorDTO>>() { Succeed = false, Message = "", Data = null };

            var authorsModels = _mapper.Map<List<AuthorDTO>>(_authorRepository.GetAuthors());

            if (authorsModels.Count > 0)
            {
                apiResponseMessage.Succeed = true;
                apiResponseMessage.Message = $"Islem basarili.";
                apiResponseMessage.Data = authorsModels;
            }

            return Ok(apiResponseMessage);
        }

        [HttpGet("GetAuthorForArticles/{authorId}")]
        public IActionResult GetAuthorForArticles(Guid authorId)
        {
            ApiResponseMessage<AuthorDTO> apiResponseMessage = new ApiResponseMessage<AuthorDTO>() { Succeed = false, Message = "", Data = null };
            if (authorId == Guid.Empty)
                return BadRequest();

            if (!_authorRepository.AuthorExists(authorId))
                return NoContent();

            var authorForArticlesModel = _mapper.Map<AuthorDTO>(_authorRepository.GetAuthorForArticles(authorId));
            if (authorForArticlesModel != null)
            {
                apiResponseMessage.Succeed = true;
                apiResponseMessage.Message = $"Islem basarili.";
                apiResponseMessage.Data = authorForArticlesModel;
            }

            return Ok(apiResponseMessage);
        }

        [HttpGet("{authorId}", Name = "GetAuthor")]
        public IActionResult GetAuthor(Guid authorId)
        {
            ApiResponseMessage<AuthorDTO> apiResponseMessage = new ApiResponseMessage<AuthorDTO>() { Succeed = false, Message = "", Data = null };
            if (authorId == Guid.Empty)
            {
                return BadRequest("Mesaj yaz");
            }

            var authorModel = _mapper.Map<AuthorDTO>(_authorRepository.GetAuthor(authorId));
            if (authorModel == null)
            {
                //return NoContent("Mesaj yaz");
            }

            apiResponseMessage.Succeed = true;
            apiResponseMessage.Message = $"Islem basarili. GetAuthor";
            apiResponseMessage.Data = authorModel;

            return Ok(apiResponseMessage);
        }

        [HttpPost("CreateAuthor")]
        public IActionResult CreateAuthor([FromBody] AuthorCreateDTO authorCreateModel)
        {
            if (authorCreateModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var authorEntity = _mapper.Map<Author>(authorCreateModel);
            _authorRepository.AddAuthor(authorEntity);

            if (!_authorRepository.Save())
            {
                throw new Exception("Kayit sirasinda hata olustu");
            }

            return CreatedAtRoute("GetAuthor", new { authorId = authorEntity.Id }, authorEntity);
        }

        [HttpDelete("DeleteAuthor/{authorId}")]
        public IActionResult DeleteAuthor(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                return BadRequest($"Author Id is not empty");
            }

            var authorEntity = _authorRepository.GetAuthor(authorId);
            if (authorEntity == null)
            {
                return NoContent();
            }

            _authorRepository.DeleteAuthor(authorEntity);

            if (!_authorRepository.Save())
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }

            return Ok();
        }

        [HttpPut("UpdateAuthor")]
        public IActionResult UpdateAuthor(Guid authorId, [FromBody] AuthorUpdateDTO author)
        {
            if (author == null)
            {
                return BadRequest();
            }

            var authorEntity = _authorRepository.GetAuthor(authorId);
            if (authorEntity == null)
            {
                return NotFound();
            }

            var authorUpdateEntity = _mapper.Map<Author>(author);
            _authorRepository.UpdateAuthor(authorUpdateEntity);

            if (!_authorRepository.Save())
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtRoute("GetAuthor", new { authorId = authorUpdateEntity.Id }, authorUpdateEntity);

        }

        //[HttpGet("{authorId}", Name = "BlockAuthorCreation")]
        //public IActionResult BlockAuthorCreation(Guid authorId)
        //{
        //    if (!_authorRepository.AuthorExists(authorId))
        //    {
        //        return new StatusCodeResult(StatusCodes.Status409Conflict);
        //    }
        //    return NotFound();
        //}


    }
}