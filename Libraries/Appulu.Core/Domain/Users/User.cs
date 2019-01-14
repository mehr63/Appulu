using System;
using System.Collections.Generic;
using System.Linq;
using Appulu.Core.Domain.Common;
using Appulu.Core.Domain.Orders;

namespace Appulu.Core.Domain.Users
{
    /// <summary>
    /// Represents a user
    /// </summary>
    public partial class User : BaseEntity
    {
        private ICollection<ExternalAuthenticationRecord> _externalAuthenticationRecords;
        private ICollection<UserUserRoleMapping> _userUserRoleMappings;
        private ICollection<ShoppingCartItem> _shoppingCartItems;
        private ICollection<ReturnRequest> _returnRequests;
        protected ICollection<UserAddressMapping> _userAddressMappings;
        private IList<UserRole> _userRoles;

        public User()
        {
            this.UserGuid = Guid.NewGuid();
        }

        /// <summary>
        /// Gets or sets the user GUID
        /// </summary>
        public Guid UserGuid { get; set; }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the email that should be re-validated. Used in scenarios when a user is already registered and wants to change an email address.
        /// </summary>
        public string EmailToRevalidate { get; set; }

        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
        public string AdminComment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is tax exempt
        /// </summary>
        public bool IsTaxExempt { get; set; }

        /// <summary>
        /// Gets or sets the affiliate identifier
        /// </summary>
        public int AffiliateId { get; set; }

        /// <summary>
        /// Gets or sets the vendor identifier with which this user is associated (maganer)
        /// </summary>
        public int VendorId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this user has some products in the shopping cart
        /// <remarks>The same as if we run this.ShoppingCartItems.Count > 0
        /// We use this property for performance optimization:
        /// if this property is set to false, then we do not need to load "ShoppingCartItems" navigation property for each page load
        /// It's used only in a couple of places in the presenation layer
        /// </remarks>
        /// </summary>
        public bool HasShoppingCartItems { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is required to re-login
        /// </summary>
        public bool RequireReLogin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating number of failed login attempts (wrong password)
        /// </summary>
        public int FailedLoginAttempts { get; set; }

        /// <summary>
        /// Gets or sets the date and time until which a user cannot login (locked out)
        /// </summary>
        public DateTime? CannotLoginUntilDateUtc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user account is system
        /// </summary>
        public bool IsSystemAccount { get; set; }

        /// <summary>
        /// Gets or sets the user system name
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the last IP address
        /// </summary>
        public string LastIpAddress { get; set; }

        /// <summary>
        /// Gets or sets the date and time of entity creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last login
        /// </summary>
        public DateTime? LastLoginDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of last activity
        /// </summary>
        public DateTime LastActivityDateUtc { get; set; }

        /// <summary>
        ///  Gets or sets the store identifier in which user registered
        /// </summary>
        public int RegisteredInStoreId { get; set; }

        /// <summary>
        /// Gets or sets the billing address identifier
        /// </summary>
        public int? BillingAddressId { get; set; }

        /// <summary>
        /// Gets or sets the shipping address identifier
        /// </summary>
        public int? ShippingAddressId { get; set; }

        #region Navigation properties

        /// <summary>
        /// Gets or sets user generated content
        /// </summary>
        public virtual ICollection<ExternalAuthenticationRecord> ExternalAuthenticationRecords
        {
            get => _externalAuthenticationRecords ?? (_externalAuthenticationRecords = new List<ExternalAuthenticationRecord>());
            protected set => _externalAuthenticationRecords = value;
        }

        /// <summary>
        /// Gets or sets user roles
        /// </summary>
        public virtual IList<UserRole> UserRoles
        {
            get => _userRoles ?? (_userRoles = UserUserRoleMappings.Select(mapping => mapping.UserRole).ToList());
        }

        /// <summary>
        /// Gets or sets user-user role mappings
        /// </summary>
        public virtual ICollection<UserUserRoleMapping> UserUserRoleMappings
        {
            get => _userUserRoleMappings ?? (_userUserRoleMappings = new List<UserUserRoleMapping>());
            protected set => _userUserRoleMappings = value;
        }

        /// <summary>
        /// Gets or sets shopping cart items
        /// </summary>
        public virtual ICollection<ShoppingCartItem> ShoppingCartItems
        {
            get => _shoppingCartItems ?? (_shoppingCartItems = new List<ShoppingCartItem>());
            protected set => _shoppingCartItems = value;
        }

        /// <summary>
        /// Gets or sets return request of this user
        /// </summary>
        public virtual ICollection<ReturnRequest> ReturnRequests
        {
            get => _returnRequests ?? (_returnRequests = new List<ReturnRequest>());
            protected set => _returnRequests = value;
        }

        /// <summary>
        /// Default billing address
        /// </summary>
        public virtual Address BillingAddress { get; set; }

        /// <summary>
        /// Default shipping address
        /// </summary>
        public virtual Address ShippingAddress { get; set; }

        /// <summary>
        /// Gets or sets user addresses
        /// </summary>
        public IList<Address> Addresses => UserAddressMappings.Select(mapping => mapping.Address).ToList();

        /// <summary>
        /// Gets or sets user-address mappings
        /// </summary>
        public virtual ICollection<UserAddressMapping> UserAddressMappings
        {
            get => _userAddressMappings ?? (_userAddressMappings = new List<UserAddressMapping>());
            protected set => _userAddressMappings = value;
        }

        #endregion
    }
}