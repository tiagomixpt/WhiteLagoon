﻿using System.Linq.Expressions;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interface
{
	public interface IRepository<T> where T : class
	{
		IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
		T Get(Expression<Func<T, bool>> filter, string? includeProperties = null);

		void Add(T entity);
		bool Any(Expression<Func<T, bool>> filter);
		void Remove(T entity);
	}
}
