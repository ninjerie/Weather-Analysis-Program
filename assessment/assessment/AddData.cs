﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace assessment
{
    public partial class AddData : Form
    {
        private int curLocation;
        private int curYear;

        public AddData()
        {
            InitializeComponent();

            SetCmbxSelectToAddLocation();

            // Hide unnecessary objects
            gpBxLocation.Visible = false;
            gpBxYear.Visible = false;
            gpBxMonth.Visible = false;
            cmbxSelectToAddLocation.Visible = false;
            cmbxSelectToAddYear.Visible = false;
            lbLocationAdd.Visible = false;
            lbYearAdd.Visible = false;

        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            OptionsForm f = new OptionsForm();
            f.Show();
            this.Close();
        }

        // Add data from a file
        private void btnAddFile_Click(object sender, EventArgs e)
        {
            // Creates a file dialog
            OpenFileDialog fileDialog = new OpenFileDialog();
            // Gives the file dialog a filter so only text files show
            fileDialog.Filter = "Text Files|*.txt";
            // Shows the dialog
            fileDialog.ShowDialog();
            string file = fileDialog.FileName;

            // Create stream reader using the file selected
            StreamReader sr = new StreamReader(file);

            // Find how many locations there are
            int numOfLocationsToAdd = Convert.ToInt32(sr.ReadLine());
            int currentTotalLocations = Form1.frm1Ref.numOfLocations;

            for (int i = 0; i < numOfLocationsToAdd; i++)
            {
                // Resize the array
                Array.Resize(ref Form1.frm1Ref.locations, (currentTotalLocations + i + 1));
                // Create the location
                Form1.frm1Ref.locations[currentTotalLocations + i] = new Location(
                    sr.ReadLine(),
                    sr.ReadLine(),
                    sr.ReadLine(),
                    sr.ReadLine(),
                    sr.ReadLine(),
                    sr.ReadLine(),
                    Convert.ToInt32(sr.ReadLine())
                    );

                // For each year in location
                for (int y = 0; y < Form1.frm1Ref.locations[currentTotalLocations + i].GetNumOfYears(); y++)
                {
                    // Create a year
                    Year thisYear = new Year(
                        sr.ReadLine(),
                        Convert.ToInt32(sr.ReadLine())
                        );

                    // Add year to location
                    Form1.frm1Ref.locations[currentTotalLocations + i].SetYears(thisYear, y);

                    // For each month in year
                    for (int z = 0; z < 12; z++)
                    {
                        // Reads the repeated year id
                        if (z != 0)
                        {
                            sr.ReadLine();
                        }

                        // Create a month
                        MonthlyObservation thisMonth = new MonthlyObservation(

                            Convert.ToInt32(sr.ReadLine()),
                            Convert.ToDouble(sr.ReadLine()),
                            Convert.ToDouble(sr.ReadLine()),
                            Convert.ToInt32(sr.ReadLine()),
                            Convert.ToDouble(sr.ReadLine()),
                            Convert.ToDouble(sr.ReadLine())
                            );

                        // Add month to year
                        Form1.frm1Ref.locations[currentTotalLocations + i].SetMonths(y, thisMonth, z);

                    }
                }

            }

            MessageBox.Show("Everything has been read in.");
            Form1.frm1Ref.numOfLocations += numOfLocationsToAdd;
            OptionsForm f = new OptionsForm();
            f.Show();
            this.Close();
        }

        private void cmbxSelectToEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Hides all groups
            gpBxLocation.Visible = false;
            gpBxYear.Visible = false;
            gpBxMonth.Visible = false;

            // If adding a location...
            if (cmbxSelectToEdit.Text == "Location")
            {
                // ...show the location group
                gpBxLocation.Visible = true;

                // ...hide the location selection and label
                cmbxSelectToAddLocation.Visible = false;
                lbLocationAdd.Visible = false;

                // ...hide the year selection and label
                cmbxSelectToAddYear.Visible = false;
                lbYearAdd.Visible = false;

            }
            // If adding a year...
            if (cmbxSelectToEdit.Text == "Year")
            {
                // ...show the year group
                gpBxYear.Visible = true;

                // ...show the location selection and label
                cmbxSelectToAddLocation.Visible = true;
                lbLocationAdd.Visible = true;

                // ...hide the year selection and label
                cmbxSelectToAddYear.Visible = false;
                lbYearAdd.Visible = false;


            }
            if (cmbxSelectToEdit.Text == "Month")
            {
                // ...show the month group
                gpBxMonth.Visible = true;

                // ...show the location selection and label
                cmbxSelectToAddLocation.Visible = true;
                lbLocationAdd.Visible = true;

                // ...show the year selection and label
                cmbxSelectToAddYear.Visible = true;
                lbYearAdd.Visible = true;


            }

        }

        private void cmboBxSelectToAddLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            curLocation = cmbxSelectToAddLocation.SelectedIndex;
            SetcmbxSelectToAddYear();
        }

        // Set combobox items with locations
        private void SetCmbxSelectToAddLocation()
        {
            cmbxSelectToAddLocation.Items.Clear();

            for (int i = 0; i < Form1.frm1Ref.numOfLocations; i++)
            {
                if (Form1.frm1Ref.locations[i] != null)
                {
                    cmbxSelectToAddLocation.Items.Add(Form1.frm1Ref.locations[i].GetName());
                }

            }
        }

        // Set combobox items with years
        private void SetcmbxSelectToAddYear()
        {
            cmbxSelectToAddYear.Items.Clear();

            if (cmbxSelectToAddLocation.SelectedIndex != -1)
            {
                foreach (var years in Form1.frm1Ref.locations[curLocation].GetYears())
                {
                    cmbxSelectToAddYear.Items.Add(years.GetYearID());
                }
            }
        }





        // empty
        private void cmboBxSelectToAddYear_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
