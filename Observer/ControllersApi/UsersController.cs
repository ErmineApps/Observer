﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Observer;

namespace Observer.ControllersApi
{
    public class UsersController : ApiController
    {
        private ObserverDBEntities db = new ObserverDBEntities();

        // GET: api/Users
        public IQueryable<Users> GetUsers()
        {
            return db.Users;
        }

        // GET: api/Users/5
        [ResponseType(typeof(Users))]
        public IHttpActionResult GetUsers(int id)
        {
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }

        ///регистрация нового пользователя
        // POST: api/Users
        [ResponseType(typeof(Users))]
        public IHttpActionResult PostUsers(Users users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userOld = db.Users.Where(p => p.Name == users.Name);
            if (userOld.ToList().Count == 0)
            {
                db.Users.Add(users);
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = users.Id }, users);
            }
            else
            {
                return NotFound();
            }
        }

        ///авторизация пользователя
        // POST: api/Users
        [ResponseType(typeof(Users))]
        public IHttpActionResult Authorization(Users users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userOld = db.Users.Where(p => p.Name == users.Name);
            if (userOld.ToList().Count == 0)
            {
                db.Users.Add(users);
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = users.Id }, users);
            }
            else
            {
                return NotFound();
            }
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUsers(int id, Users users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != users.Id)
            {
                return BadRequest();
            }

            db.Entry(users).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // DELETE: api/Users/5
        [ResponseType(typeof(Users))]
        public IHttpActionResult DeleteUsers(int id)
        {
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return NotFound();
            }

            db.Users.Remove(users);
            db.SaveChanges();

            return Ok(users);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsersExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}