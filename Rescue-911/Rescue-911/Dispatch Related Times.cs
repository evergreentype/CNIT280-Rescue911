﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rescue_911
{
    public partial class Dispatch_Related_Times : Form
    {
        private Shared_Data SD;

        public Dispatch_Related_Times(ref Shared_Data xSD)
        {
            SD = xSD;
            InitializeComponent();
        }
        public Dispatch_Related_Times()
        {
            Name = "Dispatch and arrival times";
        }

        private void Dispatch_Related_Times_Load(object sender, EventArgs e)
        {
            lstEmergenciesFetch("Accepted", SD.Emergencies);
        }

        private void btnDispatchTime_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstEmergencies.SelectedItems[0] == null)
                    return;

                lstEmergencies.SelectedItems[0].SubItems.Add(dtPicker.Value.ToString("h:mm:ss MM/dd/yyyy "));
            }
            catch
            {

            }
        }

        private void btnRecordArrival_Click(object sender, EventArgs e)
        {

            try
            {
                if (lstEmergencies.SelectedItems[0] == null)
                    return;

                lstEmergencies.SelectedItems[0].SubItems.Add(dtPicker.Value.ToString("h:mm:ss MM/dd/yyyy "));
            }
            catch
            {

            }
        }

        private void lstEmergenciesFetch(string state, List<Emergency> ExistingEmergencies)
        {
            lstEmergencies.Items.Clear();

            foreach (Emergency iEmergency in ExistingEmergencies)
            {
                int j = 0;

                if (iEmergency.GetLinkedCalls()[0].GetState() != state)
                    continue;

                foreach (Emergency_Call EC in iEmergency.GetLinkedCalls())
                {
                    if (EC == null)
                        break;

                    if (j == 0)
                    {
                        ListViewItem lstItem = new ListViewItem(iEmergency.GetEmergency_ID().ToString());

                        lstItem.SubItems.Add(EC.GetAddress());

                        lstEmergencies.Items.AddRange(new ListViewItem[1] { lstItem });
                    }
                    else
                    {
                        ListViewItem lstItem = new ListViewItem();

                        lstItem.SubItems.Add(EC.GetAddress());

                        lstEmergencies.Items.AddRange(new ListViewItem[1] { lstItem });
                    }
                    j++;
                }
            }
        }

        private void lstEmergencies_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
