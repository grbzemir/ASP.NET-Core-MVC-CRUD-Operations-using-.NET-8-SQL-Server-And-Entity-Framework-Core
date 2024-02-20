using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Data;
using StudentPortal.Web.Models;
using StudentPortal.Web.Models.Entities;

namespace StudentPortal.Web.Controllers
{
    public class StudentsController : Controller
    {
        //Dependy Injection Yapıldı
        private readonly ApplicationDbContext _dbContext;
        public StudentsController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Add(AddStudentViewModel viewModel)
        {
            var student = new Student

            {         
                Name = viewModel.Name,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                Subscribed = viewModel.Subscribed

             };

            await _dbContext.Students.AddAsync(student);

            await _dbContext.SaveChangesAsync();

            return View();

        }

        [HttpGet]

        public async Task<IActionResult> List()
        {
            var students = await _dbContext.Students.ToListAsync();

            return View(students);

        }

        [HttpGet]

        public async Task<IActionResult> Edit(Guid id)

        {
            var student = await _dbContext.Students.FindAsync(id);

            return View(student);

        }

        [HttpPost]

        public async Task<IActionResult> Edit(Student viewModel)

        {
            var student = await _dbContext.Students.FindAsync(viewModel.Id);

            if(student is not null)
            {

                student.Name = viewModel.Name;
                student.Email = viewModel.Email;
                student.Phone = viewModel.Phone;
                student.Subscribed = viewModel.Subscribed;

                await _dbContext.SaveChangesAsync();

                return RedirectToAction("List");

            }

            return RedirectToAction("List" , "Students");


        }

        [HttpPost]

        public async Task<IActionResult> Delete(Student viewModel)

        {
            var student = await _dbContext.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == viewModel.Id);

            if(student is not null)
            {
                _dbContext.Students.Remove(viewModel);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("List");

            }

            return RedirectToAction("List" , "Students");

        }
    }
}
