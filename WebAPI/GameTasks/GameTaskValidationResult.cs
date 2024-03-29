﻿namespace WebAPI.GameTasks;

public class GameTaskValidationResult
{
    public string ErrorMessage { get; set; } = string.Empty;
    public bool IsValid => string.IsNullOrEmpty(ErrorMessage);
}
