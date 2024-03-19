namespace ProjectCore.TraceIdLogic.Interfaces;

/// <summary>
/// чтение трассировочных значений при создании нового scoped
/// </summary>
public interface ITraceReader
{
    string Name { get; }

    void WriteValue(string value);
}