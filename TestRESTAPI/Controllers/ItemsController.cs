﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestRESTAPI.Data;
using TestRESTAPI.Data.Models;
using TestRESTAPI.Models;

namespace TestRESTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        public ItemsController(AppDbContext db)
        {
            _db = db;
        }

        private readonly AppDbContext _db;

        [HttpGet]
        public async Task<IActionResult> AllItems()
        {
            var items = await _db.Items.ToListAsync();
            return Ok(items);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> AllItems(int id)
        {
            var item = await _db.Items.SingleOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return NotFound($"Item Code {id} not exists!");
            }
            return Ok(item);
        }

        [HttpGet("ItemsWithCategory{idCategory}")]
        public async Task<IActionResult> AllItemsWithCategory(int idCategory)
        {
            var items = await _db.Items.Where(x => x.CategoryId == idCategory).ToListAsync();
            if (items.Count == 0)
            {
                return NotFound($"Category Id {idCategory} has no items!");
            }
            return Ok(items);
        }
        [HttpPost]
        public async Task<IActionResult> AddItem([FromForm] mdlItem mdl)
        {
            using var stream = new MemoryStream();
            await mdl.Image.CopyToAsync(stream);
            var item = new Item
            {
                Name = mdl.Name,
                Price = mdl.Price,
                Notes = mdl.Notes,
                CategoryId = mdl.CategoryId,
                Image = stream.ToArray()
            };
            await _db.Items.AddAsync(item);
            await _db.SaveChangesAsync();
            return Ok(item);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> DeleteItem(int id, [FromForm] mdlItem mdl)
        {
            var item = await _db.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound($"Item Id {id} not exists!");
            }
            var isCategoryExists = await _db.Categories.AnyAsync(x => x.Id == mdl.CategoryId);
            if (isCategoryExists == false)
            {
                return NotFound($"Item Id {mdl.CategoryId} not exists!");
            }
            if (mdl.Image != null)
            {
                using var stream = new MemoryStream();
                await mdl.Image.CopyToAsync(stream);
            }
            item.Name = mdl.Name;
            item.Price = mdl.Price;
            item.Notes = mdl.Notes;
            item.CategoryId = mdl.CategoryId;

            await _db.SaveChangesAsync();
            return Ok(item);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _db.Items.SingleOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return NotFound($"Item Id {id} not exists!");
            }
            _db.Items.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(item);
        }
    }
}
