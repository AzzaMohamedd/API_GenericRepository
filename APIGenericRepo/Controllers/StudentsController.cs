using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIGenericRepo.Models;
using APIGenericRepo.DTO;
using APIGenericRepo.Repositories;

namespace APIGenericRepo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        GenericRepository<Student> GRepository;
        StudentRepository studentRepository;
        public StudentsController(GenericRepository<Student> GRepository, StudentRepository studentRepository)
        {
            this.GRepository = GRepository;
            this.studentRepository = studentRepository;
        }

        // GET: api/Students
        [HttpGet]
        public ActionResult GetStudents()
        {
            var students = GRepository.GetAll();
            if (students == null) return NotFound();
            else
            {
                List<StudentDTO> stsDTO = new List<StudentDTO>();
                foreach (var student in students)
                {
                    StudentDTO std = new StudentDTO()
                    {
                        ID = student.St_Id,
                        FName = student.St_Fname ,
                        LName= student.St_Lname,
                        Address = student.St_Address,
                        Age = student.St_Age,
                        DepartmentName = student.Dept?.Dept_Name,
                        SupervisorName = student.St_superNavigation?.St_Fname + student.St_superNavigation?.St_Lname  //put (?) if St_superNavigation=null don't access it
                    };
                    stsDTO.Add(std);
                }
                return Ok(stsDTO);
            }
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public ActionResult GetStudentByID(int id)
        {
            var student = GRepository.GetByID(id);

            if (student == null)
            {
                return NotFound();
            }
            else
            {
                StudentDTO stdDTO = new StudentDTO()
                {
                    ID = student.St_Id,
                    FName = student.St_Fname ,
                    LName= student.St_Lname,
                    Address = student.St_Address,
                    Age = student.St_Age,
                    DepartmentName = student.Dept.Dept_Name,
                    SupervisorName = student.St_superNavigation?.St_Fname + student.St_superNavigation?.St_Lname  //put (?) if St_superNavigation=null don't access it
                };
                return Ok(stdDTO);
            }
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public IActionResult EditStudent(int id, Student student)
        {
            if (id != student.St_Id) return BadRequest();
            if (student == null) return NotFound();

            GRepository.Update(student);
            GRepository.Save();

            return NoContent();
        }

        // POST: api/Students
        [Consumes("application/json")]  //to Limit add student to receive and send data in JSON format only;
        [HttpPost]
        public ActionResult AddStudent(Student student)
        {
            if (student == null) return BadRequest();
            else
            {
                GRepository.Add(student);
                GRepository.Save();

                return CreatedAtAction("GetStudentByID", new { id = student.St_Id }, student);
            }
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var student = GRepository.GetByID(id);
            if (student == null)
            {
                return NotFound();
            }

            GRepository.Delete(student);
            GRepository.Save();

            return Ok(student);
        }


        ///////////////////////////////////////////////////////////////////////////


        //Apply Pagination and searching on student data
        //1.Pagination
        [HttpGet("/api/Pagination")]
        public IActionResult GetSomeStudents(int page = 1, [FromQuery] int pageSize = 3)
        {
            var allSts = GRepository.GetAll();
            if (allSts.Count == 0) return NotFound();
            else
            {
                var totalCount = allSts.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                var sts = allSts.Skip((page - 1) * pageSize).Take(pageSize);

                List<StudentDTO> stsDTO = new List<StudentDTO>();
                foreach (var student in sts)
                {
                    StudentDTO std = new StudentDTO()
                    {
                        ID = student.St_Id,
                        FName = student.St_Fname,
                        LName =student.St_Lname,
                        Address = student.St_Address,
                        Age = student.St_Age,
                        DepartmentName = student.Dept?.Dept_Name,
                        SupervisorName = student.St_superNavigation?.St_Fname + student.St_superNavigation?.St_Lname  //put (?) if St_superNavigation=null don't access it
                    };
                    stsDTO.Add(std);
                }

                return Ok(stsDTO);
            }            
        }

        //2.Searching
        [HttpGet("/api/Searching")]
        public ActionResult GetStudentFromSearch([FromQuery] string search)
        {
            var sts = studentRepository.Search(search);

            if (sts == null) return NotFound();
            else
            {
                List<StudentDTO> stsDTO = new List<StudentDTO>();
                foreach (var student in sts)
                {
                    StudentDTO std = new StudentDTO()
                    {
                        ID = student.St_Id,
                        FName = student.St_Fname ,
                        LName =student.St_Lname,
                        Address = student.St_Address,
                        Age = student.St_Age,
                        DepartmentName = student.Dept?.Dept_Name,
                        SupervisorName = student.St_superNavigation?.St_Fname + student.St_superNavigation?.St_Lname  //put (?) if St_superNavigation=null don't access it
                    };
                    stsDTO.Add(std);
                }
             
                return Ok(stsDTO);
            }
        }
    }
}
