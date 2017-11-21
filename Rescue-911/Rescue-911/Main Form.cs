﻿using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Reflection;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Rescue_911
{
    public partial class Main_Form : Form
    {
        // DATA STRUCTURE
        //Primitives
        private Shared_Data SD;

        //Composite Data
        private Special_View Current_View;
        //


        public Main_Form(ref Shared_Data xSD)
        {
            // Setting up the form.
            // To-Do: Add dynamic ID type identification

            SD = xSD;

            SD.MainForms.Add(this);

            InitializeComponent();
        }

        // SETTING UP THE VIEWS
        //Updating the Current_View
        public void View_Switch(Special_View sender)
        {
            sideBar.MenuElement_Changed(null,null);

            Current_View = sender;

            Current_View.Show();
        }

        //Displaying the Login screen when the app is launched.
        private void Main_Form_Load(object sender, EventArgs e)
        {
            Login_Prepare(sender, e);
        }

        //Displaying the Main screen when the Login screen is closed.
        private void MainView_Prepare(object sender, EventArgs e)
        {
            if (sideBar.Visible == false)
            {
                sideBar.CallButton_Click += new EventHandler(CallView_Prepare);
                sideBar.EmergencyManagement_Click += new EventHandler(EmergencyManagementView_Prepare);
                sideBar.ResponseTeamInfoButton_Click += new EventHandler(ResponseTeamInfoView_Prepare);
                sideBar.EMTLoginShiftButton_Click += new EventHandler(EMTLoginShiftView_Prepare);
                sideBar.BaseStationRecordsButton_Click += new EventHandler(BaseStationRecordsView_Prepare);
                sideBar.DispatchRelatedTimesButton_Click += new EventHandler(DispatchRelatedTimesView_Prepare);
                sideBar.DispatchReportButton_Click += new EventHandler(DispatchReportView_Prepare);
                sideBar.LinkPatientButton_Click += new EventHandler(LinkPatientView_Prepare);
                sideBar.PatientInformationButton_Click += new EventHandler(PatientInformationView_Prepare);
                sideBar.InvoiceButton_Click += new EventHandler(InvoiceView_Prepare);
                sideBar.LogoutButton_Click += new EventHandler(Logout_Prepare);

                sideBar.Visible = true;
            }

            Main_View MainView = (Main_View)SetView(typeof(Main_View));
        }

        //Event for displaying the Login screen.
        private void Login_Prepare(object sender, EventArgs e)
        {
            Login_View LoginView = (Login_View)SetView(typeof(Login_View));

            LoginView.LoginButton_Click += new EventHandler(MainView_Prepare);
        }

        //Event for trashing out the Current View and going back to Login screen.
        private void Logout_Prepare(object sender, EventArgs e)
        {
            // Disposing all Views.
            while (false != true)
            {
                if (Current_View.GetPrevious_View() != null)
                {
                    Special_View ViewTemp = Current_View;
                    Current_View = Current_View.GetPrevious_View();
                    ViewTemp.Dispose();
                }
                else
                {
                    Current_View.Dispose();
                    break;
                }
            }

            // Preparing the login screen.
            Login_Prepare(sender, e);
        }

        private void CallView_Prepare(object sender, EventArgs e)
        {
            Call_View CallView = (Call_View)SetTypicalView(typeof(Call_View));

            CallView.LinkEmergencyButton_Click += new EventHandler(EmergencyLinkView_Prepare);
        }

        private void EmergencyLinkView_Prepare(object sender, EventArgs e)
        {
            Call_View CallView = (Call_View)Current_View;

            SetView(typeof(Emergency_Link_View), new List<object> { CallView.GetEmergency_Call() });

            Current_View.SetPrevious_View(CallView);
        }

        private void EmergencyManagementView_Prepare(object sender, EventArgs e)
        {
            SetTypicalView(typeof(Emergency_Management_View));
        }

        private void ResponseTeamInfoView_Prepare(object sender, EventArgs e)
        {
            SetTypicalView(typeof(Response_Team_Information_View));
        }

        private void EMTLoginShiftView_Prepare(object sender, EventArgs e)
        {
            SetTypicalView(typeof(EMT_Login_Shift_View));
        }

        private void BaseStationRecordsView_Prepare(object sender, EventArgs e)
        {
            SetTypicalView(typeof(Base_Station_Records_View));
        }

        private void DispatchRelatedTimesView_Prepare(object sender, EventArgs e)
        {
            SetTypicalView(typeof(Dispatch_Related_Times_View));
        }

        private void DispatchReportView_Prepare(object sender, EventArgs e)
        {
            SetTypicalView(typeof(Dispatch_Report_View));
        }

        private void LinkPatientView_Prepare(object sender, EventArgs e)
        {
            SetTypicalView(typeof(Link_Patient_View));
        }

        private void PatientInformationView_Prepare(object sender, EventArgs e)
        {
            SetTypicalView(typeof(Patient_Information_View));
        }

        private void InvoiceView_Prepare(object sender, EventArgs e)
        {
            SetTypicalView(typeof(Invoice_View));
        }
        //


        // FUNCTIONAL METHODS
        public void SetSideBar(ref Shared_Data xSD, List<Type> xAcccessibleViews, Person xUserType)
        {
            sideBar.PopulateSideBar(ref SD, xAcccessibleViews, xUserType);
        }

        private Special_View SetView(Type xSpecialView, List<object> xAdditionalParam = null)
        {
            List<object> Parameters = new List<object>();
            Parameters.Add(SD);

            // Use this if a view's constructor has more additional parameters (besides Shared_Data).
            if (xAdditionalParam != null)
            {
                Parameters.AddRange(xAdditionalParam);
            }

            Current_View = (Special_View)(Activator.CreateInstance(xSpecialView, Parameters.ToArray()));

            if (Current_View.GetMiddleAligned() == false)
            {
                this.SizeChanged -= new EventHandler(Main_Form_SizeChanged);
                Current_View.Dock = DockStyle.Fill;
            }
            else
            {
                this.SizeChanged += new EventHandler(Main_Form_SizeChanged);

                Main_Form_SizeChanged(null, null);
            }

            Current_View.Show();

            this.Controls.Add(Current_View);
            Current_View.BringToFront();

            return Current_View;
        }

        private Special_View SetTypicalView(Type xTypicalView)
        {
            Special_View SVtemp = Current_View;

            //Current_View.Dispose();

            SetView(xTypicalView);

            Current_View.SetPrevious_View(SVtemp);

            return Current_View;
        }

        public void UpdateSD(Shared_Data xSD)
        {
            SD = xSD;
        }
        //


        private void Main_Form_SizeChanged(object sender, EventArgs e)
        {
            Current_View.Location = new System.Drawing.Point((int)(this.Width / 2.0) - (int)(Current_View.Width / 2.0), 15);
        }

        private void Main_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            for(int i = 0; i < SD.MainForms.Count; i++)
            {
                
                if (SD.MainForms[i] == this)
                {
                    SD.MainForms.RemoveAt(i);

                    if (SD.MainForms.Count == 0)
                    {
                        Application.Exit();
                        return;
                    }
                    else
                    {
                        e.Cancel = true;
                        this.Dispose(false);

                       // SD.MainForms[0].Focus();
                        return;
                    }
                }
            }
        }
    }
}