using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolApi.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private static List<StudentModel> studentList = new List<StudentModel>()
        {
            new StudentModel(){Id=1,CourseId=100,Name="John",Surname="Doe",Gender="M"},
            new StudentModel(){Id=2,CourseId=100,Name="Liza",Surname="Koter",Gender="W"},
            new StudentModel(){Id=3,CourseId=200,Name="Fin",Surname="Olemna",Gender="W"},
            new StudentModel(){Id=4,CourseId=300,Name="Eve",Surname="Lesto",Gender="W"},
            new StudentModel(){Id=5,CourseId=320,Name="Abraham",Surname="Lincoln",Gender="M"},
            new StudentModel(){Id=6,CourseId=320,Name="George",Surname="Gratt",Gender="M"},
            new StudentModel(){Id=7,CourseId=320,Name="Ralph",Surname="Sutre",Gender="M"},
            new StudentModel(){Id=8,CourseId=400,Name="Daniella",Surname="Ekto",Gender="W"}
        };

        private bool Compare(string? pattern, string property)
        {
            if (pattern==null||pattern.Length == 0)
            {
                return true;
            }
            return property.ToLower().Contains(pattern.ToLower());
        }

        private bool Compare(int? number, int property)
        {
            if (number ==null)
            {
                return true;
            }
            return number == property;
        }
        [HttpGet]
        public IActionResult GetStudents()
        {
            return Ok(studentList);
        }
        [HttpGet("list")]
        public IActionResult GetStudentsFromQuery([FromQuery] int? id,int? courseid, string name, string surname, string gender)
        {
            var filteredStudents = studentList.Where(x => Compare(id, x.Id) && Compare(courseid, x.CourseId) &&
              Compare(name, x.Name) && Compare(surname, x.Surname) && Compare(gender, x.Gender));
            return Ok(filteredStudents);
        }
        [HttpPut]
        public IActionResult UpdateStudent([FromBody] StudentModel student)
        {
            if (ModelState.IsValid)
            {
                var upToUpdate = studentList.FirstOrDefault(x => x.Id == student.Id);
                if (upToUpdate != null)
                {
                    studentList.Remove(upToUpdate);
                    studentList.Add(student);
                    return Created($"api/students/{student.Id}", student);
                }
                return BadRequest("The object to be updated was not found");
            }
            return BadRequest();
        }
        [HttpPost]
        public IActionResult AddStudent([FromBody] StudentModel student)
        {
            if (ModelState.IsValid)
            {
                studentList.Add(student);
                return Created($"api/students/{student.Id}", student);
            }
            return BadRequest();
        }
        [HttpDelete]
        public IActionResult DeleteStudent([FromQuery] int id)
        {
            var upToDelete = studentList.FirstOrDefault(x => x.Id == id);
            if (upToDelete != null)
            {
                studentList.Remove(upToDelete);
                return Ok();
            }
            return NotFound("The object to be deleted was not found");
        }

    }
}
