using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TokyoGarden.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseCrudController<TDto, TEntity> : ControllerBase where TEntity : class
    {
        protected readonly IMapper _mapper;

        protected BaseCrudController(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected IActionResult NotFoundIfNull(object? obj) => obj == null ? NotFound() : Ok(obj);
    }
}
