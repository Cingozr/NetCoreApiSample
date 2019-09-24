using BlogApi.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlogApi.Services
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BlogContext _blogContext;
        public AuthorRepository(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }
        public void AddAuthor(Author author)
        {
            _blogContext.Authors.Add(author);
        }

        public bool AuthorExists(Guid authorId)
        {
            return _blogContext.Authors.Any(x => x.Id == authorId);
        }

        public void DeleteAuthor(Author author)
        {
            _blogContext.Authors.Remove(author);
        }

        public Author GetAuthor(Guid authorId)
        {
            return _blogContext.Authors.Where(x => x.Id == authorId).FirstOrDefault();
        }

        public Author GetAuthorForArticles(Guid authorId)
        {
            return _blogContext.Authors
                .Where(x => x.Id == authorId)
                .Include(x => x.Articles)
                .FirstOrDefault();
        }

        public IEnumerable<Author> GetAuthors()
        {
            return _blogContext.Authors.ToList();
        }

        public bool Save()
        {
            return _blogContext.SaveChanges() > 0;
        }

        public void UpdateAuthor(Author author)
        {
            _blogContext.Update(author);
        }
    }
}
