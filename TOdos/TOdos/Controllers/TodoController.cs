using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TOdos.Data;
using TOdos.Models;

namespace TOdos.Controllers;

public class TodoController : Controller
{
	public async Task<IActionResult> Index()
	{
		using (Ab108todosContext ctx = new Ab108todosContext())
		{
			return View(await ctx.Todos.ToListAsync());
		}
	}
	public IActionResult Create()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Create(Todo data)
	{
		using (Ab108todosContext ctx = new Ab108todosContext())
		{
			await ctx.AddAsync(data);
			await ctx.SaveChangesAsync();
		}
		return RedirectToAction(nameof(Index));
	}
	public async Task<IActionResult> Delete(int? id)
	{
		if (id == null) return BadRequest();

		using (Ab108todosContext ctx = new Ab108todosContext())
		{
			if (await ctx.Todos.AnyAsync(x => x.Id == id))
			{
				ctx.Todos.Remove(new Todo { Id = id.Value });
				await ctx.SaveChangesAsync();
			}
		}
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Update(int? id)
	{
		if (id == null) return BadRequest();
		using (Ab108todosContext ctx = new Ab108todosContext())
		{
			var data = await ctx.Todos.Where(x => x.Id == id.Value).FirstOrDefaultAsync();
			if (data == null) return NotFound();
			return View(data);
		}
	}

	[HttpPost]
	public async Task<IActionResult> Update(int? id, Todo todos)
	{
		if (id == null) return BadRequest();
		using (Ab108todosContext ctx = new Ab108todosContext())
		{
			var entity = await ctx.Todos.FindAsync(id.Value);
			if (entity == null) return NotFound();
			entity.Title = todos.Title;
			entity.Description = todos.Description;
			entity.Deadline = todos.Deadline;
			await ctx.SaveChangesAsync();
		}
		return RedirectToAction(nameof(Index));
	}

	public async Task<IActionResult> Look(int? id)
	{
		if (id == null) return BadRequest();
		using (Ab108todosContext ctx = new())
		{
			var data = await ctx.Todos.FindAsync(id.Value);
			if (data == null) return NotFound();
			return View(data);
		}
	}
	public async Task<IActionResult> FinishTask(int? id)
	{
		if (id == null) return BadRequest();
		using (Ab108todosContext ctx = new Ab108todosContext())
		{
			var entity = await ctx.Todos.Where(x => x.Id == id.Value).FirstOrDefaultAsync();
			if (entity == null) return NotFound();
			if (entity.Deadline > DateTime.Now)
			{
				entity.IsDone = true;
			}
			await ctx.SaveChangesAsync();
		}
		return RedirectToAction(nameof(Index));
	}
}
