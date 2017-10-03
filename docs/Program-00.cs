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
		public class Task
		{
			public int id {get; set; }
			public string title {get; set; }
			public int weight {get; set; }
			public int parent { get; set; }
		}
		
		public class Sort
		{
		    public string Property { get; set; }
		    public string Direction { get; set; }
		}
		
		public static IEnumerable<Task> DoSort<Task>(IEnumerable<Task> Tsks, Sort[] sort)
		{
		    IOrderedEnumerable<Task> temp = null;
		    foreach (var s in sort)
		    {
		        Func<Task, IComparable> keySelector = GetKeySelector<Task>(s.Property);
		
		        if (temp == null)
		        {
		            temp = s.Direction == "Asc" ?
		                    Tsks.OrderBy(keySelector) :
		                    Tsks.OrderByDescending(keySelector);
		        }
		        else
		        {
		            temp = s.Direction == "Asc" ?
		                    temp.ThenBy(keySelector) :
		                    temp.ThenByDescending(keySelector);
		        }
		    }
		
		    return temp ?? Tsks;
		}
		
		private static Func<Task, IComparable> GetKeySelector<Task>(string property)
		{
		    var param = Expression.Parameter(typeof(Task));
		    var lambda = Expression.Lambda<Func<Task, IComparable>>(
		        Expression.Convert(
		            Expression.Property(param, property),
		            typeof(IComparable)),
		        param);
		    return lambda.Compile();
		}
		
		public static void AddTask (XmlDocument xDoc, List <Task> Tasks, int id, string title, int weight, int parent)
		{
			Console.WriteLine("Adding...");
/*			Task task = new Task();
			task.id = id;
			task.title = title;
			task.weight = weight;
			task.parent = parent;
			Tasks.Add(task);
*/			XmlElement xRoot = xDoc.DocumentElement;
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
			
			xDoc.Save("Tasks-mod.xml");
		}
		
		public static void ChangeTask(int id, string WhichParam, string NewParam)
		{
			XDocument xDoc = XDocument.Load("Tasks.xml");
			XElement root = xDoc.Element("tasks");
			foreach (XElement e in root.Elements("task").ToList())
			{
				if (e.Element("id").Value == id.ToString())
				{
					Console.WriteLine("Found: " + e.Element(WhichParam).Value);
					e.Element(WhichParam).Value = NewParam;
					Console.WriteLine("Replaced with: " + e.Element(WhichParam).Value);
				}
				else Console.WriteLine("Not found");
			}
			xDoc.Save("Tasks-adj.xml");
		}
		
		public static void Main(string[] args)
		{
			List <Task> Tasks = new List <Task>();
			
			XmlDocument xDoc0 = new XmlDocument();
			xDoc0.Load("Tasks.xml");
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
	            Tasks.Add(task);
	        }
	        Console.WriteLine("Total of {0} tasks", Tasks.Count);
	        foreach (Task a in Tasks)
	            Console.WriteLine("ID: {0}, Title: {1}, Weight: {2}, Parent: {3}", a.id, a.title, a.weight, a.parent);

	        ChangeTask(1, "title", "Alarm Clock");
	        
	        Console.WriteLine("-- Adding Task");
	        AddTask(xDoc0, Tasks, 6, "Have fun", 10, -1);
	        
// Разбираем порядок сортировки

	        Sort[] sort = new Sort[3];
			
			XmlDocument xDoc1 = new XmlDocument();
			xDoc1.Load("Sorting.xml");
	        // получим корневой элемент
	        XmlElement xRoot1 = xDoc1.DocumentElement;
	        
	        // обход всех узлов в корневом элементе
	        int i = 0;
	        foreach(XmlNode xnode in xRoot1)
	        {
//	        	Sort sort = new Sort();
	        	sort[i] = new Sort();
	        	
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
	            	// если узел - property
	                if(childnode.Name == "property")
	                {
	                    Console.WriteLine("Property: {0}", childnode.InnerText);
//	                    sort.Property = childnode.InnerText;
	                    sort[i].Property = childnode.InnerText;
	                }
	            	
	                // если узел - direction
	                if(childnode.Name == "direction")
	                {
	                    Console.WriteLine("Направление: {0}", childnode.InnerText);
//	                    sort.Direction = childnode.InnerText;
	                    sort[i].Direction = childnode.InnerText;
	                }
	            }
//	            Sorts.Add(sort);
	            i++;
	        }
			
	        for (int j=0; j<3; j++)
	        {
	        	Console.WriteLine("{0} - {1}", sort[j].Property, sort[j].Direction);
	        }

	        
	        
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
	        List<Task> NTasks = DoSort<Task>(Tasks, sort).ToList();

	            
	        Console.WriteLine("----->");
//	        List<Task> SortedTasks = Tasks.OrderBy(t => t.id).ThenBy(t => t.weight).ToList();
	        foreach (Task a in NTasks)
	        	Console.WriteLine("ID: {0}, Title: {1}, Weight: {2}, Parent {3}", a.id, a.title, a.weight, a.parent);
	        Console.ReadKey(true);
		}
	}
}