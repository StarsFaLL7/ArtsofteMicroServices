namespace ProjectCore.TraceIdLogic.Interfaces;

public interface ITraceIdAccessor
{
    string Name { get; }
    
    void WriteValue(string value);
    
    string GetValue();
}