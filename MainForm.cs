/*
 * Created by SharpDevelop.
 * User: Mike
 * Date: 2017-10-12
 * Time: 9:00 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Linq.Expressions;
using System.Xml;
using System.Xml.Linq;

namespace TaskManager
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		Sort[] sort;
			
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			
			// loading Tasks
			List <Task> tasks = Task.GetTasks();
			Console.WriteLine("Total of {0} tasks", tasks.Count);
	        
			// load last saved Sorts
			sort = Sort.LoadSorts();
			Console.WriteLine("Sorting loaded: " + sort);
			
			SortAndShow();
		}
		
		private void SortAndShow(){
			List <Task> tasks = Task.GetTasks();
			List<Task> sortedTasks = Sort.DoSort<Task>(tasks, sort).ToList();
			UpdateList(sortedTasks);
		}
		
		public void UpdateList(List<Task> data){
			listView1.Items.Clear();
			foreach (Task t in data) {
				ListViewItem listitem = new ListViewItem(t.id.ToString());
				listitem.SubItems.Add(t.title);
				listitem.SubItems.Add(t.weight.ToString());
				listitem.SubItems.Add(t.parent.ToString());
				listView1.Items.Add(listitem);
			}
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			Console.WriteLine("-- Adding Task");
			Task.AddTask(6, "Have fun", 10, -1);
			//Task.WriteTasks("Tasks-adj.xml");		//FIXME:  not saving for now
			SortAndShow();
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			Console.WriteLine("-- Changing Task");
			Task.ChangeTask(1, "title", "Alarm Clock");
			//Task.WriteTasks("Tasks-add.xml");		//FIXME:  not saving for now
			SortAndShow();
		}
	}
}
