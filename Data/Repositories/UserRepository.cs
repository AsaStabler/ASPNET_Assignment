﻿using Data.Contexts;
using Data.Entities;
using Domain.Models;

namespace Data.Repositories;

public interface IUserRepository : IBaseRepository<UserEntity, User>
{
}

public class UserRepository(DataContext context) : BaseRepository<UserEntity, User>(context), IUserRepository
{
}
