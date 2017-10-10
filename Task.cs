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

		public static List<Task> LoadTasks()
		{
			List <Task> tasks = new List <Task>();
			
			XmlDocument xDoc0 = new XmlDocument();
			xDoc0.Load(Program.DataDir + "Tasks.xml");
	        // получим корневой элемент
	        XmlElement xRoot0 = xDoc0.DocumentElement;
	        // обход всех узлов в корневом элементе
	        foreach(XmlNode xnode in xRoot0)
	        {
	        	Task task = new Task();
	        	// получаем атрибут name
/*	            if(xnode.Attributes.Count>0)
	            {
	                XmlNode attr = xnode.Attributes.GetNamedItem("id");
	                if (attr!=null)
	                    Console.WriteLine(attr.Value);
	            }
*/	            // обходим все дочерние узлы элемента user
	            foreach(XmlNode childnode in xnode.ChildNodes)
	            {
	            	// если узел - id
	                if(childnode.Name == "id")
	                {
	                    Console.WriteLine("ID: {0}", childnode.InnerText);
	                    task.id = Int32.Parse(childnode.InnerText);
	                }
	            	
	                // если узел - title
	                if(childnode.Name == "title")
	                {
	                    Console.WriteLine("Название: {0}", childnode.InnerText);
	                    task.title = childnode.InnerText;
	                }
	                
	                // если узел - weight
	                if (childnode.Name == "weight")
	                {
	                    Console.WriteLine("Вес: {0}", childnode.InnerText);
	                    task.weight = Int32.Parse(childnode.InnerText);
	                }
	                
	                // если узел - parent
	                if(childnode.Name == "parent")
	                {
	                    Console.WriteLine("Parent: {0}", childnode.InnerText);
	                    task.parent = Int32.Parse(childnode.InnerText);
	                }
	            }
	            tasks.Add(task);
	        }
	        return tasks;
		}
	
		public static void AddTask(List <Task> tasks, int id, string title, int weight, int parent)
		{
			// add to collection
			Task task = new Task();
			task.id = id;
			task.title = title;
			task.weight = weight;
			task.parent = parent;
			tasks.Add(task);
			Console.WriteLine("Adding..."  + task);

			// add to XML
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load(Program.DataDir + "Tasks.xml");
			XmlElement xRoot = xDoc.DocumentElement;
			
			XmlElement NewTask = xDoc.CreateElement("task");
			
			// Создаём элементы
			XmlElement ElemId = xDoc.CreateElement("id");
			XmlElement ElemTitle = xDoc.CreateElement("title");
			XmlElement ElemWeight = xDoc.CreateElement("weight");
			XmlElement ElemParent = xDoc.CreateElement("parent");
			
			// Создаём текстовые значения
			XmlText IdText = xDoc.CreateTextNode(id.ToString());
			XmlText TitleText = xDoc.CreateTextNode(title);
			XmlText WeightText = xDoc.CreateTextNode(weight.ToString());
			XmlText ParentText = xDoc.CreateTextNode(parent.ToString());
			
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
			
			xDoc.Save(Program.DataDir + "Tasks-mod.xml");
		}
		
		public static void ChangeTask(List <Task> tasks, int id, string whichParam, object newValue)
		{
			// replace in collection
			Task t = FindTaskById(tasks, id);
			if(t == null)
			{
				Console.WriteLine("Not found");
				return;		// no such task
			}
			t.GetType().GetProperty(whichParam).SetValue(t, newValue, null);	//TODO: validate reflection result
			
			// replace in XML
			XDocument xDoc = XDocument.Load(Program.DataDir + "Tasks.xml");
			XElement root = xDoc.Element("tasks");
			foreach (XElement e in root.Elements("task").ToList())
			{
				if (e.Element("id").Value == id.ToString())
				{
					Console.WriteLine("Found: " + e.Element(whichParam).Value);
					e.Element(whichParam).Value = newValue.ToString();
					Console.WriteLine("Replaced with: " + e.Element(whichParam).Value);
				}
				else Console.WriteLine("Not found");
			}
			xDoc.Save(Program.DataDir + "Tasks-adj.xml");
		}
		
		public static Task FindTaskById(List <Task> tasks, int id)
		{
			foreach(Task t in tasks)
			{
				if(t.id == id)
					return t;
			}
			return null;
		}
		
	}
}
