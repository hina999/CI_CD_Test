//-----------------------------------------------------------------------
// <copyright file="EfRepository.cs" company="Premiere Digital Services">
//     Copyright Premiere Digital Services. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
//using CordobaModels.Edmx;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Common;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
//using System.Linq;




using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using FXAdminTransferConnex.Data.Databases;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Data.Entity.Infrastructure;


namespace FXAdminTransferConnex.Data.Repository
{
    /// <summary>
    /// class EFREPOSITORY
    /// </summary>
    /// <typeparam name="T">return type</typeparam>
    public class EfRepository<T> : IRepository<T> where T : class
    {
        #region Fields

        /// <summary>
        /// The _context
        /// </summary>
        private readonly IDbContext _context;

        /// <summary>
        /// The _entities
        /// </summary>
        private IDbSet<T> _entities;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="EfRepository{T}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public EfRepository(IDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets no tracking when u read data for not edit
        /// </summary>
        public virtual IQueryable<T> AsNoTracking
        {
            get
            {
                return Entities.AsNoTracking();
            }
        }

        /// <summary>
        /// Gets whole table
        /// </summary>
        public virtual IQueryable<T> Table
        {
            get
            {
                return Entities;
            }
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <value>
        /// The entities.
        /// </value>
        protected virtual IDbSet<T> Entities
        {
            get { return _entities ?? (_entities = _context.Set<T>()); }
        }

        /// <summary>
        /// get record by id
        /// </summary>
        /// <param name="id">the id</param>
        /// <returns>
        /// return entity
        /// </returns>
        public virtual T GetById(object id)
        {
            return Entities.Find(id);
        }

        /// <summary>
        /// insert record
        /// </summary>
        /// <param name="entity">the entity</param>
        /// <exception cref="System.ArgumentNullException">return entity</exception>
        public virtual void Insert(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                Entities.Add(entity);

                ////this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbex)
            {
                string msg = dbex.EntityValidationErrors.Aggregate(string.Empty, (current1, validationErrors) =>
                    validationErrors.ValidationErrors.Aggregate(current1, (current, validationError) => 
                        current + (string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine)));

                Exception fail = new Exception(msg, dbex);
                throw fail;
            }
        }

        /// <summary>
        /// update record
        /// </summary>
        /// <param name="entity">the entity</param>
        /// <param name="changeState">if set to <c>true</c> [change state].</param>
        /// <exception cref="System.ArgumentNullException">return entity</exception>
        public virtual void Update(T entity, bool changeState = true)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                if (changeState)
                {
                    _context.Entry(entity).State = EntityState.Modified;
                }

                ////this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbex)
            {
                string msg = dbex.EntityValidationErrors.Aggregate(string.Empty, (current1, validationErrors) => 
                    validationErrors.ValidationErrors.Aggregate(current1, (current, validationError) => 
                        current + (Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage))));

                Exception fail = new Exception(msg, dbex);
                throw fail;
            }
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        public virtual void SaveChanges()
        {
            _context.SaveChanges();
        }

        /// <summary>
        /// delete record
        /// </summary>
        /// <param name="entity">the entity</param>
        /// <exception cref="System.ArgumentNullException">the entity</exception>
        public virtual void Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                _context.Entry(entity).State = EntityState.Deleted;
                Entities.Remove(entity);

                ////this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbex)
            {
                string msg = dbex.EntityValidationErrors.Aggregate(string.Empty, (current1, validationErrors) => validationErrors.ValidationErrors.Aggregate(current1, (current, validationError) => current + (Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage))));

                Exception fail = new Exception(msg, dbex);
                throw fail;
            }
        }

        /// <summary>
        /// Gets all lazy load.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="children">The children.</param>
        /// <returns>return entity</returns>
        public virtual IQueryable<T> GetAllLazyLoad(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] children)
        {
            IQueryable<T> query = Entities;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = children.Aggregate(query, (current, include) => current.Include(include));
            ////children.ToList().ForEach(x => query.Include(x));
            return query;
        }

        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">The Parameters</param>
        /// <returns>return entities</returns>
        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : class
        {
            _context.Db.CommandTimeout = 1500;
            return _context.ExecuteStoredProcedureList<TEntity>(commandText, parameters);
        }

        /// <summary>
        /// Executes the stored procedure.
        /// </summary>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>return element</returns>
        public IEnumerable<TElement> ExecuteStoredProcedure123<TElement>(string commandText, params object[] parameters)
        {
            //StringBuilder sp = new StringBuilder();
            //sp.Append(commandText);
            //foreach (var item in parameters)
            //{
            //    sp.Append(" @" + item);
            //}
            _context.Db.CommandTimeout = 1500;
            return _context.SqlQuery<TElement>(commandText, parameters);
        }

        public IEnumerable<TElement> ExecuteStoredProcedure<TElement>(string commandText, params object[] parameters)
        {
            _context.Db.CommandTimeout = 5000;
            //add parameters to command
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    DbParameter p = parameters[i] as DbParameter;
                    if (p == null)
                    {
                        throw new Exception("Not support parameter type");
                    }

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        ////output parameter
                        commandText += " output";
                    }
                }
            }
            return _context.SqlQuery<TElement>(commandText, parameters);
        }

        /// <summary>
        /// Executes the stored procedure list temporary.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// return list
        /// </returns>
        public IList<TEntity> ExecuteStoredProcedureListTmp<TEntity>(string commandText, params object[] parameters) where TEntity : class
        {
            _context.Db.CommandTimeout = 1500;
            return _context.ExecuteStoredProcedureList<TEntity>(commandText, parameters);
        }

        /// <summary>
        /// Executes the stored procedure.
        /// </summary>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// return List
        /// </returns>
        public IEnumerable<TElement> ExecuteStoredProceduretmp<TElement>(string commandText, params object[] parameters)
        {
            _context.Db.CommandTimeout = 1500;
            return _context.SqlQuery<TElement>(commandText, parameters);
        }
       
        #endregion
    }
}
