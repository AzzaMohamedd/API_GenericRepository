using APIGenericRepo.Models;

namespace APIGenericRepo.Repositories
{
    public class StudentRepository
    {
        ITIContext db;
        public StudentRepository(ITIContext db)
        {
            this.db = db;
        }

        public List<Student> Search(string search)
        {
            return db.Students.Where(x => x.St_Fname.StartsWith(search)).ToList();
        }
    }
}
