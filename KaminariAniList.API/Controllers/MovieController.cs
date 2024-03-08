using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Core.Entities.Api.MovieController;
using WMS.Core.Entities.Api.TmdbV3Api.BaseObjects;
using WMS.Core.Entities.Base;
using WMS.Service.ErrorHandling;
using WMS.Service.ServiceArticles;

namespace KaminariAniList.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpPost("DiscoverMovie")]
        public async Task<IActionResult> DiscoverMovieAsync([FromBody] BaseRequest<MovieDiscoverRequest> movieDiscoverRequest)
        {
            try
            {
                BaseResponse<BaseMovie> discoverMovieResult = await _movieService.DiscoverMovieAsync(movieDiscoverRequest);
                if (discoverMovieResult.Type == ResponseType.SUCCESS)
                    return Ok(discoverMovieResult);
                else
                    return BadRequest(discoverMovieResult);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ExceptionHandler<BaseMovie>.ExceptionToResponse(ex));
            }
        }

        [HttpPost("RecommandationMovie")]
        public async Task<IActionResult> RecommandationMovieAsync([FromBody] BaseRequest<MovieRecommendationRequest> movieRecommendationRequest)
        {
            try
            {
                BaseResponse<BaseMovie> recommandationMovieResult = await _movieService.RecommandationMovieAsync(movieRecommendationRequest);
                if (recommandationMovieResult.Type == ResponseType.SUCCESS)
                    return Ok(recommandationMovieResult);
                else
                    return BadRequest(recommandationMovieResult);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ExceptionHandler<BaseMovie>.ExceptionToResponse(ex));
            }
        }

        [HttpPost("MovieDetail")]
        public async Task<IActionResult> MovieDetailAsync([FromBody] BaseRequest<MovieDetailRequest> movieDetailRequest)
        {
            try
            {
                BaseResponse<MovieDetail> movieDetailResult = await _movieService.MovieDetailAsync(movieDetailRequest);
                if (movieDetailResult.Type == ResponseType.SUCCESS)
                    return Ok(movieDetailResult);
                else
                    return BadRequest(movieDetailResult);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ExceptionHandler<MovieDetail>.ExceptionToResponse(ex));
            }
        }

        [HttpPost("SearchMovie")]
        public async Task<IActionResult> SearchMovieAsync([FromBody] BaseRequest<MovieSearchRequest> movieSearchRequest)
        {
            try
            {
                BaseResponse<BaseMovie> searchMovieResult = await _movieService.SearchMovieAsync(movieSearchRequest);
                if (searchMovieResult.Type == ResponseType.SUCCESS)
                    return Ok(searchMovieResult);
                else
                    return BadRequest(searchMovieResult);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ExceptionHandler<BaseMovie>.ExceptionToResponse(ex));
            }
        }
    }
}