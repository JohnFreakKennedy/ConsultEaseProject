namespace ConsultEaseBLL.Exceptions;

public class CounsellingCategoryNotFoundException: Exception
{
    public CounsellingCategoryNotFoundException() { }
    public CounsellingCategoryNotFoundException(string message) : base(message) { }
    public CounsellingCategoryNotFoundException(string message, Exception inner) : base(message, inner) { }
}