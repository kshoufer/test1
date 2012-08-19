//Ken Shoufer
//ASP.NET
//This program selects photos based on country



using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;


public partial class MainPage : System.Web.UI.Page
{
    #region Module Level Variables

    //ADO variables		
    private static System.Data.OleDb.OleDbConnection oleDbConnection1;
    private static System.Data.OleDb.OleDbDataReader oleDbDataReader1;
    private static System.Data.OleDb.OleDbCommand oleDbCommandGetPhotoInfoByLocation;
    private static System.Data.OleDb.OleDbCommand oleDbCommandGetCountOfPhotosByLocation;
    //Array to hold the info retrieved from the DB
    private string[,] m_strTravelInfo;

    //Other module level variables
    private string m_strFilePath;
    private int m_intPhotoCount;
    private int m_intCurrentIndex;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        //Get Images Path
        var strFilePath = Server.MapPath("MainPage.aspx");
        int indexOfLastCharacter = strFilePath.IndexOf("MainPage.aspx");
        int indexOfLastSlash = strFilePath.LastIndexOf("\\");
        var strFilePathShortened = strFilePath.Substring(0, indexOfLastSlash);
        m_strFilePath = strFilePathShortened + "\\images\\";


        lblCaption.Text = "Traveling the World.";
        lblTravelDate.Text = "April, 2012";
        imgMain.ImageUrl = "~/images/big-sur1(4-4-12).jpg";


        var dbPath = Server.MapPath("~/App_Data/TravelPhotos.mdb");

        oleDbConnection1 = new OleDbConnection();
        oleDbConnection1.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + dbPath;

        //Set the path to our image files into a variable for future use (performs this concatenation only once)
        string MapPath1 = Server.MapPath("~/App_Data/TravelPhoto.mdb");

    }// end Page_Load


    #region Database Access


    private void GetPhotoData(string strLocation)
    {
        try
        {

            //Open our connection, provided that it isn't already open
            if (oleDbConnection1.State != ConnectionState.Open)
            {
                oleDbConnection1.Open();
            }
            //Find out how many photos in our DB correspond to the selected location. 
            m_intPhotoCount = GetPhotoCount(strLocation);
            //store m_intPhotoCount in a Session variable
            Session["s_intPhotoCount"] = m_intPhotoCount;
            //Use that value to size our TravelInfo array. 
            if (m_intPhotoCount == 0)
            {	//Exit this function if the PhotoCount query returned 0 results
                Console.WriteLine("PhotoCount returned 0 results.  Please investigate.");
                return;
            }
            m_strTravelInfo = new string[m_intPhotoCount, 3];
            //Initialize the location parameter to our GetPhotoInfoByLocation ADO Command Object
            oleDbCommandGetPhotoInfoByLocation = new OleDbCommand("GetPhotoInfoByLocation_sp", oleDbConnection1);
            oleDbCommandGetPhotoInfoByLocation.CommandType = CommandType.StoredProcedure;
            oleDbCommandGetPhotoInfoByLocation.Parameters.Clear();
            oleDbCommandGetPhotoInfoByLocation.Parameters.Add("@Location", OleDbType.VarChar, 25);
            oleDbCommandGetPhotoInfoByLocation.Parameters["@Location"].Value = strLocation;
            //Use a DataReader to get the info from our database
            oleDbDataReader1 = oleDbCommandGetPhotoInfoByLocation.ExecuteReader(CommandBehavior.CloseConnection);
            int intLoopCounter = 0;
            //Loop through this data to populate our TravelInfo array.
            while (oleDbDataReader1.Read() == true)
            {
                m_strTravelInfo[intLoopCounter, 0] = oleDbDataReader1.GetString(0);	//Filename
                m_strTravelInfo[intLoopCounter, 1] = oleDbDataReader1.GetString(1);	//Caption
                m_strTravelInfo[intLoopCounter, 2] = oleDbDataReader1.GetString(2);	//TravelDate
                intLoopCounter++;
            }

            oleDbDataReader1.Close();
            //Display the first photo from our collection matching this location. 
            m_intCurrentIndex = 0;
            //Store m_intCurrentIndex in a Session variable
            Session["s_intCurrentIndex"] = m_intCurrentIndex;
            //store the array in a Session object
            Session["s_strTravelInfo"] = m_strTravelInfo;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {	
            oleDbConnection1.Close();
        }

    }// end GetPhotoData

    private int GetPhotoCount(string strLocation)
    {
        try
        {//Never try to debug a database related function without satisfactory error info. 
            //Set the parameter to allow us to find out how many photos match our selected location. 
            oleDbCommandGetCountOfPhotosByLocation = new OleDbCommand("GetCountOfPhotosByLocation_sp", oleDbConnection1);
            oleDbCommandGetCountOfPhotosByLocation.CommandType = CommandType.StoredProcedure;
            oleDbCommandGetCountOfPhotosByLocation.Parameters.Clear();
            oleDbCommandGetCountOfPhotosByLocation.Parameters.Add("@Location", OleDbType.VarChar, 25);
            oleDbCommandGetCountOfPhotosByLocation.Parameters["@Location"].Value = strLocation;
            //Execute our stored procedure to retrieve the number that we need
            int intNoOfPhotos = Convert.ToInt32(oleDbCommandGetCountOfPhotosByLocation.ExecuteScalar());
            //Send that value back to the calling function
            return intNoOfPhotos;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 0;
        }

    }// end GetPhotoCount


    #endregion

    #region Select Radio Button


    protected void rbBigSur_CheckedChanged(object sender, EventArgs e)
    {
        //load array
        //diplay first image, caption and date
        GetPhotoData("BigSur");
        //retreive m_intPhotoCount from Session variable
        m_strTravelInfo = (string[,])Session["s_strTravelInfo"];

        string strFileName = m_strTravelInfo[0, 0];
        lblCaption.Text = m_strTravelInfo[0, 1];
        lblTravelDate.Text = m_strTravelInfo[0, 2];
        imgMain.ImageUrl = "~/images/" + strFileName;
    }// end rbBigSur_CheckedChanged

    protected void rbJoshuaTree_CheckedChanged(object sender, EventArgs e)
    {
        //load array
        //diplay first image, caption and date
        GetPhotoData("JoshuaTree");
        //retreive m_intPhotoCount from Session variable
        m_strTravelInfo = (string[,])Session["s_strTravelInfo"];

        string strFileName = m_strTravelInfo[0, 0];
        lblCaption.Text = m_strTravelInfo[0, 1];
        lblTravelDate.Text = m_strTravelInfo[0, 2];
        imgMain.ImageUrl = "~/images/" + strFileName;

    }// end rbJoshuaTree_CheckedChanged


    protected void rbZion_CheckedChanged(object sender, EventArgs e)
    {
        //load array
        //diplay first image, caption and date
        GetPhotoData("Zion");
        //retreive m_intPhotoCount from Session variable
        m_strTravelInfo = (string[,])Session["s_strTravelInfo"];

        string strFileName = m_strTravelInfo[0, 0];
        lblCaption.Text = m_strTravelInfo[0, 1];
        lblTravelDate.Text = m_strTravelInfo[0, 2];
        imgMain.ImageUrl = "~/images/" + strFileName;

    }// end rbZion_CheckedChanged

    #endregion

    private void DisplayInfo(int intCounter)
    {
        //retreive m_intPhotoCount from Session variable
        m_strTravelInfo = (string[,])Session["s_strTravelInfo"];

        string strFileName = m_strTravelInfo[intCounter, 0];
        lblCaption.Text = m_strTravelInfo[intCounter, 1];
        lblTravelDate.Text = m_strTravelInfo[intCounter, 2];

        if (File.Exists(m_strFilePath + strFileName))
        {
            imgMain.ImageUrl = "~/images/" + strFileName;
        }

    }// end DisplayInfo

    #region Previous and Next Button Event Handlers

    protected void btnPrevious_Click(object sender, System.EventArgs e)
    {
        try
        {
            if (Session["s_intPhotoCount"] != null)
                m_intPhotoCount = (int)Session["s_intPhotoCount"];

            if (m_intPhotoCount == 0)
            {	//Exit this function if the PhotoCount query returned 0 results
                lblError.Text = "Please Select an Option Before Clicking on Next";
                lblError.Visible = true;
                return;
            }

            lblError.Visible = false;
            //retreive m_intPhotoCount from Session variable
            m_intPhotoCount = (int)Session["s_intPhotoCount"];
            //retreive m_intCurrentIndex from Session variable
            m_intCurrentIndex = (int)Session["s_intCurrentIndex"];    
            
            if (m_intPhotoCount == 0)
            {	//Exit this function if the PhotoCount query returned 0 results
                //Also protects from an exception if the user clicks next before
                //choosing any location.
                lblError.Text = "Please Select an Option Before Clicking on Next";
                lblError.Visible = true;
                return;
            }

            if (m_intCurrentIndex > 0)
            {
                m_intCurrentIndex -= 1;
                //Store m_intCurrentIndex in a Session variable
                Session["s_intCurrentIndex"] = m_intCurrentIndex;
                DisplayInfo(m_intCurrentIndex);
            }
            else
            {	//Loop around to the other end of our array
                m_intCurrentIndex = m_intPhotoCount - 1;
                //Store m_intCurrentIndex in a Session variable
                Session["s_intCurrentIndex"] = m_intCurrentIndex;
                DisplayInfo(m_intCurrentIndex);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }// end btnPrevious_Click

    protected void btnNext_Click(object sender, System.EventArgs e)
    {
        try
        {
            if (Session["s_intPhotoCount"] != null)
                m_intPhotoCount = (int)Session["s_intPhotoCount"];

            if (m_intPhotoCount == 0)
                {	//Exit this function if the PhotoCount query returned 0 results
                    lblError.Text = "Please Select an Option Before Clicking on Next";
                    lblError.Visible = true;
                    return;
                }

            lblError.Visible = false;
            //retreive m_intPhotoCount from Session variable
            m_intPhotoCount = (int)Session["s_intPhotoCount"];
            //retreive m_intCurrentIndex from Session variable
            m_intCurrentIndex = (int)Session["s_intCurrentIndex"];    
            
            if (m_intCurrentIndex < m_intPhotoCount - 1)
            {
                m_intCurrentIndex += 1;
                //Store m_intCurrentIndex in a Session variable
                Session["s_intCurrentIndex"] = m_intCurrentIndex;
                DisplayInfo(m_intCurrentIndex);
            }
            else
            {	//Loop around to the other end of our array
                m_intCurrentIndex = 0;
                //Store m_intCurrentIndex in a Session variable
                Session["s_intCurrentIndex"] = m_intCurrentIndex;
                DisplayInfo(m_intCurrentIndex);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }// btnNext_Click

    #endregion

}// end partial class MainPage













