using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace VipTest.Utlity.Basic;

public interface IBaseRepository<T, TId>
{
    Task<T> Add(T t);
    T AddNow(T t);
    Task<T?> Remove(TId id);
    Task<List<T>> RemoveAll(List<TId> ids);
    Task<T?> Update(T t, TId id);
    Task<List<T>> UpdateAll(List<T> data);
    Task<T?> GetById(TId id);
    Task<(List<TDto> data, int totalCount)> GetAll<TDto>(int pageNumber = 0, int pageSize = 30);

    Task<(List<TDto> data, int totalCount)> GetAll<TDto>(Expression<Func<T, bool>>? predicate,
        int pageNumber = 0, int pageSize = 30
    );

    Task<(List<T> data, int totalCount)> GetAll(int pageNumber = 0, int pageSize = 30);

    Task<(List<T> data, int totalCount)> GetAll(Expression<Func<T, bool>>? predicate,
        int pageNumber = 0, int pageSize = 30
    );

    Task<(List<T> data, int totalCount)> GetAll(
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
        int pageNumber = 0, int pageSize = 30
    );

    Task<(List<T> data, int totalCount)> GetAll(Expression<Func<T, bool>>? predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
        int pageNumber = 0, int pageSize = 30
    );

    Task<TDto?> Get<TDto>(Expression<Func<T, bool>>? predicate);
    Task<T?> Get(Expression<Func<T, bool>>? predicate);

    Task<T?> Get(Expression<Func<T, bool>>? predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include);

    Task<int?> Count();
    Task<int?> Count(Expression<Func<T, bool>>? predicate);
}

public abstract class BaseRepository<T, TId> : IBaseRepository<T, TId>
    where T : BaseEntity<TId>
{
    private readonly DbContext _context;
    private readonly IMapper _mapper;

    protected BaseRepository(DbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<T> Add(T t)
    {
        await _context.Set<T>().AddAsync(t);
        await _context.SaveChangesAsync();
        return t;
    }

    public T AddNow(T t)
    {
        _context.Set<T>().Add(t);
        _context.SaveChanges();
        return t;
    }

    public async Task<T?> Remove(TId id)
    {
        var result = await GetById(id);
        if (result == null) return null;
        _context.Set<T>().Remove(result);
        await _context.SaveChangesAsync();
        return result;
    }

    public async Task<List<T>> RemoveAll(List<TId> ids)
    {
        var data = await _context.Set<T>().Where(x => ids.Contains(x.Id)).ToListAsync();
        foreach (var t in data)
        {
            _context.Set<T>().Remove(t);
        }

        await _context.SaveChangesAsync();
        return data;
    }

    public async Task<T?> Update(T t, TId id)
    {
        var result = await GetById(id);
        if (result == null) return null;
        _mapper.Map(t, result);
        await _context.SaveChangesAsync();
        return result;
    }

    public Task<List<T>> UpdateAll(List<T> data)
    {
        foreach (var t in data)
        {
            _context.Entry(t).State = EntityState.Modified;
        }

        _context.SaveChanges();
        return Task.FromResult(data);
    }

    public async Task<T?> GetById(TId id) =>
        await _context.Set<T>().FirstOrDefaultAsync(entity => entity.Id != null && entity.Id.Equals(id));

    public async Task<(List<TDto> data, int totalCount)> GetAll<TDto>(int pageNumber = 1, int pageSize = 30) =>
        await GetAll<TDto>(null, pageNumber, pageSize);

    public async Task<(List<TDto> data, int totalCount)> GetAll<TDto>(
        Expression<Func<T, bool>>? predicate,
        int pageNumber = 1, // Change default pageNumber to 1
        int pageSize = 30
    )
    {
        // Prepare the query
        var query = predicate == null
            ? _context.Set<T>()
            : _context.Set<T>().Where(predicate);

        query = query.OrderByDescending(model => model.CreatedAt);

        // Get the total count before pagination
        var totalCount = await query.CountAsync();

        // Apply pagination
        var data = await query
            .Skip(pageSize * (pageNumber - 1)) // Skip the items for previous pages
            .Take(pageSize) // Take only the items for the current page
            .ProjectTo<TDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return (data, totalCount);
    }

    public async Task<(List<T> data, int totalCount)> GetAll(
        Expression<Func<T, bool>>? predicate,
        int pageNumber = 1, // Change default pageNumber to 1
        int pageSize = 30
    ) => await GetAll(predicate, null, pageNumber, pageSize);

    public async Task<(List<T> data, int totalCount)> GetAll(
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include, 
        int pageNumber = 1, // Change default pageNumber to 1
        int pageSize = 30
    ) => await GetAll(null, include, pageNumber, pageSize);

    public async Task<(List<T> data, int totalCount)> GetAll(
        Expression<Func<T, bool>>? predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include,
        int pageNumber = 1, // Change default pageNumber to 1
        int pageSize = 30
    )
    {
        // Prepare the query
        var query = predicate == null
            ? _context.Set<T>()
            : _context.Set<T>().Where(predicate);
        
        query = query.OrderByDescending(model => model.CreatedAt);
        query = include != null ? include(query) : query;

        // Get the total count before pagination
        var totalCount = await query.CountAsync();

        // Apply pagination
        var data = await query
            .Skip(pageSize * (pageNumber - 1)) // Skip the items for previous pages
            .Take(pageSize) // Take only the items for the current page
            .ToListAsync();

        return (data, totalCount);
    }

    public async Task<(List<T> data, int totalCount)> GetAll(int pageNumber = 1, int pageSize = 30)
    {
        return await GetAll((Expression<Func<T, bool>>?)null, pageNumber, pageSize);
    }


    public async Task<TDto?> Get<TDto>(Expression<Func<T, bool>>? predicate)
    {
        var query = _context.Set<T>().AsQueryable();
        query = predicate != null ? query.Where(predicate) : query;
        return await query.ProjectTo<TDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
    }

    public async Task<T?> Get(Expression<Func<T, bool>>? predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include)
    {
        var query = _context.Set<T>().AsQueryable();
        query = predicate != null ? query.Where(predicate) : query;
        query = include != null ? include(query) : query;
        return await query.FirstOrDefaultAsync();
    }

    public async Task<T?> Get(Expression<Func<T, bool>>? predicate) =>
        await (predicate != null ? _context.Set<T>().Where(predicate) : _context.Set<T>())
            .FirstOrDefaultAsync();

    public async Task<int?> Count() => await Count(null);

    public async Task<int?> Count(Expression<Func<T, bool>>? predicate) =>
        predicate == null
            ? await _context.Set<T>().CountAsync()
            : await _context.Set<T>().CountAsync(predicate);
}
