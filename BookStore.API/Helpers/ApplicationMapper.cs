using AutoMapper;
using BookStore.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.API.Data;

namespace BookStore.API.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Books, Book>().ReverseMap();
        }
    }
}
