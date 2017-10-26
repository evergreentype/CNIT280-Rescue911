﻿using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Rescue_911
{
    public partial class CallForm : Form
    {
        Shared_Data SD;
        Emergency_Call Current_Call;
        private int Current_Call_Index;

        public CallForm(ref Shared_Data xSD)
        {
            SD = xSD;

            Current_Call = new Emergency_Call();
            Current_Call.SetDateTime(DateTime.Now);

            int maxCaller_ID = 0;
            for (int i = 0; i < (SD.Calls.Length - 1); i++)
            {
                if (SD.Calls[i] == null)
                {
                    Current_Call_Index = i;

                    Current_Call.GetEmergency_Caller().SetCaller_ID(SD.Calls[i - 1].GetEmergency_Caller().GetCaller_ID() + 1);
                    Current_Call.SetState("not logged");

                    // Update the Shared Data values regarding the Calls.
                    ((Form1)SD.OpenForms[2, 0]).UpdateSD(SD);

                    break;
                }
                else
                {
                    if (maxCaller_ID > SD.Calls[i].GetEmergency_Caller().GetCaller_ID())
                        maxCaller_ID = SD.Calls[i].GetEmergency_Caller().GetCaller_ID();
                }
            }
            InitializeComponent();
        }

        private void CallForm_Load(object sender, EventArgs e)
        {
            lbCallID.Text = Current_Call.GetEmergency_Caller().GetCaller_ID().ToString();

            lbCallState.Text = Current_Call.GetState();

            txtCallDateTime.Text = Current_Call.GetDateTime().ToString("h:mm:ss MM/dd/yyyy ");
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            int teams;

            // Existence checks
            if (Current_Call.GetPriority() == null)
            {
                cboCallPriority.Focus();
            }
            else if (Current_Call.GetEmergency_Caller().GetPhone_Number() == null)
            {
                txtPhoneNumber.Focus();
            }
            else if (Current_Call.GetEmergency_Caller().GetName() == null)
            {
                txtCallerName.Focus();
            }
            else if (Current_Call.GetAddress() == null)
            {
                txtAddress.Focus();
            }
            else if (Current_Call.GetDescription() == null)
            {
                txtDescription.Focus();
            }
            else if (int.TryParse(txtTeamsReq.Text, out teams) == false)
            {
                txtTeamsReq.Focus();
            }
            else // All checks are satisfied
            {
                Current_Call.SetTeams_Required(teams);
                Current_Call.SetState("Logged");

                Current_Call.GetEmergency_Caller().SetLast_Name(txtCallerLastName.Text);
                Current_Call.SetLandmark(txtLandmark.Text);

                // Open the Emergency Form
                Emergency_Form EmergencyForm = new Emergency_Form();
                EmergencyForm.SetEmergency_Call(Current_Call);

                // Update the Shared Data values regarding the Calls.
                SD.Calls[Current_Call_Index] = Current_Call;
                ((Form1)SD.OpenForms[2, 0]).UpdateSD(SD);

                EmergencyForm.Show();
                this.Close();
            }
        }

        private void cboCallPriority_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Sets an actual index.
            Current_Call.SetPriority(cboCallPriority.SelectedIndex + 1);
        }

        private void txtPhoneNumber_Leave(object sender, EventArgs e)
        {
            int number;
            if (int.TryParse(txtPhoneNumber.Text, out number) && txtPhoneNumber.Text.Length == 10)
            {
                Current_Call.GetEmergency_Caller().SetPhone_Number(txtPhoneNumber.Text);


                // Suggeted names from a database
                //lstCallerNames.Items.Clear();
                //int j = 0;
                //for (int i = 0; i < CallerNamesTest.Count(); i++)
                //{
                //    if (CallerPNTest[i] == txtPhoneNumber.Text)
                //    {
                //        SuggestedCallerIDs[j] = i;
                //        lstCallerNames.Items.Add(CallerNamesTest[i] + " " + CallerLNamesTest[i]);

                //        j++;
                //    }
                //}
                //
            }
        }

        private void txtCallerName_Leave(object sender, EventArgs e)
        {
            if (txtCallerName.Text.Trim() != string.Empty)
                Current_Call.GetEmergency_Caller().SetName(txtCallerName.Text);
        }

        private void txtAddress_Leave(object sender, EventArgs e)
        {
            if (txtAddress.Text.Trim() != string.Empty)
                Current_Call.SetAddress(txtAddress.Text);
        }

        private void txtDescription_Leave(object sender, EventArgs e)
        {
            if (txtDescription.Text.Trim() != string.Empty)
                Current_Call.SetDescription(txtDescription.Text);
        }

        private void lstCallerNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (lstCallerNames.SelectedIndex != -1)
            //{
            //    foreach (int index in SuggestedCallerIDs)
            //    {
            //        if (index == lstCallerNames.SelectedIndex)
            //        {
            //            Current_Call.GetEmergency_Caller().SetCaller_ID(SuggestedCallerIDs[index]);
            //            Current_Call.GetEmergency_Caller().SetName(CallerNamesTest[SuggestedCallerIDs[index]]);
            //            Current_Call.GetEmergency_Caller().SetLast_Name(CallerLNamesTest[SuggestedCallerIDs[index]]);
            //        }
            //    }

            //    lbCallID.Text = Current_Call.GetEmergency_Caller().GetCaller_ID().ToString();
            //    txtCallerName.Text = Current_Call.GetEmergency_Caller().GetName();
            //    txtCallerLastName.Text = Current_Call.GetEmergency_Caller().GetLast_Name();
            //}
        }
    }
}