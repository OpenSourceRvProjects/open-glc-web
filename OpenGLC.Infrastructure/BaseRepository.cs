using Microsoft.EntityFrameworkCore;
using OpenGLC.Data.Entities;
using OpenGLC.Infrastructure.Interfaces;
using OpenGLC.Models.Pagination;
using System.Linq.Expressions;

namespace OpenGLC.Infrastructure
{
	public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
	{

		private readonly OpenglclevelContext _dbContext;

		public BaseRepository(OpenglclevelContext dbContext)
		{
			_dbContext = dbContext;
		}

		public virtual async Task AddAsync(TEntity entity)
		{
			await _dbContext.AddAsync(entity);
		}

		public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
		{
			await _dbContext.AddRangeAsync(entities);
		}

		public virtual void DeleteAsync(TEntity entity)
		{
			_dbContext.Remove(entity);
		}

		public virtual void DeleteRangeAsync(IEnumerable<TEntity> entities)
		{
			_dbContext.RemoveRange(entities);
		}

		public virtual IQueryable<TEntity> FindByExpresion(Expression<Func<TEntity, bool>> expression)
		{
			return _dbContext.Set<TEntity>().Where(expression);
		}

		public async Task<IEnumerable<TEntity>> GetAllAsync()
		{
			return await _dbContext.Set<TEntity>().ToListAsync();
		}

		public async Task<PaginationListEntityModel<TEntity>> GetAllPagedAsync<T>(int page, int pageSize, Expression<Func<TEntity, T>> sorter, IEnumerable<TEntity> filterableEntityQry = null, bool orderByDesc = false)
		{
			IEnumerable<TEntity> allData = filterableEntityQry == null ? await GetAllAsync() : filterableEntityQry;

			var dataCount = allData.Count();
			var totalPages = Math.Ceiling((decimal)dataCount / pageSize);

			var orderedData = orderByDesc ? allData.AsQueryable().OrderByDescending(sorter) : allData.AsQueryable().OrderBy(sorter);

			var pagedQuery = orderedData.Skip(page * pageSize).Take(pageSize).ToList();

			PaginationListEntityModel<T> response = new PaginationListEntityModel<T>();

			return new PaginationListEntityModel<TEntity>()
			{
				PagedList = pagedQuery,
				TotalPages = totalPages,
				PageNumber = page,
				TotalCount = dataCount,
			};

		}

		public virtual async Task<TEntity> GetByIdAsync(Guid id)
		{
			return await _dbContext.Set<TEntity>().FindAsync(id);
		}

		public void UpdateAsync(TEntity entity)
		{
			_dbContext.Update(entity);
		}
	}
}
