using AutoMapper;
using BookGate.Application.DTOs;
using BookGate.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookGate.API.Controllers
{
    public class BookController : Controller
    {
        private readonly BookService _service;
        private readonly PublisherService _publisherService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private async Task<string> SaveImage(IFormFile file)
        {
            string folder = "uploads/books/";
            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

            // Tạo thư mục nếu chưa có
            if (!Directory.Exists(serverFolder)) Directory.CreateDirectory(serverFolder);

            // Tạo tên file duy nhất để không bị trùng
            string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(serverFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return "/" + folder + fileName; // Trả về đường dẫn để lưu vào DB
        }

        public BookController(BookService service, PublisherService publisherService, IWebHostEnvironment webHostEnvironment)
        {
            _service = service;
            _publisherService = publisherService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var books = await _service.GetAll();
            return View(books);
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
