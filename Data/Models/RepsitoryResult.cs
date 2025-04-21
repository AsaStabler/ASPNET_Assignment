namespace Data.Models;

public class RepsitoryResult<T>
{
    public bool Succeeded { get; set; }

    public int StatusCode { get; set; }

    public string? Error { get; set; }

    public T? Result { get; set; }
}
