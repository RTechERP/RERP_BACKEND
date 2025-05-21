    using Microsoft.EntityFrameworkCore;
    using RERPAPI.IRepo;
    using RERPAPI.Model.Context;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace RERPAPI.Repo
    {
        public class GenericRepo<T> : IGenericRepo<T> where T : class, new()
        {
            protected RTCContext db { get; set; }
            protected DbSet<T> table = null;
            public GenericRepo()
            {
                db = new RTCContext();
                table = db.Set<T>();
            }

            public GenericRepo(RTCContext db)
            {
                this.db = db;
                table = db.Set<T>();
            }

            public List<T> GetAll()
            {
                try
                {
                    return table.ToList();
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.ToString());
                }
            }

            public T GetByID(int id)
            {
                try
                {
                    T model = table.Find(id) ?? new T();
                    return model;
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.ToString());
                }
            }

            public int Create(T item)
            {
                try
                {
                    table.Add(item);
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.ToString());
                }
            }

            public int CreateRange(List<T> items)
            {
                try
                {
                    table.AddRange(items);
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.ToString());
                }
            }

            public int Update(T item)
            {
                try
                {
                    table.Attach(item);
                    db.Entry(item).State = EntityState.Modified;
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.ToString());
                }
            }

            public int Delete(int id)
            {
                try
                {
                    T model = table.Find(id) ?? new T();
                    table.Remove(model);
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.ToString());
                }
            }

            public async Task<int> CreateAsync(T item)
            {
                try
                {
                    await table.AddAsync(item);
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.ToString());
                }
            }

            public async Task<int> UpdateAsync(T item)
            {
                try
                {
                    table.Attach(item);
                    db.Entry(item).State = EntityState.Modified;
                    return await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.ToString());
                }
            }

            public async Task<int> DeleteAsync(int id)
            {
                try
                {
                    T model = await table.FindAsync(id) ?? new T();
                    table.Remove(model);
                    return await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.ToString());
                }
            }

            public async Task<int> CreateRangeAsync(List<T> items)
            {
                try
                {
                    await table.AddRangeAsync(items);
                    return await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.ToString());
                }
            }

            public int DeleteRange(List<T> items)
            {

                try
                {
                    table.RemoveRange(items);
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }

            public async Task<int> DeleteRangeAsync(List<T> items)
            {
                try
                {
                    table.RemoveRange(items);
                    return await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }

        }
    }
