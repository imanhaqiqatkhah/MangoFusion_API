using MangoFusion_API.Data;
using MangoFusion_API.Models;
using MangoFusion_API.Models.Dto;
using MangoFusion_API.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MangoFusion_API.Controllers
{
    [Route("api/MenuItem")]
    [Authorize(Roles = SD.Role_Admin)]
    [ApiController]
    public class MenuItemController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly ApiResponse _response;
        private readonly IWebHostEnvironment _env;

        public MenuItemController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _response = new ApiResponse();
            _env = env;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetMenuItems()
        {
            List<MenuItem> menuItems = _db.MenuItems.ToList();
            List<OrderDetail> orderDetailsWithRatings = _db.OrderDetails.Where(u => u.Rating != null).ToList();
            foreach(var menuItem in menuItems)
            {
                var ratings = orderDetailsWithRatings.Where(u => u.MenuItemId == menuItem.Id).Select(u => u.Rating.Value);
                double avgRating = ratings.Any() ? ratings.Average() : 0;
                menuItem.Rating = avgRating;
            }
            _response.Result = menuItems;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id:int}", Name="GetMenuItem")]
        [AllowAnonymous]
        public IActionResult GetMenuItem(int id)
        {
            if (id == 0)
            {
                _response.StatusCode= HttpStatusCode.BadRequest;
                _response.InSuccess = false;    
                return BadRequest(_response);
            }
            MenuItem? menuItem = _db.MenuItems.FirstOrDefault(u => u.Id == id);
            List<OrderDetail> orderDetailsWithRatings = _db.OrderDetails.Where(u => u.Rating != null &&u.MenuItemId==menuItem.Id).ToList();
            
                var ratings = orderDetailsWithRatings.Select(u => u.Rating.Value);
                double avgRating = ratings.Any() ? ratings.Average() : 0;
                menuItem.Rating = avgRating;
            

            _response.Result = menuItem;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateMenuItem([FromForm]MenuItemCreateDTO menuItemCreateDTO)
        {
            try {
                if (ModelState.IsValid)
                {
                    if (menuItemCreateDTO.File == null || menuItemCreateDTO.File.Length == 0)
                    {
                        _response.InSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.ErrorMessages = ["فایل لازم است"];
                        return BadRequest(_response);
                    }
                    var imagesPath = Path.Combine(_env.WebRootPath, "images");
                    if (!Directory.Exists(imagesPath))
                    {
                        Directory.CreateDirectory(imagesPath);
                    }
                    var filePath = Path.Combine(imagesPath, menuItemCreateDTO.File.FileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    // uploading the image
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await menuItemCreateDTO.File.CopyToAsync(stream);
                    }

                    MenuItem menuItem = new()
                    {
                        Name = menuItemCreateDTO.Name,
                        Description = menuItemCreateDTO.Description,
                        Price = menuItemCreateDTO.Price,
                        Category = menuItemCreateDTO.Category,
                        SpecialTag = menuItemCreateDTO.SpecialTag,
                        Image = "images/"+ menuItemCreateDTO.File.FileName
                    };

                    _db.MenuItems.Add(menuItem);
                    await _db.SaveChangesAsync();

                    _response.Result = menuItemCreateDTO;
                    _response.StatusCode = HttpStatusCode.Created;
                    return CreatedAtRoute("GetMenuItem", new { id = menuItem.Id }, _response);

                }
                else {
                    _response.InSuccess = false;
                }
            } catch (Exception ex) {
                _response.InSuccess = false;
                _response.ErrorMessages = [ex.ToString()];
            }

            return BadRequest(_response);
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse>> UpdateMenuItem(int id ,[FromForm] MenuItemUpdateDTO menuItemUpdateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (menuItemUpdateDTO == null || menuItemUpdateDTO.Id!=id)
                    {
                        _response.InSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        return BadRequest(_response);
                    }

                    MenuItem? menuItemFormDb = await _db.MenuItems.FirstOrDefaultAsync(u => u.Id == id);

                    if (menuItemFormDb == null)
                    {
                        _response.InSuccess = false;
                        _response.StatusCode = HttpStatusCode.NotFound;
                        return NotFound(_response);
                    }

                    menuItemFormDb.Name = menuItemUpdateDTO.Name;
                    menuItemFormDb.Description = menuItemUpdateDTO.Description;
                    menuItemFormDb.Price = menuItemUpdateDTO.Price;
                    menuItemFormDb.Category = menuItemUpdateDTO.Category;
                    menuItemFormDb.SpecialTag = menuItemUpdateDTO.SpecialTag;

                    if (menuItemUpdateDTO.File!=null && menuItemUpdateDTO.File.Length>0) {
                        var imagesPath = Path.Combine(_env.WebRootPath, "images");
                        if (!Directory.Exists(imagesPath))
                        {
                            Directory.CreateDirectory(imagesPath);
                        }
                        var filePath = Path.Combine(imagesPath, menuItemUpdateDTO.File.FileName);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        var filePath_OldFile = Path.Combine(_env.WebRootPath, menuItemFormDb.Image);
                        if (System.IO.File.Exists(filePath_OldFile))
                        {
                            System.IO.File.Delete(filePath_OldFile);
                        }

                        // uploading the image
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await menuItemUpdateDTO.File.CopyToAsync(stream);
                        }
                        menuItemFormDb.Image = "images/" + menuItemUpdateDTO.File.FileName;
                    }


                    // uploading the image
                    

                    

                    _db.MenuItems.Update(menuItemFormDb);
                    await _db.SaveChangesAsync();

                    
                    _response.StatusCode = HttpStatusCode.NoContent;
                    return Ok(_response);

                }
                else
                {
                    _response.InSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _response.InSuccess = false;
                _response.ErrorMessages = [ex.ToString()];
            }

            return BadRequest(_response);
        }

        [HttpDelete]
        public async Task<ActionResult<ApiResponse>> DeleteMenuItem(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (id==0)
                    {
                        _response.InSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        return BadRequest(_response);
                    }

                    MenuItem? menuItemFormDb = await _db.MenuItems.FirstOrDefaultAsync(u => u.Id == id);

                    if (menuItemFormDb == null)
                    {
                        _response.InSuccess = false;
                        _response.StatusCode = HttpStatusCode.NotFound;
                        return NotFound(_response);
                    }

                    

                   
                        
                        var filePath_OldFile = Path.Combine(_env.WebRootPath, menuItemFormDb.Image);
                        if (System.IO.File.Exists(filePath_OldFile))
                        {
                            System.IO.File.Delete(filePath_OldFile);
                        }


                    _db.MenuItems.Remove(menuItemFormDb);
                    await _db.SaveChangesAsync();


                    _response.StatusCode = HttpStatusCode.NoContent;
                    return Ok(_response);

                }
                else
                {
                    _response.InSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _response.InSuccess = false;
                _response.ErrorMessages = [ex.ToString()];
            }

            return BadRequest(_response);
        }

    }
}
