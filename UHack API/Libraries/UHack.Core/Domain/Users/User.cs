using System;
using System.Collections.Generic;

namespace UHack.Core.Domain.Users
{
    /// <summary>
    /// Represents a User
    /// </summary>
    public partial class User : BaseEntity
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public User()
        {
            this.UserGuid = Guid.NewGuid().ToString();
            this.PasswordFormat = PasswordFormat.Clear;
        }

        /// <summary>
        /// Gets or sets User Guid
        /// </summary>
        public string UserGuid { get; set; }

        /// <summary>
        /// Gets or sets Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or Sets First Name
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// Gets or Sets Last Name
        /// </summary>
        public string Lastname { get; set; }

        /// <summary>
        /// Gets or Sets Phone Number
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or Sets Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password format
        /// </summary>
        public PasswordFormat PasswordFormat
        {
            get { return (PasswordFormat)PasswordFormatId; }
            set { this.PasswordFormatId = (int)value; }
        }

        /// <summary>
        /// Gets or Sets Password Format Id
        /// </summary>
        public int PasswordFormatId { get; set; }

        /// <summary>
        /// Gets or Sets Password Salt
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// Gets or Sets Role Id
        /// </summary>
        public short RoleId { get; set; }

        /// <summary>
        /// Gets or Sets Is Active In Business
        /// </summary>
        public bool ActiveInBusiness { get; set; }

        /// <summary>
        /// Gets or Sets Active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// /Gets or Sets Deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or Sets Last IP Address
        /// </summary>
        public string LastIpAddress { get; set; }

        /// <summary>
        /// Gets or Sets Created On
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or Sets Last login date
        /// </summary>
        public DateTime LastLoginDate { get; set; }


        #region Navigation properties


        #endregion

    }
}
