using ProjectManagement.Data.Entity;

namespace ProjectManagement.Model.Services.Interface;

public interface IJobServices
{
     List<Job> GetAll();
     Task  AddJobAsync (Job job);
}