using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

    public class ArticlesController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IMapper _mapper;

        public ArticlesController(IArticleRepository articleRepository, IMapper mapper)
        {
            _articleRepository = articleRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public IActionResult GetArticles()
        {
            ApiResponseMessage<List<ArticleDTO>> apiResponseMessage = new ApiResponseMessage<List<ArticleDTO>>() { Succeed = false, Message = "", Data = null };

            var model = _mapper.Map<List<ArticleDTO>>(_articleRepository.GetArticles());
            if (model.Count > 0)
            {
                apiResponseMessage.Succeed = true;
                apiResponseMessage.Message = $"Islem basarili donen kayit sayisi: {model.Count}";
                apiResponseMessage.Data = model;
            }


            return Ok(apiResponseMessage);
        }

        [HttpGet("{authorId}", Name = "GetArticlesForAuthor")]
        public IActionResult GetArticlesForAuthor(Guid authorId)
        {
            ApiResponseMessage<List<ArticleDTO>> apiResponseMessage = new ApiResponseMessage<List<ArticleDTO>>() { Succeed = false, Message = "", Data = null };

            var articlesForAuthor = _mapper.Map<List<ArticleDTO>>(_articleRepository.GetArticlesForAuthor(authorId));
            if (articlesForAuthor != null)
            {
                apiResponseMessage.Succeed = true;
                apiResponseMessage.Message = $"Islem basarili GetArticlesForAuthor";
                apiResponseMessage.Data = articlesForAuthor;
            }
            else
                return NotFound();

            return Ok(apiResponseMessage);
        }

        [HttpPost("CreateArticle")]
        public IActionResult CreateArticle(Guid authorId, [FromBody] ArticleCreateDTO articleCreateModel)
        {
            if (articleCreateModel == null)
            {
                return BadRequest();
            }

            var articleEntity = _mapper.Map<Article>(articleCreateModel);
            _articleRepository.AddArticleForAuthor(authorId, articleEntity);
            if (!_articleRepository.Save())
            {
                throw new Exception("Kayit sirasinda hata olustu");
            }

            return CreatedAtRoute("GetArticlesForAuthor",
                new { authorId = articleEntity.AuthorId },
                articleEntity);

        }

        [HttpGet("GetArticleExist/{articleId}")]
        public IActionResult GetArticle(Guid articleId)
        {
            try
            {
                throw new Exception("Bilinmeyen bir hata olustu");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Bilinmeyen bir hata olustu");
            }
        }

        [HttpDelete("DeleteArticle/{articleId}")]
        public IActionResult DeleteArticle(Guid articleId)
        {
            if (articleId == Guid.Empty)
            {
                return BadRequest($"Article Id is not empty");
            }

            var articleEntity = _articleRepository.GetArticle(articleId);
            if (articleEntity == null)
            {
                return NotFound($"{articleId} is not found");
            }

            _articleRepository.DeleteArticle(articleEntity);
            if (!_articleRepository.Save())
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok();
        }



    }
}