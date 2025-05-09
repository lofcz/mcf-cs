using System.Diagnostics.CodeAnalysis;

namespace McfCs;

public class DataOrException<T>
{
    [MemberNotNullWhen(false, nameof(Exception))]
    private bool DataIsNotNull => Data is not null;
    
    [MemberNotNullWhen(false, nameof(Data))]
    private bool ExceptionIsNotNull => Exception is not null;
    
    public T? Data { get; set; }
    public Exception? Exception { get; set; }
    public ExceptionTypes ExceptionType { get; set; }

    public DataOrException(T? data)
    {
        Data = data;
    }

    public DataOrException(Exception? exception, ExceptionTypes type = ExceptionTypes.Unhandled)
    {
        Exception = exception;
        ExceptionType = type;
    }

    public static implicit operator DataOrException<T>(UpdateResult<T> data) => data.Error is not null ? new DataOrException<T>(new Exception(data.Error ?? string.Empty)) : new DataOrException<T>(data.Data);
}

public enum ExceptionTypes
{
    Unknown,
    Unhandled,
    Handled
}

public class UpdateResult : ICrudResult
{
    public bool Ok { get; set; }
    public string? Error { get; set; }
    public int AffectedRows { get; set; }
}

public class UpdateResult<T> : UpdateResult
{
    public T? Data { get; set; }
    
    public UpdateResult(string errorMsg)
    {
        Error = errorMsg;
    }
    
    public UpdateResult(T data)
    {
        Ok = true;
        Data = data;
    }
}

public class UpdateResult<T, T2> : UpdateResult
{
    public T? Data { get; set; }
    public T2? Data2 { get; set; }
}

public interface ICrudResult
{
    public bool Ok { get; set; }
    public string? Error { get; set; }
}

public class CreateResult : ICrudResult
{
    public bool Ok { get; set; }
    public string? Error { get; set; }

    public CreateResult()
    {
        
    }

    public CreateResult(string errorMsg)
    {
        Error = errorMsg;
    }
}

public class CreateResult<T> : ICrudResult where T : class
{
    public T? Entity { get; set; }
    public bool Ok { get; set; }
    public string? Error { get; set; }

    public CreateResult()
    {
        
    }

    public CreateResult(string errorMsg)
    {
        Error = errorMsg;
    }

    public CreateResult(T entity)
    {
        Entity = entity;
        Ok = true;
    }
}

public class ExecResult : ICrudResult
{
    public bool Ok { get; set; }
    public string? Error { get; set; }
    public int AffectedRows { get; set; }
}

public class DeleteResult : ICrudResult
{
    public bool Ok { get; set; }
    public string? Error { get; set; }
    public int AffectedRows { get; set; }
}

public class ReadResult<T> : ICrudResult
{
    public bool Ok { get; set; }
    public string? Error { get; set; }
    public T? Data { get; set; }
}