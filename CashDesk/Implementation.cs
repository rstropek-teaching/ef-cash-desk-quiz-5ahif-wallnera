using CashDesk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashDesk
{
    public class DataAccess : IDataAccess
    {
        /// <summary>
        /// connects to a DB
        /// </summary>
        public void InitializeDatabaseAsync()
        {
            using(var db = new MyContext())
            {
            }
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

            using (var db = new MyContext())
            {
                var memberswithsamename = db.Members.Where(p => p.LastName.ToLower().Equals(lastName.ToLower())).ToArray();

                // Person is already inside with the same last name -> can't be added a second time
                if (memberswithsamename.Count() == 0)
                {
                    db.Members.Add(new Member { FirstName = firstName, LastName = lastName, Birthday = birthday });
                    db.SaveChanges();
                    var newmember = db.Members.Where(p => p.LastName.ToLower().Equals(lastName.ToLower())).ToArray()[0];
                    return newmember.MemberNumber;
                }
                else {
                    throw new DuplicateNameException();
                }
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

            using(var db = new MyContext())
            {
                
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



            return null;
        }

        /// <summary>
        /// removes a member from an membership
        /// </summary>
        /// <param name="memberNumber"></param>
        /// <returns>
        /// IMembership
        /// </returns>
        public IMembership CancelMembershipAsync(int memberNumber)
        {
            if (memberNumber < 0)
            {
                throw new ArgumentException();
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
        public Task DepositAsync(int memberNumber, decimal amount)
        {
            if (memberNumber < 0 || amount < 0 )
            {
                throw new ArgumentException();
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<IDepositStatistics>> GetDepositStatisticsAsync() 
            => throw new NotImplementedException();

        /// <summary>
        /// delets everything from the DB
        /// </summary>
        public void Dispose()
        {
        }
    }
}
