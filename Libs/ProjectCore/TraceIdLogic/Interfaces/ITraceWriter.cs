﻿namespace ProjectCore.TraceIdLogic.Interfaces;

/// <summary>
/// Запись трассировочных значений при отправке запроса
/// </summary>
public interface ITraceWriter
{
    string Name { get; }

    string GetValue();
}