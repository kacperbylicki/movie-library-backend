using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using movie_library.Services;

namespace movie_library.Controllers;

[ApiController]
[Route("api/videos")]
public class VideosController : ControllerBase
{
    private readonly ILogger<VideosController> _logger;
    private readonly VideosService _videosService;
    
    public VideosController(ILogger<VideosController> logger, VideosService videosService)
    {
        _logger = logger;
        _videosService = videosService;
    }
    
    [HttpGet]
    [Authorize(Policy = "AdminOrModeratorOnly")]
    public async Task<IActionResult> GetUploadedVideos()
    {
        try
        {
            var videos = await _videosService.GetUploadedVideosKeys();

            return Ok(videos);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
}