using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIGenericRepo.Models;
using APIGenericRepo.DTO;
using APIGenericRepo.Models;
using APIGenericRepo.Repositories;

namespace APIGenericRepo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        GenericRepository<Department> deptGenRepo;
        public DepartmentsController(GenericRepository<Department> deptGenRepo)
        {
            this.deptGenRepo = deptGenRepo;
        }

        // GET: api/Departments
        [HttpGet]
        public ActionResult GetDepartments()
        {
            var depts = deptGenRepo.GetAll();

            if (depts == null) return NotFound();
            else
            {
                List<DepartmentDTO> deptsDTO = new List<DepartmentDTO>();
                foreach (var dept in depts)
                {
                    DepartmentDTO depDTO = new DepartmentDTO()
                    {
                        ID = dept.Dept_Id,
                        Name = dept.Dept_Name,
                        Description = dept.Dept_Desc,
                        Location = dept.Dept_Location,
                        Dept_Manager = dept.Dept_Manager,
                        NumOfStudents = dept.Students.Count()
                    };
                    deptsDTO.Add(depDTO);
                }
                return Ok(deptsDTO);
            }
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public ActionResult GetDepartmentByID(int id)
        {
            var department = deptGenRepo.GetByID(id);

            if (department == null)
            {
                return NotFound();
            }
            else
            {
                DepartmentDTO depDTO = new DepartmentDTO()
                {
                    ID = department.Dept_Id,
                    Name = department.Dept_Name,
                    Description = department.Dept_Desc,
                    Location = department.Dept_Location,
                    Dept_Manager = department.Dept_Manager,
                    NumOfStudents = department.Students.Count()
                };
                return Ok(depDTO);
            }
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult EditDepartment(int id, Department department)
        {
            if (id != department.Dept_Id) return BadRequest();
            if (department == null) return NotFound();

            deptGenRepo.Update(department);
            deptGenRepo.Save();

            return NoContent();
        }

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult AddDepartment(Department department)
        {
            if (department == null) return BadRequest();
            else
            {
                deptGenRepo.Add(department);
                deptGenRepo.Save();

                return CreatedAtAction("GetDepartmentByID", new { id = department.Dept_Id }, department);
            }
        }
        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            var department = deptGenRepo.GetByID(id);
            if (department == null)
            {
                return NotFound();
            }

            deptGenRepo.Delete(department);
            deptGenRepo.Save();

            return Ok(department);
        }
    }
}
