using Data.Contexts;
using Data.Models;
using Domain.Extensions;

using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Data.Repositories;

public interface IBaseRepository<TEntity, TModel> where TEntity : class
{
    Task<RepsitoryResult<bool>> AddAsync(TEntity entity);
    Task<RepsitoryResult<bool>> DeleteAsync(TEntity entity);
    Task<RepsitoryResult<bool>> ExistAsync(Expression<Func<TEntity, bool>> findBy);
    Task<RepsitoryResult<IEnumerable<TModel>>> GetAllAsync(bool orderByDescending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? where = null, params Expression<Func<TEntity, object>>[] includes);
    Task<RepsitoryResult<IEnumerable<TSelect>>> GetAllAsync<TSelect>(Expression<Func<TEntity, TSelect>> selector, bool orderByDescending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? where = null, params Expression<Func<TEntity, object>>[] includes);
    Task<RepsitoryResult<TModel>> GetAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes);
    Task<RepsitoryResult<bool>> UpdateAsync(TEntity entity);
}

public abstract class BaseRepository<TEntity, TModel> : IBaseRepository<TEntity, TModel> where TEntity : class
{
    protected readonly DataContext _context;
    protected readonly DbSet<TEntity> _table;

    public BaseRepository(DataContext context)
    {
        _context = context;
        _table = _context.Set<TEntity>();
    }

    #region BaseRepository with Dynamic Mapping, and Get methods with Extended funtionality

    public virtual async Task<RepsitoryResult<bool>> AddAsync(TEntity entity)
    {
        if (entity == null)
            return new RepsitoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entity can't be null." };

        try
        {
            _table.Add(entity);
            await _context.SaveChangesAsync();
            return new RepsitoryResult<bool> { Succeeded = true, StatusCode = 201 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepsitoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }

    /* GetAllAsync without filtration, selects all (*) properties in _table */
    public virtual async Task<RepsitoryResult<IEnumerable<TModel>>> GetAllAsync(bool orderByDescending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? where = null, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _table;

        if (where != null)
            query = query.Where(where);

        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);

        if (sortBy != null)
            query = orderByDescending
                ? query.OrderByDescending(sortBy)
                : query.OrderBy(sortBy);

        var entities = await query.ToListAsync();
        var result = entities.Select(entity => entity.MapTo<TModel>());
        return new RepsitoryResult<IEnumerable<TModel>> { Succeeded = true, StatusCode = 200, Result = result };
    }

    /* GetAllAsync with filtration, selected properties are defined in the input parameter selector */
    public virtual async Task<RepsitoryResult<IEnumerable<TSelect>>> GetAllAsync<TSelect>(Expression<Func<TEntity, TSelect>> selector, bool orderByDescending = false, Expression<Func<TEntity, object>>? sortBy = null, Expression<Func<TEntity, bool>>? where = null, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _table;

        if (where != null)
            query = query.Where(where);

        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);

        if (sortBy != null)
            query = orderByDescending
                ? query.OrderByDescending(sortBy)
                : query.OrderBy(sortBy);

        var entities = await query.Select(selector).ToListAsync();
        var result = entities.Select(entity => entity!.MapTo<TSelect>());
        return new RepsitoryResult<IEnumerable<TSelect>> { Succeeded = true, StatusCode = 200, Result = result };
    }

    public virtual async Task<RepsitoryResult<TModel>> GetAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _table;

        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);

        var entity = await query.FirstOrDefaultAsync(where);
        if (entity == null)
            return new RepsitoryResult<TModel> { Succeeded = false, StatusCode = 404, Error = "Entity not found." };

        var result = entity.MapTo<TModel>();
        return new RepsitoryResult<TModel> { Succeeded = true, StatusCode = 200, Result = result };
    }

    public virtual async Task<RepsitoryResult<bool>> ExistAsync(Expression<Func<TEntity, bool>> findBy)
    {
        var exists = await _table.AnyAsync(findBy);
        return !exists
            ? new RepsitoryResult<bool> { Succeeded = false, StatusCode = 404, Error = "Entity not found." }
            : new RepsitoryResult<bool> { Succeeded = true, StatusCode = 200 };
    }

    public virtual async Task<RepsitoryResult<bool>> UpdateAsync(TEntity entity)
    {
        if (entity == null)
            return new RepsitoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entity can't be null." };

        try
        {
            _table.Update(entity);
            await _context.SaveChangesAsync();
            return new RepsitoryResult<bool> { Succeeded = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepsitoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }

    public virtual async Task<RepsitoryResult<bool>> DeleteAsync(TEntity entity)
    {
        if (entity == null)
            return new RepsitoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entity can't be null." };

        try
        {
            //Context.Entry(entity).State = EntityState.Detached;
            _context.ChangeTracker.Clear();

            _table.Remove(entity);
            await _context.SaveChangesAsync();
            return new RepsitoryResult<bool> { Succeeded = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepsitoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }
    #endregion

    #region BaseRepository, Get methods with Dynamic Mapping, but without Extended functionality

    /*
    public virtual async Task<RepsitoryResult<IEnumerable<TModel>>> GetAllAsync()
    {
        var entities = await _table.ToListAsync();

        var result = entities.Select(entity => entity.MapTo<TModel>());

        return new RepsitoryResult<IEnumerable<TModel>> { Succeeded = true, StatusCode = 200, Result = result };
    }

    public virtual async Task<RepsitoryResult<TModel>> GetAsync(Expression<Func<TEntity, bool>> findBy)
    {
        var entity = await _table.FirstOrDefaultAsync(findBy);
        if (entity == null)
            return new RepsitoryResult<TModel> { Succeeded = false, StatusCode = 404, Error = "Entity not found." };

        var result = entity.MapTo<TModel>();
        return new RepsitoryResult<TModel> { Succeeded = true, StatusCode = 200, Result = result };
    }
    */

    #endregion

    #region BaseRepository with RepositoryResult
    /*
    public virtual async Task<RepsitoryResult<bool>> AddAsync(TEntity entity)
    {
        if (entity == null)    
            return new RepsitoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entity can't be null." };

        try
        {
            _table.Add(entity);
            await _context.SaveChangesAsync();
            return new RepsitoryResult<bool> { Succeeded = true, StatusCode = 201 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepsitoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }

    public virtual async Task<RepsitoryResult<IEnumerable<TEntity>>> GetAllAsync()
    {
        var entities = await _table.ToListAsync();
        return new RepsitoryResult<IEnumerable<TEntity>> { Succeeded = true, StatusCode = 200, Result = entities };
    }

    public virtual async Task<RepsitoryResult<TEntity>> GetAsync(Expression<Func<TEntity, bool>> findBy)
    {
        var entity = await _table.FirstOrDefaultAsync(findBy);
        return entity == null
            ? new RepsitoryResult<TEntity> { Succeeded = false, StatusCode = 404, Error = "Entity not found." }
            : new RepsitoryResult<TEntity> { Succeeded = true, StatusCode = 200, Result = entity };
    }

    public virtual async Task<RepsitoryResult<bool>> ExistAsync(Expression<Func<TEntity, bool>> findBy)
    {
        var exists = await _table.AnyAsync(findBy);
        return !exists
            ? new RepsitoryResult<bool> { Succeeded = false, StatusCode = 404, Error = "Entity not found." }
            : new RepsitoryResult<bool> { Succeeded = true, StatusCode = 200 };
    }

    public virtual async Task<RepsitoryResult<bool>> UpdateAsync(TEntity entity)
    {
        if (entity == null)
            return new RepsitoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entity can't be null." };

        try
        {
            _table.Update(entity);
            await _context.SaveChangesAsync();
            return new RepsitoryResult<bool> { Succeeded = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepsitoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }

    public virtual async Task<RepsitoryResult<bool>> DeleteAsync(TEntity entity)
    {
        if (entity == null)
            return new RepsitoryResult<bool> { Succeeded = false, StatusCode = 400, Error = "Entity can't be null." };

        try
        {
            _table.Remove(entity);
            await _context.SaveChangesAsync();
            return new RepsitoryResult<bool> { Succeeded = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepsitoryResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }
    */
    #endregion

    #region Simple BaseRepository 
    /*
        //using Microsoft.AspNetCore.Http;
        //using static System.Runtime.InteropServices.JavaScript.JSType;

        public virtual async Task<bool> AddAsync(TEntity entity) 
        {
            if (entity == null) 
                return false;

            try
            {
                _table.Add(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) 
            { 
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var entities = await _table.ToListAsync();
            return entities;
        }

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> findBy)
        {
            var entity = await _table.FirstOrDefaultAsync(findBy);
            return entity ?? null!;
        }

        public virtual async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> findBy)
        {
            var exists = await _table.AnyAsync(findBy);
            return exists;
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            if (entity == null)
                return false;

            try
            {
                _table.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
            if (entity == null)
                return false;

            try
            {
                _table.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    */
    #endregion
}
