using CashDesk.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashDesk
{
    public class DataAccess : IDataAccess
    {
        public MyContext db;
        /// <summary>
        /// connects to a DB
        /// </summary>
        public void InitializeDatabaseAsync()
        {
            this.db = new MyContext();
        }

        /// <summary>
        /// adds a member, but before checks if there is a member with the same name in the DB
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="birthday"></param>
        /// <returns></returns>
        public int AddMemberAsync(string firstName, string lastName, DateTime birthday)
        {
            if (firstName == null || lastName == null)
            {
                throw new ArgumentException();
            }

            if(this.db != null)
            {
                var memberswithsamename = this.db.Members.Where(p => p.LastName.ToLower().Equals(lastName.ToLower())).ToArray();

                // Person is already inside with the same last name -> can't be added a second time
                if (memberswithsamename.Count() == 0)
                {
                    this.db.Members.Add(new Member { FirstName = firstName, LastName = lastName, Birthday = birthday });
                    this.db.SaveChanges();
                    var newmember = this.db.Members.Where(p => p.LastName.ToLower().Equals(lastName.ToLower())).ToArray().First();
                    return newmember.MemberNumber;
                }
                else {
                    throw new DuplicateNameException();
                }
            }
            else {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// delets a member from the DB but before checks, if memberNumber is correct
        /// </summary>
        /// <param name="memberNumber"></param>
        public void DeleteMemberAsync(int memberNumber)
        {
            if(memberNumber < 0){
                throw new ArgumentException();
            }

            if(this.db != null)
            {
                var killmember = db.Members.Where(killmem => killmem.MemberNumber == memberNumber).First();
                this.db.Remove(killmember);
            }
            else {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// adds a member to a membership
        /// </summary>
        /// <param name="memberNumber"></param>
        /// <returns>
        /// IMembership
        /// </returns>
        public IMembership JoinMemberAsync(int memberNumber)
        {
            if (memberNumber < 0)
            {
                throw new ArgumentException();
            }

            if (this.db != null)
            {
                var newbie = this.db.Members.Where(mem => mem.MemberNumber == memberNumber).ToArray().First();

         

                if (this.db.Memberships.Where(ship => ship.Begin == null && ship.Member.Equals(newbie) || ship.Begin != null && ship.Member.Equals(newbie)).Count() != 0)
                {
                    throw new AlreadyMemberException();
                }

                else
                {
                    var ship = this.db.Memberships.Add(new Membership { Member = newbie, Begin = DateTime.Now });
                    this.db.SaveChanges();
                    
                    return ship.Entity;
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
            //return null
        }

        /// <summary>
        /// removes a member from an membership
        /// </summary>
        /// <param name="memberNumber"></param>
        /// <returns>
        /// IMembership
        /// </returns>
        public async Task<IMembership> CancelMembershipAsync(int memberNumber)
        {
            if (memberNumber < 0)
            {
                throw new ArgumentException();
            }
            if (this.db != null)
            {
                var killmember = await this.db.Members.Where(mem => mem.MemberNumber == memberNumber).FirstAsync();

                if (this.db.Memberships.Where(ship => ship.End != null && ship.Member.Equals(killmember)).Count() == 0)
                {
                    throw new NoMemberException();
                }
                var killmembership = await this.db.Memberships.Where(ship => ship.End != null && ship.Member.Equals(killmember)).FirstAsync();
                killmembership.End = DateTime.Now;
                await this.db.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException();
            }
            return null;
        }

        /// <summary>
        /// checks for correct numbers
        /// </summary>
        /// <param name="memberNumber"></param>
        /// <param name="amount"></param>
        /// <returns>
        /// Task
        /// </returns>
        public async Task DepositAsync(int memberNumber, decimal amount)
        {
            if (memberNumber < 0 || amount < 0 )
            {
                throw new ArgumentException();
            }
            if (this.db != null)
            {
                var paymember = await this.db.Members.Where(mem => mem.MemberNumber.Equals(memberNumber)).FirstAsync();
                var paymembership = await this.db.Memberships.Where(ship => ship.End != null && ship.Member.Equals(paymember)).FirstOrDefaultAsync();

                if (paymembership == null)
                {
                    throw new NoMemberException();
                }

                var deposit = this.db.Deposits.Add(new Deposit { Membership = paymembership, Amount = amount });
                this.db.SaveChanges();

            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<IDepositStatistics>> GetDepositStatisticsAsync()
        {
            if (this.db != null)
            {
                var deposit = await this.db.Deposits.GroupBy(dep => new { Year = dep.Membership.Begin.Year, Member = dep.Membership.Member }).Select(depstat => new DepositStatistics { Member = depstat.Key.Member, Year = depstat.Key.Year, TotalAmount = depstat.Sum(sum => sum.Amount) }).ToListAsync();
                return deposit;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// delets everything from the DB
        /// </summary>
        public void Dispose()
        {
        }
    }
}
