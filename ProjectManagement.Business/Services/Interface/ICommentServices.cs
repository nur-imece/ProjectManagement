using ProjectManagement.Data.Entity;

namespace ProjectManagement.Model.Services.Interface;

public interface ICommentServices
{
     List<Comment> GetAll();
     Task  AddCommentAsync (Comment comment);


}