using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogApi.Entities;
using BlogApi.Models;

namespace BlogApi.Mapping
{
    public class BlogCustomMappingProfile : Profile
    {
        public BlogCustomMappingProfile() : this("BlogProfile")
        {
        }

        public BlogCustomMappingProfile(string profileName) : base(profileName)
        {
            CreateMap<Article, ArticleDTO>().ReverseMap();
            CreateMap<Article, ArticleCreateDTO>().ReverseMap();
            CreateMap<Author, AuthorDTO>().ReverseMap();
            CreateMap<Author, AuthorCreateDTO>().ReverseMap();
            CreateMap<Author, AuthorUpdateDTO>().ReverseMap();


        }
    }
}
