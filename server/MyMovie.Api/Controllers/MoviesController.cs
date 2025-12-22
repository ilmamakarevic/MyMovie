using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyMovie.Application;
using MyMovie.Domain.Interfaces;
using MyMovie.Application.DTOs;
using MyMovie.Domain.Entities;
using MyMovie.Infrastructure;
using MyMovie.Infrastructure.Repositories;
using System.Text.Json;

namespace MyMovie.Api.Controllers
{
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
    }
}
