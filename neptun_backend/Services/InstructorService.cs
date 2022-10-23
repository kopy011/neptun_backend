﻿using Microsoft.EntityFrameworkCore;
using neptun_backend.Context;
using neptun_backend.Entities;

namespace neptun_backend.Services
{
    public interface IInstructorService
    {
        List<Instructor> getAll();
        List<Course> getAllCourse(string NeptunCode, int SemesterId);
    }

    public class InstructorService : IInstructorService
    {
        private readonly NeptunBackendDbContext _dbContext;

        public InstructorService(NeptunBackendDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Instructor> getAll()
        {
            return _dbContext.Instructors.Include(i => i.Courses).ToList();
        }
        public List<Course> getAllCourse(string NeptunCode, int SemesterId)
        {
            return _dbContext.Instructors.Include(i => i.Courses.Where(c => c.Semester.Id == SemesterId)).Where(i => i.NeptunCode.Equals(NeptunCode)).FirstOrDefault()?.Courses ?? new List<Course>();
        }
    }
}
