using DotNetTrainingBatch5.Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetTrainingBatch5.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {

        private readonly AppDbContext _db = new AppDbContext();
        [HttpGet]
        public IActionResult GetBlogs() 
        {
            var lst = _db.TblBlogs.AsNoTracking().Where (x => x.DeleteFlag ==false).ToList();
            return Ok(lst);
        }

        [HttpGet ( "{id}")]
        public IActionResult GetBlog(int id)
        {
            var item = _db.TblBlogs.AsNoTracking().FirstOrDefault(x => x.BlogId == id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreateBlogs(TblBlog Blog)
        {
            _db.TblBlogs.Add(Blog);
            _db.SaveChanges();
            return Ok(Blog);
        }

        [HttpPut ("{id}")]
        public IActionResult UpdateGlogs(int id , TblBlog Blog)
        {
            var item = _db.TblBlogs.AsNoTracking().FirstOrDefault(x => x.BlogId == id);
            if (item == null)
            {
                return NotFound();
            }
            item.BlogTitle = Blog.BlogTitle;
            item.BlogAuthor = Blog.BlogAuthor;
            item.BlogContent = Blog.BlogContent;

            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();

            return Ok(item);
        }

        [HttpPatch ("{id}")]
        public IActionResult PatchBlogs(int id,TblBlog Blog)
        {
            var item = _db.TblBlogs.AsNoTracking().FirstOrDefault(x => x.BlogId == id);
            if (item == null)
            {
                return NotFound();
            }
            if (string.IsNullOrEmpty(Blog.BlogTitle))
            {
                item.BlogTitle = Blog.BlogTitle;
            }
            if (string.IsNullOrEmpty(Blog.BlogAuthor))
            {
                item.BlogAuthor = Blog.BlogAuthor;
            }
            if (string.IsNullOrEmpty(Blog.BlogContent))
            {
                item.BlogContent = Blog.BlogContent;
            }
            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();
            return Ok(item);
        }


        [HttpDelete ("{id}")]
        public IActionResult DeleteBogs(int id)
        {
            var item = _db.TblBlogs.AsNoTracking().FirstOrDefault(x => x.BlogId == id);
            if (item == null)
            {
                return NotFound();
            }
            //_db.Entry (item).State = EntityState.Deleted;
            item.DeleteFlag = true;
            _db.Entry(item).State = EntityState.Modified;
            _db.SaveChanges();
            return Ok();
        }

    }
}
