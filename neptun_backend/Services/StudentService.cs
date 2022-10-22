﻿using neptun_backend.Context;
using neptun_backend.Entities;

namespace neptun_backend.Services
{
    public interface IStudentService
    {
        List<Student> getAll();
    }

    public class StudentService : IStudentService
    {
        private readonly NeptunBackendDbContext _dbContext;

        public StudentService(NeptunBackendDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Student> getAll()
        {
            return _dbContext.Students.ToList();
        }
    }
}