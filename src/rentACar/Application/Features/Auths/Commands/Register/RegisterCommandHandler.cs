﻿using Application.Features.Auths.Dtos;
using Application.Features.Auths.Rules;
using Application.Services.AuthService;
using Application.Services.Repositories;
using Core.Security.Entities;
using Core.Security.Hashing;
using Core.Security.JWT;
using MediatR;

namespace Application.Features.Auths.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisteredDto>
    {
        private readonly IUserRepository _userRepository;

        private readonly AuthBusinessRules _authBusinessRules;

        private readonly IAuthService _authService;

        public RegisterCommandHandler(IUserRepository userRepository, AuthBusinessRules authBusinessRules, IAuthService authService)
        {
            _userRepository = userRepository;
            _authBusinessRules = authBusinessRules;
            _authService = authService;
        }

        public async Task<RegisteredDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            await _authBusinessRules.EmailCanNotBeDublicatedWhenRegistered(request.UserForRegisterDto.Email);


            HashingHelper.CreatePasswordHash(request.UserForRegisterDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User newUser = new()
            {
                Email = request.UserForRegisterDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt=passwordSalt,
                FirstName=request.UserForRegisterDto.FirstName,
                LastName=request.UserForRegisterDto.LastName,
                Status=true
            };

            User createdUser = await _userRepository.AddAsync(newUser);


            AccessToken createdAccessToken = await _authService.CreateAccessToken(createdUser);
            RefreshToken createdRefreshToken = await  _authService.CreateRefreshToken(createdUser,request.IpAddress);
            RefreshToken addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);

            RegisteredDto registeredDto = new()
            {
                AccessToken = createdAccessToken,
                RefreshToken = addedRefreshToken,
            };

            return registeredDto;

        }
    }
}
