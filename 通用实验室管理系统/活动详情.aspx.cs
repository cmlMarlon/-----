﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mysqlHelper;
using System.IO;
using System.Data;

public partial class 活动详情 : System.Web.UI.Page
{
    private string taId_find;
    private static string image_yuan;
    protected void Page_Load(object sender, EventArgs e)
    {
        //获得活动数据库长度
        string sqlcount = "select count(*) from general_TeamActive";
        DataTable dt_count = MysqlHelper.ExecuteDataTable(sqlcount);
        int llength = Convert.ToInt32(dt_count.Rows[0][0]);

        string[] taId_table = new string[llength];           //编号
        string[] taDate_table = new string[llength];         //时间
        string[] taTitle_table = new string[llength];        //标题
        string[] taText_table = new string[llength];         //内容
        string[] taImage_table = new string[llength];        //图片

        string sql = "select * from general_TeamJournalism";
        DataTable dt = MysqlHelper.ExecuteDataTable(sql);
        int i;
        for (i = 0; i < taId_table.Length; i++)
        {
            taId_table[i] = dt.Rows[i][0].ToString().Trim();
            taDate_table[i] = dt.Rows[i][1].ToString().Trim();
            taTitle_table[i] = dt.Rows[i][2].ToString().Trim();
            taText_table[i] = dt.Rows[i][3].ToString().Trim();
            taImage_table[i] = dt.Rows[i][4].ToString().Trim();
        }

        int j = 0;
        if (!IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                taId_find = Request.QueryString["id"];
                for (j = 0; j < taId_table.Length; j++)
                {
                    if (taId_table[j].ToString().Trim() == taId_find)
                    {

                        taDate.Text = taDate_table[j];
                        taTitle.Text = taTitle_table[j];
                        taText.Text = taText_table[j];
                        taImage.ImageUrl = taImage_table[j];

                        //------------------------textbox--------------------------//
                        tadateBox.Text = taDate_table[j];
                        tatitleBox.Text = taTitle_table[j];
                        tatextBox.Text = taText_table[j];
                        image_yuan = taImage_table[j];
                        break;
                    }
                }
            }
        }
    }
    protected void 提交修改_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] != null)
        {
            taId_find = Request.QueryString["id"];
        }
        //Response.Write("<script>alert('"+ tjId_find + "')</script>");
        string taImage_change;
        string pa = FileUpload1.PostedFile.FileName;
        string path = Path.GetFileName(FileUpload1.PostedFile.FileName);

        if (pa.Trim().Length != 0)
        {
            System.IO.File.Copy(pa, Server.MapPath("./Images/" + path), true);

            taImage_change = "./Images/" + path;
        }
        else
        {
            taImage_change = image_yuan;
        }
        string my_sql = "update general_TeamACtive set taDate='" + tadateBox.Text.Trim() + "',taTitle='" + tatitleBox.Text.Trim() + "', taText='"+tatextBox.Text.Trim()+"',taImage='" + taImage_change + "' where taId=" + taId_find;
        MysqlHelper.ExecuteNonQuery(my_sql);

        Response.Redirect("./新闻详情.aspx?id=" + taId_find);
    }
}