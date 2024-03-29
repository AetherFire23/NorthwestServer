﻿using System.Net;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.Exceptions;
using WebAPI.Models;
using WebAPI.Models.Requests;
using WebAPI.Repositories;
using WebAPI.Services;
namespace WebAPI.Authentication;

public class AuthenticationService
{
    private readonly UserService _userService;
    private readonly JwtTokenManager _jwtTokenManager;
    private readonly UserRepository _userRepository;
    public AuthenticationService(UserService userService,
        JwtTokenManager jwtTokenManager,
        UserRepository userRepository)
    {
        _userService = userService;
        _jwtTokenManager = jwtTokenManager;
        _userRepository = userRepository;
    }

    public async Task<LoginResult> TryLogin(LoginRequest loginRequest)
    {
        (bool isTokenIssued, User? userModel) = await _userService.CanIssueTokenToUser(loginRequest);
        if (!isTokenIssued) throw new HttpRequestException(" COuld not login");

        UserDto userDto = await _userRepository.MapUserDtoById(userModel.Id);
        string token = await _jwtTokenManager.GenerateToken(userDto);

        LoginResult loginResult = new LoginResult
        {
            Token = token,
            UserId = userDto.Id,
        };

        return loginResult;
    }

    /// <summary> Content is userdto</summary>
    public async Task<UserDto> TryRegister(RegisterRequest registerRequest)
    {
        if ((await _userRepository.IsUserExists(registerRequest)))
            throw new RequestException(HttpStatusCode.BadRequest);

        UserDto user = await _userRepository.CreateUser(registerRequest);
        return user;
    }
}
