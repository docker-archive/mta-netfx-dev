using AutoMapper;
using SignUp.Messaging;
using SignUp.Messaging.Messages.Events;
using SignUp.Model;
using SignUp.Web.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SignUp.Web
{
    public partial class SignUp : Page
    {     
        private static Dictionary<string, Country> _Countries;
        private static Dictionary<string, Role> _Roles;

        public static void PreloadStaticDataCache()
        {
            Log.Info("Starting pre-load data cache");
            var stopwatch = Stopwatch.StartNew();

            _Countries = new Dictionary<string, Country>();
            _Roles = new Dictionary<string, Role>();
            using (var context = new SignUpDbEntities())
            {
                _Countries["-"] = context.Countries.First(x => x.CountryCode == "-");
                foreach (var country in context.Countries.Where(x=>x.CountryCode != "-").OrderBy(x => x.CountryName))
                {
                    _Countries[country.CountryCode] = country;
                }

                _Roles["-"] = context.Roles.First(x => x.RoleCode == "-");
                foreach (var role in context.Roles.Where(x => x.RoleCode != "-").OrderBy(x => x.RoleName))
                {
                    _Roles[role.RoleCode] = role;
                }
            }

            Log.Info("Completed pre-load data cache, took: {0}ms", stopwatch.ElapsedMilliseconds);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PopulateRoles();
                PopulateCountries();
            }
        }

        private void PopulateRoles()
        {
            ddlRole.Items.Clear();
            ddlRole.Items.AddRange(_Roles.Select(x => new ListItem(x.Value.RoleName, x.Key)).ToArray()); 
        }

        private void PopulateCountries()
        {
            ddlCountry.Items.Clear();
            ddlCountry.Items.AddRange(_Countries.Select(x => new ListItem(x.Value.CountryName, x.Key)).ToArray());
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            var country = _Countries[ddlCountry.SelectedValue];
            var role = _Roles[ddlRole.SelectedValue];

            var prospect = new Prospect
            {
                CompanyName = txtCompanyName.Text,
                EmailAddress = txtEmail.Text,
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                Country = country,
                Role = role
            };

            var eventMessage = new ProspectSignedUpEvent
            {
                Prospect = Mapper.Map<Entities.Prospect>(prospect),
                SignedUpAt = DateTime.UtcNow
            };

            MessageQueue.Publish(eventMessage);

            Server.Transfer("ThankYou.aspx");
        }
    }
}