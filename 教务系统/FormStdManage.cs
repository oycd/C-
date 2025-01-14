﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 教务系统
{
    public partial class FormStdManage : Form
    {
        bool add = false;//表示是否点击“新增”按钮
        int idx;//记录当前修改的操作员在DataGridVIew的位置
        DbDataReader reader;
        string sql;
        string code;
        DBHelper helper;
        public FormStdManage()
        {
            InitializeComponent();
        }

        private void FormStdManage_Load(object sender, EventArgs e)
        {
            if (textdelive7.Text.Equals("学生"))
            {
                btnAdd.Enabled = false;
                btnDel.Enabled = false;
                sql = "select  code,name,dpm,major,class from admin where code = '"+textdelivecode.Text+"'";
               
                helper = new DBHelper("mysql");
                if (reader != null && reader.Read())
                {
                    code = reader.GetString(0);
                }
                
                DataTable table = helper.FillTable(sql);
                grdData.DataSource = table;
            }
            else
            {
                btnAdd.Enabled = true;
                btnDel.Enabled = true;
                sql = "select  code,name,dpm,major,class from admin where role = '学生'";
                helper = new DBHelper("mysql");
                DataTable table = helper.FillTable(sql);
                grdData.DataSource = table;
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
           this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            add = true;
            grpData.Visible = false;
            grpEditor.Visible = true;
            textCode.Text = "";
            textName.Text = "";
            textDpm.Text = "";
            textMajor.Text = "";
            textClass.Text = "";
            textPwd.Text = "";
            textName.Enabled = true;
            textCode.Focus();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            grpEditor.Visible = false;
            grpData.Visible = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            
            
                if (textdelive7.Text.Equals("学生"))
                {
                    bool success = false;
                    sql = "update admin set pwd='" + textPwd.Text + "' where code='" + grdData.Rows[idx].Cells[0].Value.ToString() + "'";
                    if (helper.Update(sql) > 0)
                    {
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                    if (success)
                    {

                        /* grdData.Rows[idx].Cells[0].Value = textCode.Text.Trim();
                         grdData.Rows[idx].Cells[2].Value = textDpm.Text.Trim();
                         grdData.Rows[idx].Cells[3].Value = textMajor.Text.Trim();
                         grdData.Rows[idx].Cells[4].Value = textClass.Text.Trim();*/


                        grpEditor.Visible = false;
                        grpData.Visible = true;
                        MessageBox.Show("密码修改成功，请重启程序登录！");
                        Application.Exit();
                    }
                    else
                    {
                        MessageBox.Show("密码修改失败");
                    }
                }
                else
                {

                    bool success = false;
                    long classcode = long.Parse(textCode.Text);
                    classcode = classcode / 100;

                    if (add)
                    {
                        sql = "insert into admin values ('" + textCode.Text.Trim() + "','" + textName.Text.Trim() + "','" + textPwd.Text.Trim() + "','学生','pic1.png','" + textDpm.Text.Trim() + "','" + textMajor.Text.Trim() + "','" + textClass.Text.Trim() + "','" + classcode + "')";
                    }
                    else
                    {
                        sql = "update admin set code='" + textCode.Text + "',name='" + textName.Text + "',pwd='" + textPwd.Text + "',dpm='" + textDpm.Text + "',major='" + textMajor.Text + "',class='" + textClass.Text + "' where code='" + grdData.Rows[idx].Cells[0].Value.ToString() + "'";
                    }
                    if (helper.Update(sql) > 0)
                    {
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                    if (success)
                    {
                        if (add)
                        {
                            //grdData.Rows.Add(txtCode.Text.Trim(), txtName.Text.Trim(), role);
                            DataTable dt = (DataTable)grdData.DataSource;
                            dt.Rows.Add(textCode.Text.Trim(), textName.Text.Trim(), textDpm.Text.Trim(), textMajor.Text.Trim(), textClass.Text.Trim());
                        }
                        else
                        {
                            grdData.Rows[idx].Cells[0].Value = textCode.Text.Trim();
                            grdData.Rows[idx].Cells[2].Value = textDpm.Text.Trim();
                            grdData.Rows[idx].Cells[3].Value = textMajor.Text.Trim();
                            grdData.Rows[idx].Cells[4].Value = textClass.Text.Trim();

                        }
                        grpEditor.Visible = false;
                        grpData.Visible = true;
                        MessageBox.Show("数据保存成功！");
                    }
                    else
                    {
                        MessageBox.Show("数据保存失败，可能编号重复！");
                    }
                }
            
        }

        private void grdData_SelectionChanged(object sender, EventArgs e)
        {
            if (textdelive7.Text.Equals("学生"))
            {
                btnEdit.Enabled = grdData.SelectedRows.Count == 1;
                //btnDel.Enabled = grdData.SelectedRows.Count == 1;
                idx = grdData.CurrentRow.Index;

            }
            else 
            { 
            btnEdit.Enabled = grdData.SelectedRows.Count == 1;
            btnDel.Enabled = grdData.SelectedRows.Count == 1;
            idx = grdData.CurrentRow.Index;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (textdelive7.Text.Equals("学生"))
            {
                add = false;
                grpData.Visible = false;
                grpEditor.Visible = true;
                textCode.Focus();
                textName.Enabled = false;
                textClass.Enabled = false;
                textCode.Enabled = false;
                textDpm.Enabled = false;
                textMajor.Enabled = false;

                textName.Text = grdData.Rows[idx].Cells[1].Value.ToString();
                
                textCode.Text = grdData.Rows[idx].Cells[0].Value.ToString();
                textDpm.Text = grdData.Rows[idx].Cells[2].Value.ToString();
                textMajor.Text = grdData.Rows[idx].Cells[3].Value.ToString();
                textClass.Text = grdData.Rows[idx].Cells[4].Value.ToString();


            }
            else
            {
                add = false;
                grpData.Visible = false;
                grpEditor.Visible = true;
                textCode.Focus();
                textName.Enabled = false;
                textClass.Enabled = true;
                textCode.Enabled = true;
                textDpm.Enabled = true;
                textMajor.Enabled = true;
                textName.Text = grdData.Rows[idx].Cells[1].Value.ToString();
                textCode.Text=grdData.Rows[idx].Cells[0].Value.ToString() ;
                textDpm.Text=grdData.Rows[idx].Cells[2].Value.ToString() ;
                textMajor.Text=grdData.Rows[idx].Cells[3].Value.ToString() ;
                textClass.Text=grdData.Rows[idx].Cells[4].Value.ToString();
            }


        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            bool success = false;
            if (MessageBox.Show("是否确认删除选中记录？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                //1、准备sql语句

                sql = "delete from admin where code = '" + grdData.Rows[idx].Cells[0].Value + "'";
                //2、执行sql语句
                if (helper.Update(sql) > 0)
                {
                    success = true;
               


                }
                else
                {
                    success = false;
                    MessageBox.Show("删除失败");
                }

            }
            //3、如果成功在数据库中删除记录则在界面中删除1行记录
            if (success)
            {
                grdData.Rows.RemoveAt(idx);
                
            }
        }
    }
}
