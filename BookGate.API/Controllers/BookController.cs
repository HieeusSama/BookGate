using AutoMapper;
using BookGate.Application.DTOs;
using BookGate.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookGate.API.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookController : Controller
    {
        private readonly BookService _service;
        private readonly PublisherService _publisherService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private async Task<string> SaveImage(IFormFile file)
        {
            string folder = "uploads/books/";
            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

            if (!Directory.Exists(serverFolder)) Directory.CreateDirectory(serverFolder);

            string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(serverFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return "/" + folder + fileName;
        }

        public BookController(BookService service, PublisherService publisherService, IWebHostEnvironment webHostEnvironment)
        {
            _service = service;
            _publisherService = publisherService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm, int page = 1)
        {
            int pageSize = 5;

            var allBooks = await _service.GetAll();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var lowerSearchTerm = searchTerm.ToLower();

                allBooks = allBooks.Where(b =>
                    (b.Title != null && b.Title.ToLower().Contains(lowerSearchTerm)) ||
                    (b.Author != null && b.Author.ToLower().Contains(lowerSearchTerm)) ||
                    (b.Genre != null && b.Genre.ToLower().Contains(lowerSearchTerm))
                ).ToList();
            }

            int totalBooks = allBooks.Count();
            int totalPages = totalBooks > 0 ? (int)Math.Ceiling(totalBooks / (double)pageSize) : 0;

            var booksOnPage = allBooks
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchTerm = searchTerm;

            return View(booksOnPage);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var publishers = await _publisherService.GetAll();
            ViewBag.PublisherList = new SelectList(publishers, "PublisherId", "PublisherName");
            ViewBag.IsEdit = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookDTO book, IFormFile coverImage)
        {
            if (ModelState.IsValid)
            {
                if (coverImage != null)
                {
                    book.FileUrl = await SaveImage(coverImage);
                }
            }
            var publishers = await _publisherService.GetAll();
            ViewBag.PublisherList = new SelectList(publishers, "PublisherId", "PublisherName");
            ViewBag.IsEdit = false;
            await _service.Add(book);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var book = await _service.GetById(id);
            if (book == null) return NotFound();
            var publishers = await _publisherService.GetAll();
            ViewBag.PublisherList = new SelectList(publishers, "PublisherId", "PublisherName");
            ViewBag.IsEdit = true;
            return View("Create", book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BookDTO book, IFormFile? coverImage)
        {
            if (ModelState.IsValid)
            {
                if (coverImage != null)
                {
                    book.FileUrl = await SaveImage(coverImage);
                }
                var publishers = await _publisherService.GetAll();
                ViewBag.PublisherList = new SelectList(publishers, "PublisherId", "PublisherName");
                ViewBag.IsEdit = true;
                await _service.Update(book);
                return RedirectToAction(nameof(Index));
            }
            return View("Create", book);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
