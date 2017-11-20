﻿using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Rescue_911
{
    public partial class Login_View : Special_View
    {
        public event EventHandler LoginButton_Click;

        // CONSTRUCTORS
        //To-display the view.
        public Login_View(ref Shared_Data xSD) : base(ref xSD, "Welcome", true, SystemColors.MenuHighlight)
        {
            InitializeComponent();
        }

        //To-instantiate the view.
        public Login_View() : base("Welcome", true, System.Drawing.SystemColors.MenuHighlight)
        { }
        //

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == string.Empty && txtPassword.Text.Trim() == string.Empty)
            {
                txtName.Focus();
                return;
            }

            lbName.Visible = false;
            lbPassword.Visible = false;
            string nameFound = string.Empty;

            foreach (EMT iEMT in SD.EMTs)
            {
                if (txtName.Text != iEMT.GetName())
                    continue;

                nameFound = iEMT.GetName();

                if (txtPassword.Text != iEMT.GetPassword())
                    continue;

                this.Hide();

                // Adding accessible Forms for an EMT
                List<Type> AccessibleViews = new List<Type>();
                AccessibleViews.Add(typeof(Response_Team_Information_View));
                AccessibleViews.Add(typeof(EMT_Login_Shift_View));
                AccessibleViews.Add(typeof(Dispatch_Report_View));
                AccessibleViews.Add(typeof(Patient_Information_View));
                AccessibleViews.Add(typeof(Login_View));

                // Accessing the main form and telling it what to do.
                // Reference: https://social.msdn.microsoft.com/Forums/en-US/99df9c07-c117-465a-9207-fa3534982021/how-to-get-the-mainform-reference?forum=winforms
                ((Main_Form)Application.OpenForms[0]).SetSideBar(ref SD, AccessibleViews, iEMT);

                LoginButton_Click?.Invoke(this, e);
            }

            if (nameFound == string.Empty)
            {
                txtName.Focus();
                lbName.Visible = true;
            }
            else
            {
                txtPassword.Focus();
                lbPassword.Visible = true;
            }
        }

        private void btnLoginOther_Click(object sender, EventArgs e)
        {
            this.Hide();

            // Adding accessible Forms for a somebody
            List<Type> AccessibleViews = new List<Type>();

            AccessibleViews.Add(typeof(Call_View));
            AccessibleViews.Add(typeof(Emergency_Management_View));
            AccessibleViews.Add(typeof(Response_Team_Information_View));
            AccessibleViews.Add(typeof(EMT_Login_Shift_View));
            AccessibleViews.Add(typeof(Base_Station_Records_View));
            AccessibleViews.Add(typeof(Dispatch_Related_Times_View));
            AccessibleViews.Add(typeof(Dispatch_Report_View));
            AccessibleViews.Add(typeof(Link_Patient_View));
            AccessibleViews.Add(typeof(Patient_Information_View));
            AccessibleViews.Add(typeof(Invoice_View));
            AccessibleViews.Add(typeof(Login_View));

            Person fakePerson = new Person();
            fakePerson.SetName("Other");
            fakePerson.SetLast_Name("Person");
            fakePerson.SetAddress("Cupertino, California");


            ((Main_Form)Application.OpenForms[0]).SetSideBar(ref SD, AccessibleViews, fakePerson);

            LoginButton_Click?.Invoke(this, e);
        }

        // To-Do: Adjust to a View
        private void Login_Form_Activated(object sender, EventArgs e)
        {
            txtPassword.Clear();
            lbName.Visible = false;
            lbPassword.Visible = false;
        }
    }
}