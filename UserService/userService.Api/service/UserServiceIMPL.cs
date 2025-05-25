using System;
using System.Threading.Tasks;
using userService.Api.DTO;
using userService.Api.entity;
using userService.Api.repository.interfaces;
using userService.Api.service.interfaces;

namespace userService.Api.service
{
    public class UserServiceIMPL : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserServiceIMPL(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> createUserAsync(UserDTO user)
        {
            var entity = ToEntity(user);
            var result = await _userRepository.createUserAsync(entity);

            if (!result)
                throw new Exception("User creation failed.");

            return result;
        }

        public async Task<UserDTO> getUsersAsync(long userId)
        {
            var user = await _userRepository.getUsersAsync(userId);

            if (user == null)
                throw new Exception($"User with ID {userId} not found.");

            return ToDTO(user);
        }

        public async Task<bool> loginUserAsync(string userName, string password)
        {
            var user = await _userRepository.loginUserAsync(userName, password);

            if (user == null)
                throw new Exception("Invalid username or password.");

            return true;
        }

        public UserDTO ToDTO(UserEntity entity)
        {
            return new UserDTO
            {
                userId = entity.userId,
                userName = entity.userName,
                password = entity.password,
                FirstName = entity.firstName,
                LastName = entity.lastName
            };
        }

        public UserEntity ToEntity(UserDTO dto)
        {
            return new UserEntity
            {
                userId = dto.userId,
                userName = dto.userName,
                password = dto.password,
                firstName = dto.FirstName,
                lastName = dto.LastName
            };
        }
    }
}
