/*
 * Created by SharpDevelop.
 * User: mgerdov
 * Date: 10/3/2017
 * Time: 10:11 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml;
using System.Xml.Linq;

namespace TaskManager
{
	/// <summary>
	/// Description of Task.
	/// </summary>
	public class Task
	{
		public int id { get; set; }
		public string title { get; set; }
		public int weight { get; set; }
		public int parent { get; set; }

		private static List <Task> allTasks = null;
		
		public static List <Task> LoadTasks(string fileName = "Tasks.xml")
		{
			List <Task> tasks = new List <Task>();
			try {
				XmlDocument xDoc0 = new XmlDocument();
				xDoc0.Load(Program.DataDir + fileName);
				// получим корневой элемент
				XmlElement xRoot0 = xDoc0.DocumentElement;
				// обход всех узлов в корневом элементе
				foreach (XmlNode xnode in xRoot0) {
					Task task = new Task();
					// получаем атрибут name
/*	            if(xnode.Attributes.Count>0)
	            {
	                XmlNode attr = xnode.Attributes.GetNamedItem("id");
	                if (attr!=null)
	                    Console.WriteLine(attr.Value);
	            }
*/	            // обходим все дочерние узлы элемента user
					foreach (XmlNode childnode in xnode.ChildNodes) {
						// если узел - id
						if (childnode.Name == "id") {
							Console.WriteLine("ID: {0}", childnode.InnerText);
							task.id = Int32.Parse(childnode.InnerText);
						}
	            	
						// если узел - title
						if (childnode.Name == "title") {
							Console.WriteLine("Название: {0}", childnode.InnerText);
							task.title = childnode.InnerText;
						}
	                
						// если узел - weight
						if (childnode.Name == "weight") {
							Console.WriteLine("Вес: {0}", childnode.InnerText);
							task.weight = Int32.Parse(childnode.InnerText);
						}
	                
						// если узел - parent
						if (childnode.Name == "parent") {
							Console.WriteLine("Parent: {0}", childnode.InnerText);
							task.parent = Int32.Parse(childnode.InnerText);
						}
					}
					tasks.Add(task);
				}
				// all good
				return tasks;
			} catch (Exception e) {
				Console.WriteLine("ERROR while loading tasks: '{0}'", e);
				return null;
			}
		}
	
		public static bool WriteTasks(string fileName = "Tasks.xml")
		{
			try {
				// write into new XML file
				XmlDocument xDoc = new XmlDocument();
				XmlNode xRoot = xDoc.CreateElement("tasks");
				xDoc.AppendChild(xRoot);
				
				foreach (Task task in allTasks) {
            		            	
					XmlElement NewTask = xDoc.CreateElement("task");
			
					// Создаём элементы
					XmlElement ElemId = xDoc.CreateElement("id");
					XmlElement ElemTitle = xDoc.CreateElement("title");
					XmlElement ElemWeight = xDoc.CreateElement("weight");
					XmlElement ElemParent = xDoc.CreateElement("parent");
			
					// Создаём текстовые значения
					XmlText IdText = xDoc.CreateTextNode(task.id.ToString());
					XmlText TitleText = xDoc.CreateTextNode(task.title);
					XmlText WeightText = xDoc.CreateTextNode(task.weight.ToString());
					XmlText ParentText = xDoc.CreateTextNode(task.parent.ToString());
			
					// Добавляем узлы
					ElemId.AppendChild(IdText);
					ElemTitle.AppendChild(TitleText);
					ElemWeight.AppendChild(WeightText);
					ElemParent.AppendChild(ParentText);
			
					// Добавляем элементы
					NewTask.AppendChild(ElemId);
					NewTask.AppendChild(ElemTitle);
					NewTask.AppendChild(ElemWeight);
					NewTask.AppendChild(ElemParent);
			
					// Добавляем в корень
					xRoot.AppendChild(NewTask);
				}
				
				xDoc.Save(Program.DataDir + fileName);
				
				// all good
				return true;
			} catch (Exception e) {
				Console.WriteLine("ERROR while saving tasks: '{0}'", e);
				return false;
			}			
		}
		
		public static List <Task> GetTasks()
		{
			if(allTasks == null)
			{
				// no tasks yet, trye to load them first
				allTasks = LoadTasks();
			}
			return allTasks;
		}
		
		public static Task AddTask(int id, string title, int weight, int parent)
		{
			// add to collection
			Task task = new Task();
			task.id = id;
			task.title = title;
			task.weight = weight;
			task.parent = parent;
			Console.WriteLine("Adding..."  + task);
			allTasks.Add(task);
			return task;
		}
		
		public static Task ChangeTask(int id, string whichParam, object newValue)
		{
			// replace in collection
			Task t = FindTaskById(id);
			if(t == null)
			{
				Console.WriteLine("Not found");
				return null;		// no such task
			}
			Console.WriteLine("Changing..."  + t);
			t.GetType().GetProperty(whichParam).SetValue(t, newValue, null);	//TODO: validate reflection result
			return t;
		}
		
		public static Task FindTaskById(int id)
		{
			foreach(Task t in allTasks)
			{
				if(t.id == id)
					return t;
			}
			return null;
		}
	}
}
