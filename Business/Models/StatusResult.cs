using Domain.Models;

namespace Business.Models;

public class StatusResult<Status> : ServiceResult
{
    public Status? Result { get; set; }
}

public class StatusResult : ServiceResult
{
    public IEnumerable<Status>? Result { get; set; }
}
