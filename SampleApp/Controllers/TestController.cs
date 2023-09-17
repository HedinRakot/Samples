using Microsoft.AspNetCore.Mvc;
using SampleApp.Models;
using SampleApp.Application;

namespace SampleApp.Controllers
{
    public class TestController : Controller
    {
        private TestRepository _testRepository;
        public TestController(TestRepository testRepository)
        {
            _testRepository = testRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var models = new List<TestModel>();
            foreach (var item in _testRepository.Tests)
            {
                models.Add(new TestModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    LastName = item.LastName,
                });
            }

            return View(models);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new TestModel());
        }

        [HttpPost]
        public IActionResult Add(TestModel model)
        {
            if (ModelState.IsValid)
            {
                var lastId = _testRepository.Tests.Count != 0 ? _testRepository.Tests.Max(x => x.Id) : 0;

                _testRepository.Tests.Add(new Domain.Test
                {
                    Id = lastId + 1,
                    Name = model.Name,
                    LastName = model.LastName,
                });

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            var test = _testRepository.Tests.FirstOrDefault(x => x.Id == id);
            return View(new TestModel
            {
                Id = test.Id,
                Name = test.Name,
                LastName = test.LastName,
            });
        }

        [HttpPost]
        public IActionResult Edit(TestModel model)
        {
            if (ModelState.IsValid)
            {
                var test = _testRepository.Tests.FirstOrDefault(x => x.Id == model.Id);
                test.Name = model.Name;
                test.LastName = model.LastName;

                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}