/*
 * Created by SharpDevelop.
 * User: User
 * Date: 03.05.2017
 * Time: 17:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
// Manual at https://metanit.com/sharp/tutorial/16.4.php

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml;
using System.Xml.Linq;

namespace TaskManager
{
	class Program
	{
		public static string DataDir = "../../data/";
		// for running from IDE only. Replace with "." for release!
								
		public static void Main(string[] args)
		{
			// loading tasks
			List <Task> tasks = Task.GetTasks();
			Console.WriteLine("Total of {0} tasks", tasks.Count);
			foreach (Task a in tasks)
				Console.WriteLine("ID: {0}, Title: {1}, Weight: {2}, Parent: {3}", a.id, a.title, a.weight, a.parent);

			Console.WriteLine("-- Changing Task");
			Task.ChangeTask(1, "title", "Alarm Clock");
			Task.WriteTasks("Tasks-add.xml");
	        
			Console.WriteLine("-- Adding Task");
			Task.AddTask(6, "Have fun", 10, -1);
			Task.WriteTasks("Tasks-adj.xml");
	        
// Разбираем порядок сортировки
			Sort[] sort = Sort.LoadSorts();
/*	        Sort[] sort = new Sort[3];
	        sort[0] = new Sort();
	        sort[0].Property = "id";
	        sort[0].Direction = "Asc";
	        sort[1] = new Sort();
	        sort[1].Property = "weight";
	        sort[1].Direction = "Dsc";
	        sort[2] = new Sort();
	        sort[2].Property = "title";
	        sort[2].Direction = "Dsc";
*/	        
			for (int j = 0; j < sort.Length; j++) {
				Console.WriteLine("{0} - {1}", sort[j].Property, sort[j].Direction);
			}
	        
			List<Task> NTasks = Sort.DoSort<Task>(tasks, sort).ToList();
	            
			Console.WriteLine("----->");
//	        List<Task> SortedTasks = Tasks.OrderBy(t => t.id).ThenBy(t => t.weight).ToList();
			foreach (Task a in NTasks)
				Console.WriteLine("ID: {0}, Title: {1}, Weight: {2}, Parent {3}", a.id, a.title, a.weight, a.parent);
			Console.ReadKey(true);
		}
	}
}